using Entidades;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Globalization;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Conexiones
{
    public class Con_Productos : ConexionMySQL
    {
        #region Con_CargarProducto
        public async Task<(bool estado, string mensaje)> Con_CargarProducto(Productos producto)
        {
            using(var con = new MySqlConnection(cadenaDeConexion))
            {
                try
                {
                    con.Open();
                    int idCategoriaExistente = await Con_ObtenerCategoriaProducto(con, producto.Codigo);
                    int idCategoriaFinal = await Con_ObtenerIDCategoria(con, producto.Categoria);
                    string t = await Con_ObtenerUnidadMedidaPorCodigo(con, producto.Codigo);
                    if(!string.IsNullOrWhiteSpace(t) && t != producto.UnidadMedida)
                        return (estado: false, mensaje: "El producto ya está registrado y" +
                            " la 'unidad de medida' que intentas asignar no coincide con la 'unidad de medida'" +
                            " existente. Para continuar, puedes cambiar la 'unidad de medida' del" +
                            " producto que estás intentando cargar o actualizar la del producto ya registrado.");
                    if (idCategoriaExistente > 0  && idCategoriaExistente != idCategoriaFinal)
                        return (estado: false, mensaje: "El producto ya está registrado y"+
                            " la 'categoría' que intentas asignar no coincide con la 'categoría'" +
                            " existente. Para continuar, puedes cambiar la 'categoría' del" +
                            " producto que estás intentando cargar o actualizar la del producto ya registrado.");
                    int IDCategoria = idCategoriaFinal;
                    string insertarProductos = "INSERT INTO Productos (Nombre, IDCategoria, Codigo, Venta, Compra, Cantidad, Proveedor, Mostrar, Ingreso, Vencimiento, UnidadMedida)"+
                        " VALUES(@nombre, @categoria, @codigo, @venta, @compra, @cantidad, @proveedor, \"SI\", @ingreso, @vencimiento, @unidad);";
                    MySqlCommand cmd = new MySqlCommand(insertarProductos, con);
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@categoria", IDCategoria);
                    cmd.Parameters.AddWithValue("@codigo", producto.Codigo);
                    cmd.Parameters.AddWithValue("@venta", producto.Venta);
                    cmd.Parameters.AddWithValue("@compra", producto.Compra);
                    cmd.Parameters.AddWithValue("@cantidad", producto.Cantidad);
                    cmd.Parameters.AddWithValue("@proveedor", producto.Proveedor);
                    cmd.Parameters.AddWithValue("@ingreso", producto.Ingreso);
                    cmd.Parameters.AddWithValue("@vencimiento", producto.Vencimiento);
                    cmd.Parameters.AddWithValue("@@unidad", producto.UnidadMedida);
                    var resultado = await cmd.ExecuteNonQueryAsync();
                    if(resultado>0)
                        return (true, "Se cargo exitosamente");
                    return (false, "No se pudo cargar el producto correctamente");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return (false, ex.Message);
                }
            }
        }
        private async Task<string> Con_ObtenerUnidadMedidaPorCodigo(MySqlConnection con, string codigo)
        {
            string obtenerUnidadMedida = "SELECT UnidadMedida FROM Productos WHERE Codigo = @codigo";
            MySqlCommand cmd = new MySqlCommand(obtenerUnidadMedida, con);
            cmd.Parameters.AddWithValue("@codigo", codigo);
            string t = "";
            using (MySqlDataReader datos = cmd.ExecuteReader())
            {
                if (datos.Read())
                {
                    t = Convert.ToString(datos["UnidadMedida"]);
                }
            }
            return t;
        }
        private async Task<int> Con_ObtenerIDCategoria(MySqlConnection con, string nombreCategoria)
        {
            string obtenerID = "SELECT ID FROM Categorias WHERE Nombre = @nombre";
            MySqlCommand cmd = new MySqlCommand(obtenerID, con);
            cmd.Parameters.AddWithValue("@nombre", nombreCategoria);
            int n = -1;
            using(MySqlDataReader datos = cmd.ExecuteReader())
            {
                if (datos.Read())
                {
                    n = Convert.ToInt32(datos["ID"]);
                }
            }
            Debug.WriteLine("Con_Productos - Con_ObtenerIDCategoria. n: " + n);
            return n;
            
        }
        private async Task<int> Con_ObtenerCategoriaProducto(MySqlConnection con, string codigo)
        {
            string query = "SELECT IDCategoria FROM Productos WHERE Codigo = @codigo";
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@codigo", codigo);
            int n = -1;
            using (MySqlDataReader datos = cmd.ExecuteReader())
            {
                if (datos.Read())
                {
                    n = Convert.ToInt32(datos["IDCategoria"]);
                }
            }
            return n;
        }
        #endregion
        #region Con_RevisarExistenciaProducto
        public async Task<(bool estado, bool existencia, string mensaje, Productos producto)> Con_RevisarExistenciaProducto(string codigo)
        {
            using (MySqlConnection con = new MySqlConnection(cadenaDeConexion))
            {
                try
                {
                    con.Open();
                    string revisarExistencia = "SELECT * FROM ( SELECT p.*, ROW_NUMBER() OVER (PARTITION BY Codigo ORDER BY Ingreso DESC) AS rn FROM Productos p) t WHERE rn = 1 AND Codigo = @codigo;";
                    MySqlCommand cmd = new MySqlCommand(revisarExistencia, con);
                    cmd.Parameters.AddWithValue("@codigo", codigo);
                    Productos pObtenido = new Productos();
                    //Aca tendria que iterar 1 sola vez, nose si es la forma mas optima para hacerlo
                    using (MySqlDataReader lector = cmd.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            pObtenido = new Productos()
                            {
                                Codigo = lector.GetString("Codigo"),
                                Nombre = lector.GetString("Nombre"),
                                Proveedor = lector.GetString("Proveedor"),
                                IDCategoria = lector.GetInt32("IDCategoria"),
                                UnidadMedida = lector.GetString("UnidadMedida")
                            };
                            var listaCate = await Con_ObtenerCategorias();
                            List<Categorias> listaCompleta = listaCate.lista;
                            for (int j = 0; j < listaCompleta.Count; j++)
                            {
                                if (listaCompleta[j].ID == pObtenido.IDCategoria)
                                {
                                    pObtenido.Categoria = listaCompleta[j].Nombre;
                                    break;
                                }
                            }
                            return (true, true, "Producto existente en el sistema", pObtenido);
                        }
                    }
                    return (true, false, "Registrando un producto nuevo", new Productos());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return (false, false, ex.Message,new Productos());
                }
            }
        }
        #endregion
        #region Con_EditarProductoPorCodigo
        public async Task<(bool estado, string mensaje)> Con_EditarProductoPorCodigo(Productos producto)
        {
            using (MySqlConnection con = new MySqlConnection(cadenaDeConexion))
            {
                try
                {
                    con.Open();
                    int idCategoriaFinal = await Con_ObtenerIDCategoria(con, producto.Categoria);
                    string editarTodosLosLotes = "UPDATE Productos SET Nombre = @nombre, IDCategoria = @categoria WHERE Codigo = @codigo;";
                    MySqlCommand cmd = new MySqlCommand(editarTodosLosLotes, con);
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@categoria", idCategoriaFinal);
                    cmd.Parameters.AddWithValue("@codigo", producto.Codigo);
                    string editarProducto = "UPDATE Productos SET Compra = @compra, Venta = @venta, Mostrar = @mostrar WHERE ID = @id;";
                    MySqlCommand cmd2 = new MySqlCommand(editarProducto, con);
                    decimal precioCompra = decimal.Parse(producto.Compra, NumberStyles.Any, new CultureInfo("es-AR"));
                    decimal precioVenta = decimal.Parse(producto.Venta, NumberStyles.Any, new CultureInfo("es-AR"));
                    cmd2.Parameters.AddWithValue("@compra", precioCompra);
                    cmd2.Parameters.AddWithValue("@venta", precioVenta);
                    cmd2.Parameters.AddWithValue("@mostrar", producto.Mostrar);
                    cmd2.Parameters.AddWithValue("@id", producto.ID);
                    var resultado = cmd.ExecuteNonQuery();
                    var resultado2 = cmd2.ExecuteNonQuery();
                    if(resultado > 0 && resultado2 > 0)
                        return (true, "Se edito correctamente el producto y los lotes");
                    return (false, "No se pudo terminar la opreación");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return (false, ex.Message);
                }
            }
        }
        #endregion
        #region Con_EditarLote
        public async Task<(bool estado, string mensaje)> Con_EditarLote(Productos producto)
        {
            using (var con = new MySqlConnection(cadenaDeConexion))
            {
                try
                {
                    con.Open();
                    string editarLote = "UPDATE Productos SET Nombre = @nombre, Compra = @compra, Venta = @venta, Cantidad = @cantidad, Ingreso = @ingreso,"+
                        " Vencimiento = @vencimiento WHERE ID = @id;";
                    MySqlCommand cmd = new MySqlCommand(editarLote, con);
                    decimal precioCompra = decimal.Parse(producto.Compra, NumberStyles.Any, new CultureInfo("es-AR"));
                    decimal precioVenta = decimal.Parse(producto.Venta, NumberStyles.Any, new CultureInfo("es-AR"));
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@compra", precioCompra);
                    cmd.Parameters.AddWithValue("@venta", precioVenta);
                    cmd.Parameters.AddWithValue("@cantidad", producto.Cantidad);
                    cmd.Parameters.AddWithValue("@ingreso", producto.Ingreso);
                    cmd.Parameters.AddWithValue("@vencimiento", producto.Vencimiento);
                    cmd.Parameters.AddWithValue("@id", producto.ID);
                    var resultado = cmd.ExecuteNonQuery();
                    if(resultado > 0)
                        return (true, "Se modifico el lote correctamente");
                    return (true, "No se pudo modificar el lote");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return (false, ex.Message);
                }
            }
        }
        #endregion
        #region Con_CargarCategoria
        public async Task<(bool estado, string mensaje)> Con_CargarCategoria(Categorias categoria)
        {
            using (var con = new MySqlConnection(cadenaDeConexion))
            {
                try
                {
                    con.Open();    
                    string cargarCategoria = "INSERT INTO Categorias (Nombre, Mostrar) VALUES(@nombre, \"si\");";
                    MySqlCommand cmd = new MySqlCommand(cargarCategoria, con);
                    cmd.Parameters.AddWithValue("@nombre", categoria.Nombre);
                    var resultado = cmd.ExecuteNonQuery();
                    Debug.WriteLine(resultado);
                    return (true, "Pruebas");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return (false, ex.Message);
                }
            }
        }
        #endregion
        #region Con_EditarCategoria
        public async Task<(bool estado, string mensaje)> Con_EditarCategoria(Categorias categoria)
        {
            using (var con = new MySqlConnection(cadenaDeConexion)) 
            {
                try
                {
                    con.Open();
                    string insertarCategoria = "UPDATE Categorias SET Nombre = @nombre, Mostrar = @mostrar WHERE ID = @id";
                    MySqlCommand cmd = new MySqlCommand(insertarCategoria, con);
                    cmd.Parameters.AddWithValue("@nombre", categoria.Nombre);
                    cmd.Parameters.AddWithValue("@mostrar", categoria.Mostrar);
                    cmd.Parameters.AddWithValue("@id", categoria.ID);
                    var resultado = cmd.ExecuteNonQuery();
                    Debug.WriteLine(resultado);
                    return (true, "Pruebas");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return (false, ex.Message);
                }
            }
        }
        #endregion
        #region Con_ObtenerCategorias
        public async Task<(bool estado, string mensaje, List<Categorias> lista)> Con_ObtenerCategorias()
        {
            using (var con = new MySqlConnection(cadenaDeConexion))
            {
                try
                {
                    con.Open();
                    string obtenerCategorias = "SELECT ID, Nombre FROM Categorias WHERE Mostrar = \"si\"";
                    MySqlCommand cmd = new MySqlCommand(obtenerCategorias, con);
                    List<Categorias> categoriasObtenidas = new List<Categorias>();
                    using (MySqlDataReader lector = cmd.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            categoriasObtenidas.Add(new Categorias
                            {
                                ID = lector.GetInt32("ID"),
                                Nombre = lector.GetString("Nombre")
                            });
                        }
                    }
                    return (true, "Pruebas", categoriasObtenidas);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return (false, ex.Message, new List<Categorias>());
                }
            }
        }
        #endregion
        #region Con_BusquedaCategorias
        public async Task<(bool estado, string mensaje, List<Categorias> lista)> Con_BusquedaCategorias(string busqueda)
        {
            using (var con = new MySqlConnection(cadenaDeConexion))
            {
                try
                {
                    con.Open();
                    if (!string.IsNullOrEmpty(busqueda))
                        busqueda = $"%{busqueda}%";
                    string obtenerCategorias = "SELECT ID, Nombre, Mostrar FROM Categorias WHERE (Nombre LIKE @nombre  OR  @nombre = \"\")";
                    MySqlCommand cmd = new MySqlCommand(obtenerCategorias, con);
                    cmd.Parameters.AddWithValue("@nombre", busqueda);
                    List<Categorias> categoriasObtenidas = new List<Categorias>();
                    using (MySqlDataReader lector = cmd.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            categoriasObtenidas.Add(new Categorias
                            {
                                ID = lector.GetInt32("ID"),
                                Nombre = lector.GetString("Nombre"),
                                Mostrar = lector.GetString("Mostrar")
                            });
                        }
                    }
                    return (true, "Pruebas", categoriasObtenidas);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return (false, ex.Message, new List<Categorias>());
                }
            }
        }
        #endregion
        #region Con_BusquedaPorCodigoUltLote
        //Esta es para obtener el ultimo producto cargado, buscandolo apartir del codigo de barras
        public async Task<(bool estado, string mensaje, List<Productos> lista)> Con_BusquedaPorCodigoUltLote(string busqueda)
        {
            using (var con = new MySqlConnection(cadenaDeConexion))
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(busqueda))
                        throw new Exception("El codigo no esta definido");
                    con.Open();
                    // Aca selecciono el ultimo producto cargado de tal codigo de barras
                    string soli = "SELECT * FROM ( SELECT p.*, ROW_NUMBER() OVER (PARTITION BY Codigo ORDER BY Ingreso DESC) AS rn FROM Productos p) t WHERE rn = 1 AND Codigo = @codigo;";
                    MySqlCommand cmd = new MySqlCommand(soli, con);
                    cmd.Parameters.AddWithValue("@codigo", busqueda);
                    List<Productos> productos = new List<Productos>();
                    using (MySqlDataReader lector = cmd.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            productos.Add(new Productos()
                            {
                                ID = lector.GetInt32("ID"),
                                Mostrar = lector.GetString("Mostrar"),
                                Nombre = lector.GetString("Nombre"),
                                Codigo = lector.GetString("Codigo"),
                                Compra = lector.GetDecimal("Compra").ToString(),
                                Venta = lector.GetDecimal("Venta").ToString(),
                                Cantidad = lector.GetInt32("Cantidad"),
                                UnidadMedida = lector.GetString("UnidadMedida")
                            });
                        }
                    }
                    var listaCate = await Con_ObtenerCategorias();
                    List<Categorias> listaCompleta = listaCate.lista;
                    for (int i = 0; i < productos.Count; i++)
                    {
                        for (int j = 0; j < listaCompleta.Count; j++)
                        {
                            if (listaCompleta[j].ID == productos[i].IDCategoria)
                            {
                                productos[i].Categoria = listaCompleta[j].Nombre;
                                break;
                            }
                        }
                    }
                    return (true, "Pruebas", productos);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return (false, ex.Message, new List<Productos>());
                }
            }
        }
        #endregion
        #region Con_LotesPorIngreso
        //Aca realizamos las busqueda por lotes
        public async Task<(bool estado, string mensaje, List<Productos> lista)> Con_LotesPorIngreso(string codigo, DateTime? busqueda)
        {
            using (var con = new MySqlConnection(cadenaDeConexion))
            {
                try
                {
                    con.Open();
                    string obtenerLotes = "SELECT * FROM Productos WHERE Codigo = @codigo AND (Ingreso >= @busqueda OR @busqueda IS NULL);";
                    MySqlCommand cmd = new MySqlCommand(obtenerLotes, con);
                    cmd.Parameters.AddWithValue("@codigo", codigo);
                    cmd.Parameters.AddWithValue("@busqueda", busqueda); 
                    List<Productos> productos = new List<Productos>();
                    using (MySqlDataReader lector = cmd.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            productos.Add(new Productos()
                            {
                                ID = lector.GetInt32("ID"),
                                Mostrar = lector.GetString("Mostrar"),
                                Nombre = lector.GetString("Nombre"),
                                Compra = lector.GetDecimal("Compra").ToString(),
                                Venta = lector.GetDecimal("Venta").ToString(),
                                Ingreso = lector.GetDateTime("Ingreso"),
                                Vencimiento = lector.GetDateTime("Vencimiento"),
                                Cantidad = lector.GetInt32("Cantidad"),
                            });
                        }
                    }
                    var listaCate = await Con_ObtenerCategorias();
                    List<Categorias> listaCompleta = listaCate.lista;
                    for (int i = 0; i < productos.Count; i++)
                    {
                        for (int j = 0; j < listaCompleta.Count; j++)
                        {
                            if (listaCompleta[j].ID == productos[i].IDCategoria)
                            {
                                productos[i].Categoria = listaCompleta[j].Nombre;
                                break;
                            }
                        }
                    }
                    return (true, "Pruebas", productos);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return (false, ex.Message, new List<Productos>());
                }
            }
        }
        #endregion
        #region Con_ObtenerUltLotePorNombre
        //Esta es para buscar productos para editar cuando se busca por nombre
        public async Task<(bool estado, string mensaje, List<Productos> lista)> Con_ObtenerUltLotePorNombre(string nombre)
        {
            using (var con = new MySqlConnection(cadenaDeConexion))
            {
                try
                {
                    con.Open();
                    string obtenerUltLote = "SELECT ult.ID, ult.IDCategoria, ult.Nombre, ult.Codigo, ult.Compra, ult.Venta," +
                        " ult.Proveedor, ult.Mostrar, ult.Ingreso, ult.Vencimiento, tot.Cantidad" +
                        " FROM ( SELECT ID, IDCategoria, Nombre, Codigo, Compra, Venta, Proveedor, Mostrar, Ingreso, Vencimiento" +
                        " FROM ( SELECT p.*, ROW_NUMBER() OVER (PARTITION BY Codigo ORDER BY Ingreso DESC) AS rn FROM Productos p ) t" +
                        " WHERE rn = 1 AND (Nombre LIKE @nombre OR @nombre = \"\") )ult JOIN( SELECT Codigo, CAST(SUM(Cantidad) AS SIGNED) AS Cantidad" +
                        " FROM Productos GROUP BY Codigo ) tot ON ult.Codigo = tot.Codigo;";
                    MySqlCommand cmd = new MySqlCommand(obtenerUltLote, con);
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    List<Productos> productos = new List<Productos>();
                    using (MySqlDataReader lector = cmd.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            productos.Add(new Productos()
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
                            });
                        }
                    }
                    var listaCate = await Con_ObtenerCategorias();
                    List<Categorias> listaCompleta = listaCate.lista;
                    for (int i = 0; i < productos.Count; i++)
                    {
                        for (int j = 0; j < listaCompleta.Count; j++)
                        {
                            if (listaCompleta[j].ID == productos[i].IDCategoria)
                            {
                                productos[i].Categoria = listaCompleta[j].Nombre;
                                break;
                            }
                        }
                    }
                    return (true, "Pruebas", productos);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return (false, ex.Message, new List<Productos>());
                }
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
