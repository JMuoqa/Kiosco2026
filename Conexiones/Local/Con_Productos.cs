using Entidades;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
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
                    if (idCategoriaExistente > 0  && idCategoriaExistente != idCategoriaFinal)
                        return (estado: false, mensaje: "El producto ya está registrado y"+
                            " la categoría que intentas asignar no coincide con la categoría"+
                            " existente. Para continuar, puedes cambiar la categoría del"+
                            " producto que estás intentando cargar o actualizar la categoría del producto ya registrado.");
                    int IDCategoria = idCategoriaFinal;
                    string insertarProductos = "INSERT INTO Productos (Nombre, IDCategoria, Codigo, Venta, Compra, Cantidad, Proveedor, Mostrar, Ingreso, Vencimiento)"+
                        " VALUES(@nombre, @categoria, @codigo, @venta, @compra, @cantidad, @proveedor, @mostrar, @ingreso, @vencimiento);";
                    MySqlCommand cmd = new MySqlCommand(insertarProductos, con);
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@categoria", producto.Categoria);
                    cmd.Parameters.AddWithValue("@categoria", producto.Categoria);
                    cmd.Parameters.AddWithValue("@codigo", producto.Codigo);
                    cmd.Parameters.AddWithValue("@venta", producto.Venta);
                    cmd.Parameters.AddWithValue("@compra", producto.Compra);
                    cmd.Parameters.AddWithValue("@cantidad", producto.Cantidad);
                    cmd.Parameters.AddWithValue("@proveedor", producto.Proveedor);
                    cmd.Parameters.AddWithValue("@mostrar", producto.Mostrar);
                    cmd.Parameters.AddWithValue("@ingreso", producto.Ingreso);
                    cmd.Parameters.AddWithValue("@vencimiento", producto.Vencimiento);
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
        #region Con_EditarProductoPorCodigo
        public async Task<(bool estado, string mensaje)> Con_EditarProductoPorCodigo(Productos producto)
        {
            using (MySqlConnection con = new MySqlConnection(cadenaDeConexion))
            {
                try
                {
                    int idCategoriaFinal = await Con_ObtenerIDCategoria(con, producto.Categoria);
                    string editarTodosLosLotes = "UPDATE Productos SET Nombre = @nombre, IDCategoria = @categoria WHERE Codigo = @codigo;";
                    MySqlCommand cmd = new MySqlCommand(editarTodosLosLotes, con);
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@categoria", producto.Categoria);
                    cmd.Parameters.AddWithValue("@codigo", producto.Codigo);
                    string editarProducto = "UPDATE Productos SET Compra = @compra, Venta = @venta, Mostrar = @mostrar WHERE ID = @id;";
                    MySqlCommand cmd2 = new MySqlCommand(editarProducto, con);
                    cmd2.Parameters.AddWithValue("@compra", producto.Compra);
                    cmd2.Parameters.AddWithValue("@venta", producto.Venta);
                    cmd2.Parameters.AddWithValue("@mostrar", producto.Mostrar);
                    cmd2.Parameters.AddWithValue("@id", producto.ID);
                    con.Open();
                    var resultado = cmd.ExecuteNonQuery();
                    var resultado2 = cmd2.ExecuteNonQuery();
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
        #region Con_EditarLote
        public async Task<(bool estado, string mensaje)> Con_EditarLote(Productos producto)
        {
            using (var con = new MySqlConnection(cadenaDeConexion))
            {
                try
                {

                    string editarLote = "UPDATE Productos SET Nombre = @nombre, Compra = @compra, Venta = @venta, Cantidad = @cantidad, Ingreso = @ingreso,"+
                        " Vencimiento = @vencimiento WHERE ID = @id;";
                    MySqlCommand cmd = new MySqlCommand(editarLote, con);
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@compra", producto.Compra);
                    cmd.Parameters.AddWithValue("@venta", producto.Venta);
                    cmd.Parameters.AddWithValue("@cantidad", producto.Cantidad);
                    cmd.Parameters.AddWithValue("@ingreso", producto.Ingreso);
                    cmd.Parameters.AddWithValue("@vencimiento", producto.Vencimiento);
                    cmd.Parameters.AddWithValue("@id", producto.ID);
                    con.Open();
                    var resultado = cmd.ExecuteNonQuery();
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
        public async Task<(bool estado, string mensaje, List<Categorias> lista)> Con_ObtenerCategoriasGenerales()
        {
            using (var con = new MySqlConnection(cadenaDeConexion))
            {
                try
                {
                    con.Open();
                    string obtenerCategorias = "SELECT ID, Nombre FROM Categorias WHERE AND Mostrar = \"si\"";
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
        public async Task<(bool estado, string mensaje, List<Productos> lista)> Con_BusquedaProductos(string busqueda)
        {

            using (var cliente = new HttpClient()) 
            {
                try
                {
                    string token = ObtenerTokenJWT();
                    var respuesta = await cliente.GetAsync($"https://muoqa.com/cliente-kiosco2026/busqueda-productos?token={token}&busqueda={busqueda}");
                    if (respuesta.IsSuccessStatusCode)
                    {
                        string resultado = await respuesta.Content.ReadAsStringAsync();
                        var datos = JsonSerializer.Deserialize<ResProductosDos>(resultado);
                        //Aca se obtiene el nombre de la categoria ya que en la tabla productos solo guardamos el ID de la categoria
                        List<Categorias> listaCompleta = await OrganizarCategorias();
                        for(int i = 0; i < datos.Productos.Count; i++)
                        {
                            for (int j = 0; j < listaCompleta.Count; j++)
                            {
                                if (listaCompleta[j].ID == datos.Productos[i].IDCategoria)
                                {
                                    datos.Productos[i].Categoria = listaCompleta[j].Nombre;
                                    break;
                                }
                            }
                        }
                        return (datos.estado, datos.mensaje, datos.Productos);
                    }
                    return (false, "Hubo un error en la respuesta", new List<Productos>());
                }
                catch (Exception ex) 
                {
                    Debug.WriteLine(ex.Message);
                    return (false, ex.Message, new List<Productos>());
                }
            }
        }
        //Esta sirve para resumir la busqueda de categorias
        private async Task<List<Categorias>> OrganizarCategorias()
        {
            var consultaCategorias = await Con_ObtenerCategoriasGenerales();
            var consultaCategoriasClientes = new List<Categorias>();//await Con_ObtenerCategoriasClientes();
            List<Categorias> listaGeneral = consultaCategorias.lista;
            List<Categorias> listaCliente = consultaCategoriasClientes; //.lista;
            List<Categorias> listaCompleta = new List<Categorias>();
            for (int i = 0; i < listaGeneral.Count; i++)
            {
                listaCompleta.Add(listaGeneral[i]);
            }
            for (int i = 0; i < listaCliente.Count; i++)
            {
                listaCompleta.Add(listaCliente[i]);
            }
            return listaCompleta;
        }

        public async Task<(bool estado, string mensaje, List<Productos> lista)> Con_ProductosPorIngreso(string codigo, DateTime? busqueda)
        {
            using (var cliente = new HttpClient())
            {
                try
                {
                    string token = ObtenerTokenJWT();
                    var respuesta = await cliente.GetAsync($"https://muoqa.com/cliente-kiosco2026/obtener-productos-por-ingreso?token={token}&codigo={codigo}&busqueda={busqueda}");
                    if (respuesta.IsSuccessStatusCode)
                    {
                        string resultado = await respuesta.Content.ReadAsStringAsync();
                        var datos = JsonSerializer.Deserialize<ResProductosDos>(resultado);
                        //Aca se obtiene el nombre de la categoria ya que en la tabla productos solo guardamos el ID de la categoria
                        List<Categorias> listaCompleta = await OrganizarCategorias();
                        for (int i = 0; i < datos.Productos.Count; i++)
                        {
                            for (int j = 0; j < listaCompleta.Count; j++)
                            {
                                if (listaCompleta[j].ID == datos.Productos[i].IDCategoria)
                                {
                                    datos.Productos[i].Categoria = listaCompleta[j].Nombre;
                                    break;
                                }
                            }
                        }
                        return (datos.estado, datos.mensaje, datos.Productos);
                    }
                    return (false, "Hubo un error en la respuesta", new List<Productos>());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return (false, ex.Message, new List<Productos>());
                }
            }
        }
        //Puede que esta funcion haga lo mismo que la de obtener ultimo lote por codigo
        public async Task<(bool estado, string mensaje, List<Productos> lista)> Con_BusquedaProductoPorCodigo(string codigo)
        {
            using (var cliente = new HttpClient())
            {
                try
                {
                    string token = ObtenerTokenJWT();
                    var respuesta = await cliente.GetAsync($"https://muoqa.com/cliente-kiosco2026/busqueda-por-codigo?codigo={codigo}&token={token}");
                    if (respuesta.IsSuccessStatusCode)
                    {
                        string resultado = await respuesta.Content.ReadAsStringAsync();
                        var datos = JsonSerializer.Deserialize<ResProductosDos>(resultado);
                        List<Categorias> listaCompleta = await OrganizarCategorias();
                        for (int i = 0; i < datos.Productos.Count; i++)
                        {
                            for (int j = 0; j < listaCompleta.Count; j++)
                            {
                                if (listaCompleta[j].ID == datos.Productos[i].IDCategoria)
                                {
                                    datos.Productos[i].Categoria = listaCompleta[j].Nombre;
                                    break;
                                }
                            }
                        }
                        return (datos.estado, datos.mensaje, datos.Productos);
                    }
                    return (false, "Hubo un error en la respuesta", new List<Productos>());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return (false, ex.Message, new List<Productos>());
                } 
            }
        }
        public async Task<(bool estado, string mensaje, List<Productos> lista)> Con_ObtenerUltLotePorCodigo(string codigo)
        {
            using (var cliente = new HttpClient())
            {
                try
                {
                    string token = ObtenerTokenJWT();
                    var respuesta = await cliente.GetAsync($"https://muoqa.com/cliente-kiosco2026/obtener-ultimo-lote?codigo={codigo}&token={token}");
                    if(respuesta.IsSuccessStatusCode)
                    {
                        string resultado = await respuesta.Content.ReadAsStringAsync();
                        var datos = JsonSerializer.Deserialize<ResProductosDos>(resultado);
                        List<Categorias> listaCompleta = await OrganizarCategorias();
                        for (int i = 0; i < datos.Productos.Count; i++)
                        {
                            for (int j = 0; j < listaCompleta.Count; j++)
                            {
                                if (listaCompleta[j].ID == datos.Productos[i].IDCategoria)
                                {
                                    datos.Productos[i].Categoria = listaCompleta[j].Nombre;
                                    break;
                                }
                            }
                        }
                        return (datos.estado, datos.mensaje, datos.Productos);
                    }
                    return (false, "Hubo un error en la respuesta", new List<Productos>());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return (false, ex.Message, new List<Productos>());
                }
            }
        }
        public string ObtenerTokenJWT()
        {
            string ruta = "config.json";
            string jsonDeConfiguracion = File.ReadAllText(ruta);
            Configuracion configuracion = JsonSerializer.Deserialize<Configuracion>(jsonDeConfiguracion);
            return configuracion.CodigoSeguridad;
        }
    }
}
