using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Conexiones
{
    public class Con_Ventas : ConexionMySQL
    {
        #region Con_RegistrarVenta
        public async Task<(bool estado, string mensaje)> Con_RegistrarVenta(List<Productos> pr, Ventas venta)
        {
            using (var conexion = new MySqlConnection(cadenaDeConexion))
            {
                try
                {
                    string registrarUsuario = "INSERT INTO ventas (fecha, items, cantidad_total_productos, forma_pago, total, id_usuario) VALUES( @fecha, @items, @cantidad_total, @forma_pago, @total); SELECT LAST_INSERT_ID();";
                    MySqlCommand cmd = new MySqlCommand(registrarUsuario, conexion);
                    cmd.Parameters.AddWithValue("@fecha", venta.Fecha);
                    cmd.Parameters.AddWithValue("@items", pr.Count);
                    cmd.Parameters.AddWithValue("@cantidad_total", venta.CantidadProductos);
                    cmd.Parameters.AddWithValue("@forma_pago", venta.FormaDePago);
                    cmd.Parameters.AddWithValue("@total", venta.Total);
                    conexion.Open(); 
                    var resultado = cmd.ExecuteNonQuery();
                    var ultimoIdVenta = cmd.LastInsertedId;
                    if(resultado>0)
                    {
                        for (int i = 0; i < pr.Count; i++) 
                        {
                            var resultadoDetalleVentas = await RestarProductoVendido(pr[i], conexion, ultimoIdVenta);
                        }
                    }
                    return (true, "Pruebas");
                }
                catch (Exception ex)
                {

                    Debug.WriteLine(ex.Message);
                    return (false, ex.Message);
                }
            }
        }
        public async Task<bool> RestarProductoVendido(Productos pr, MySqlConnection con, long ultimoIdVenta)
        {
            bool ciclo = true;
            while (ciclo)
            {
                string productoARestar = "SELECT * FROM (SELECT p.*, ROW_NUMBER() OVER (ORDER BY Ingreso ASC) AS rn FROM Productos p WHERE Codigo = @codigo AND Cantidad > 0) t WHERE rn = 1; ";
                MySqlCommand cmd = new MySqlCommand(productoARestar, con);
                cmd.Parameters.AddWithValue("@codigo", pr.Codigo);
                Productos primerProductoConStock = new Productos();
                using (MySqlDataReader lector = cmd.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        primerProductoConStock = new Productos()
                        {
                            ID = lector.GetInt32("ID"),
                            Mostrar = lector.GetString("Mostrar"),
                            Nombre = lector.GetString("Nombre"),
                            IDCategoria = lector.GetInt32("IDCategoria"),
                            Compra = lector.GetDecimal("Compra").ToString(),
                            Venta = lector.GetDecimal("Venta").ToString(),
                            Ingreso = lector.GetDateTime("Ingreso"),
                            Vencimiento = lector.GetDateTime("Vencimiento"),
                            Cantidad = lector.GetInt32("Cantidad"),
                            Codigo = lector.GetString("Codigo")
                        };
                    }
                }
                int cantidadARestar = pr.Cantidad;
                int cantidadDelPrimerProducto = primerProductoConStock.Cantidad;
                int resultadoDeResta = cantidadDelPrimerProducto - cantidadARestar;
                if (resultadoDeResta < 0)
                {
                    string actualizarProducto = "UPDATE Productos SET Cantidad = 0 WHERE ID = @id;";
                    MySqlCommand cmdActualizarProducto = new MySqlCommand(actualizarProducto, con);
                    cmdActualizarProducto.Parameters.AddWithValue("@id", primerProductoConStock.ID);
                    cmdActualizarProducto.ExecuteNonQuery();
                }
                else
                {
                    string actualizarProducto = "UPDATE Productos SET Cantidad = @cantidad WHERE ID = @id;";
                    MySqlCommand cmdActualizarProducto = new MySqlCommand(actualizarProducto, con);
                    cmdActualizarProducto.Parameters.AddWithValue("@id", primerProductoConStock.ID);
                    cmdActualizarProducto.Parameters.AddWithValue("@cantidad", resultadoDeResta);
                    cmdActualizarProducto.ExecuteNonQuery();
                    ciclo = false;
                }
                await CargarDetalleVenta(pr, cantidadARestar, con, ultimoIdVenta);
            }
            return true;
        }
        public async Task<bool> CargarDetalleVenta(Productos pr, int cantidadVendida, MySqlConnection con, long ultimoIdVenta)
        {

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
