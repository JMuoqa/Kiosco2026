using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Conexiones
{
    public class Con_Ventas : ConexionMySQL
    {
        private Configuracion _configuracion { get; }
        private CultureInfo _cultura;
        string carpetaDatos = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Muoqa",
            "MuoqaStock"
        );
        public Con_Ventas()
        {
            _cultura = new CultureInfo("es-AR");
            string ruta = carpetaDatos + "\\config.json";
            string jsonDeConfiguracion = File.ReadAllText(ruta);
            _configuracion = JsonSerializer.Deserialize<Configuracion>(jsonDeConfiguracion);
        }
        #region Con_RegistrarVenta
        public async Task<(bool estado, string mensaje, long idV)> Con_RegistrarVenta(Ventas venta)
        {
            using (var conexion = new MySqlConnection(cadenaDeConexion))
            {
                MySqlTransaction transaccion = null;
                try
                {
                    conexion.Open();
                    transaccion = conexion.BeginTransaction();
                    string registrarUsuario = "INSERT INTO ventas (fecha, items, cantidad_total_productos, forma_pago, descuento, total) VALUES( @fecha, @items, @cantidad_total, @forma_pago, @descuento, @total);";
                    MySqlCommand cmd = new MySqlCommand(registrarUsuario, conexion, transaccion);
                    cmd.Parameters.AddWithValue("@fecha", venta.Fecha);
                    cmd.Parameters.AddWithValue("@items", venta.Productos.Count);
                    cmd.Parameters.AddWithValue("@cantidad_total", venta.CantidadProductos);
                    cmd.Parameters.AddWithValue("@forma_pago", venta.FormaDePago);
                    cmd.Parameters.AddWithValue("@descuento", venta.Descuento);
                    cmd.Parameters.AddWithValue("@total", venta.Total);
                    var resultado = cmd.ExecuteNonQuery();
                    var ultimoIdVenta = cmd.LastInsertedId;
                    if (resultado <= 0)
                        throw new Exception("No se pudo registrar la venta.");
                    for (int i = 0; i < venta.Productos.Count; i++)
                    {
                        await RestarProductoVendido(venta.Productos[i], conexion, ultimoIdVenta, transaccion);
                    }
                    transaccion.Commit();
                    return (true, "Se registro correctamente", ultimoIdVenta);
                }
                catch (Exception ex)
                {
                    if (transaccion != null)
                        transaccion.Rollback();
                    Debug.WriteLine(ex.Message);
                    HistorialErrores error = new HistorialErrores(ex.Message, ex.GetType().FullName, ex.StackTrace, "Conexiones", "Con_Ventas", "Con_RegistrarVenta", _configuracion.NombreDeCaja);
                    RegistrarErrorBDLocal(error);
                    return (false, ex.Message, 0);
                }
            }
        }
        public async Task<bool> RestarProductoVendido(Productos pr, MySqlConnection con, long ultimoIdVenta, MySqlTransaction transaccion)
        {
            bool ciclo = true;
            while (ciclo)
            {
                string productoARestar = "SELECT * FROM (SELECT p.*, ROW_NUMBER() OVER (ORDER BY Ingreso ASC, ID ASC) AS rn FROM Productos p WHERE Codigo = @codigo AND Cantidad > 0 AND Mostrar = 1) t WHERE rn = 1 FOR UPDATE;";
                MySqlCommand cmd = new MySqlCommand(productoARestar, con, transaccion);
                cmd.Parameters.AddWithValue("@codigo", pr.Codigo);
                Productos primerProductoConStock = new Productos();
                using (MySqlDataReader lector = cmd.ExecuteReader())
                {
                    bool objetoLlenado = false;
                    while (lector.Read())
                    {
                        primerProductoConStock = new Productos()
                        {
                            ID = lector.GetInt32("ID"),
                            Mostrar = lector.GetBoolean("Mostrar"),
                            Nombre = lector.GetString("Nombre"),
                            IDCategoria = lector.GetInt32("IDCategoria"),
                            Compra = lector.GetDecimal("Compra").ToString(_cultura),
                            Venta = lector.GetDecimal("Venta").ToString(_cultura),
                            Ingreso = lector.GetDateTime("Ingreso"),
                            Vencimiento = lector.GetDateTime("Vencimiento"),
                            Cantidad = lector.GetInt32("Cantidad"),
                            Codigo = lector.GetString("Codigo")
                        };
                        objetoLlenado = true;
                    }
                    if (!objetoLlenado)
                        primerProductoConStock = pr;
                }

                int cantidadARestar = pr.Cantidad;
                int cantidadDelPrimerProducto = primerProductoConStock.Cantidad; 
                int cantidadSacadaDelLote;
                int cantidadRestante;
                if (cantidadDelPrimerProducto < cantidadARestar)
                {
                    cantidadSacadaDelLote = cantidadDelPrimerProducto;
                    cantidadRestante = cantidadARestar - cantidadDelPrimerProducto;
                    pr.Cantidad = cantidadRestante;
                    string actualizarProducto = "UPDATE Productos SET Cantidad = 0 WHERE ID = @id AND Mostrar = 1; ";
                    MySqlCommand cmdActualizarProducto = new MySqlCommand(actualizarProducto, con, transaccion);
                    cmdActualizarProducto.Parameters.AddWithValue("@id", primerProductoConStock.ID);
                    cmdActualizarProducto.ExecuteNonQuery();
                }
                else
                {
                    cantidadSacadaDelLote = cantidadARestar;
                    cantidadRestante = 0; 
                    int cantidadNuevaDelLote = cantidadDelPrimerProducto - cantidadARestar;
                    string actualizarProducto = "UPDATE Productos SET Cantidad = @cantidad WHERE ID = @id AND Mostrar = 1;";
                    MySqlCommand cmdActualizarProducto = new MySqlCommand(actualizarProducto, con, transaccion);
                    cmdActualizarProducto.Parameters.AddWithValue("@id", primerProductoConStock.ID);
                    cmdActualizarProducto.Parameters.AddWithValue("@cantidad", cantidadNuevaDelLote);
                    cmdActualizarProducto.ExecuteNonQuery();
                    ciclo = false;
                }
                bool estadoDetalle = await CargarDetalleVenta(pr, primerProductoConStock, con, ultimoIdVenta, transaccion, cantidadSacadaDelLote);
                if (!estadoDetalle)
                    throw new Exception("Ocurrio un error al cargar el detalle de la venta");
            }
            return true;
        }
        public async Task<bool> CargarDetalleVenta(Productos pr, Productos primerProductoConStock, MySqlConnection con, long ultimoIdVenta, MySqlTransaction transaccion, int cantidadSacadaDelLote)
        {
            string cargarDetalleVentacar = "INSERT INTO detalle_ventas (id_venta, id_producto, codigo, cantidad, precio_unitario, subtotal) VALUES(@idventa, @idproducto, @codigo, @cantidad, @preciounitario, @subtotal);";
            decimal subTotal = decimal.Parse(pr.Venta, NumberStyles.Any, new CultureInfo("es-AR")) * pr.Cantidad;
            decimal venta = decimal.Parse(pr.Venta, NumberStyles.Any, new CultureInfo("es-AR"));
            MySqlCommand cmd = new MySqlCommand(cargarDetalleVentacar, con, transaccion);
            cmd.Parameters.AddWithValue("@idventa", ultimoIdVenta);
            cmd.Parameters.AddWithValue("@idproducto", primerProductoConStock.ID);
            cmd.Parameters.AddWithValue("@codigo", pr.Codigo);
            cmd.Parameters.AddWithValue("@cantidad", cantidadSacadaDelLote);
            cmd.Parameters.AddWithValue("@preciounitario", venta);
            cmd.Parameters.AddWithValue("@subtotal", subTotal);
            cmd.ExecuteNonQuery();
            return true;
        }
        #endregion
        #region Con_BuscarVentas
        public async Task<(bool estado, string mensaje, List<Ventas> ventas)> Con_BuscarVentas(DateTime? desde, DateTime? hasta)
        {
            try
            {
                using (var con = new MySqlConnection(cadenaDeConexion))
                {
                    con.Open();
                    string buscar = "SELECT * FROM Ventas WHERE (fecha >= @desde OR @desde IS NULL) AND (fecha <= @hasta OR @hasta IS NULL)";
                    MySqlCommand cmd = new MySqlCommand(buscar, con);
                    cmd.Parameters.AddWithValue("@desde", desde);
                    cmd.Parameters.AddWithValue("@hasta", hasta);
                    List<Ventas> listaVentas = new List<Ventas>();
                    using (MySqlDataReader lector = cmd.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            listaVentas.Add(new Ventas
                            {
                                Id = lector.GetInt32("id"),
                                Fecha = lector.GetDateTime("fecha"),
                                Items = lector.GetInt32("items"),
                                CantidadProductos = lector.GetInt32("cantidad_total_productos"),
                                FormaDePago = lector.GetString("forma_pago"),
                                Descuento = lector.GetDecimal("descuento"),
                                Total = lector.GetDecimal("total")
                            });
                        }
                    }
                    return (true, "Se obtuvieron los datos correctamente", listaVentas);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                HistorialErrores error = new HistorialErrores(ex.Message, ex.GetType().FullName, ex.StackTrace, "Conexiones", "Con_Ventas", "Con_RegistrarVenta", _configuracion.NombreDeCaja);
                RegistrarErrorBDLocal(error);
                return (false, ex.Message, new List<Ventas>());
            }
        }
        #endregion
        #region Con_BuscarDetalles
        public async Task<(bool estado, string mensaje, Ventas venta)> Con_BuscarDetalles(Ventas ventaGuia)
        {
            try
            {
                using (var con = new MySqlConnection(cadenaDeConexion))
                {
                    con.Open();
                    //Inner join establece que seleccione donde coinciden p.ID y dv.id_producto
                    string buscar = "SELECT dv.id_producto, dv.codigo, dv.cantidad, dv.precio_unitario, dv.subtotal," +
                        " p.Nombre FROM detalle_ventas dv INNER JOIN Productos p ON p.ID = dv.id_producto WHERE dv.id_venta = @id";
                    MySqlCommand cmd = new MySqlCommand(buscar, con);
                    cmd.Parameters.AddWithValue("@id", ventaGuia.Id);
                    List<Productos> lista = new List<Productos>();
                    using (MySqlDataReader lector = cmd.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Productos producto = new Productos
                            {

                                ID = lector.GetInt32("id_producto"),
                                Codigo = lector.GetString("codigo"),
                                Nombre = lector.GetString("Nombre"),
                                Cantidad = lector.GetInt32("cantidad"),
                                Venta = lector.GetDecimal("precio_unitario").ToString(_cultura),
                                SubTotal = lector.GetDecimal("subtotal")
                            };
                            lista.Add(producto);
                        }
                    }
                    Ventas v = new Ventas
                    {
                        Productos = lista
                    };
                    return (true, "Se obtuvieron los datos correctamente", v);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                HistorialErrores error = new HistorialErrores(ex.Message, ex.GetType().FullName, ex.StackTrace, "Conexiones", "Con_Ventas", "Con_RegistrarVenta", _configuracion.NombreDeCaja);
                RegistrarErrorBDLocal(error);
                return (false, ex.Message, new Ventas());
            }
        }
        #endregion
        public string ObtenerTokenJWT()
        {
            string ruta = "config.json";
            string jsonDeConfiguracion = File.ReadAllText(ruta);
            Configuracion configuracion = JsonSerializer.Deserialize<Configuracion>(jsonDeConfiguracion);
            return configuracion.CodigoSeguridad;
        }
    }
}
