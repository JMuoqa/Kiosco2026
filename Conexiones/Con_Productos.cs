using Entidades;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
namespace Conexiones
{
    public class Con_Productos
    {
        public async Task<(bool estado, string mensaje)> Con_CargarProducto(Productos producto)
        {
            using (var cliente = new HttpClient()) 
            {
                try
                {
                    var json = JsonSerializer.Serialize(new
                    {
                        nombre = producto.Nombre,
                        categoria = producto.Categoria,
                        codigo = producto.Codigo,
                        venta = producto.Venta,
                        compra = producto.Compra,
                        cantidad = producto.Cantidad,
                        proveedor = producto.Proveedor,
                        ingreso = producto.Ingreso,
                        vencimiento = producto.Vencimiento,
                        token = ObtenerTokenJWT()
                    });
                    var contenido = new StringContent(json, Encoding.UTF8, "application/json");
                    var respuesta = await cliente.PostAsync("https://muoqa.com/cliente-kiosco2026/cargar-producto", contenido);
                    if (respuesta.IsSuccessStatusCode)
                    {
                        //{ estado: true, mensaje: 'Se cargo el producto correctamente'};
                        string resultado = await respuesta.Content.ReadAsStringAsync();
                        var datos = JsonSerializer.Deserialize<ResProductos>(resultado);
                        if(!datos.estado)
                            return (false, datos.mensaje);
                        return (true, "Se cargo correctamente");
                    }
                    return (false, "Algo salio mal");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return (false, ex.Message);
                }
            }
        }
        public async Task<(bool estado, string mensaje)> Con_CargarCategoria(Categorias categoria)
        {
            using (var cliente = new HttpClient())
            {
                try
                {
                    var json = JsonSerializer.Serialize(new
                    {
                        nombre = categoria.Nombre,
                        token = ObtenerTokenJWT()
                    });
                    var contenido = new StringContent(json, Encoding.UTF8, "application/json");
                    var respuesta = await cliente.PostAsync("https://muoqa.com/cliente-kiosco2026/cargar-categoria", contenido);
                    if (respuesta.IsSuccessStatusCode) 
                    {
                        string resultado = await respuesta.Content.ReadAsStringAsync();
                        var datos = JsonSerializer.Deserialize<ResProductos>(resultado);
                        if (!datos.estado)
                            return (false, datos.mensaje);
                        return (true, "Se cargo correctamente");
                    }
                    return (false, "Algo salio mal");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return (false, ex.Message);
                }
            }
        }
        public async Task<(bool estado, string mensaje)> Con_EditarCategoria(Categorias categoria)
        {
            using (var cliente = new HttpClient())
            {
                try
                {
                    var json = JsonSerializer.Serialize(new
                    {
                        nombre = categoria.Nombre,
                        mostrar = categoria.Mostrar,
                        token = ObtenerTokenJWT()
                    });
                    var contenido = new StringContent(json, Encoding.UTF8, "application/json");
                    var respuesta = await cliente.PostAsync("https://muoqa.com/cliente-kiosco2026/cargar-categoria", contenido);
                    if (respuesta.IsSuccessStatusCode)
                    {
                        string resultado = await respuesta.Content.ReadAsStringAsync();
                        var datos = JsonSerializer.Deserialize<ResProductos>(resultado);
                        if (!datos.estado)
                            return (false, datos.mensaje);
                        return (true, "Se edito correctamente");
                    }
                    return (false, "Algo salio mal");
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
            using (var cliente = new HttpClient()) 
            {
                try
                {
                    string token = ObtenerTokenJWT();
                    var respuesta = await cliente.GetAsync($"https://muoqa.com/cliente-kiosco2026/obtener-categorias?token={token}");
                    if (respuesta.IsSuccessStatusCode) 
                    {
                        string resultado = await respuesta.Content.ReadAsStringAsync();
                        var datos = JsonSerializer.Deserialize<ResCategorias>(resultado);
                        return (true, "ok", datos.Categorias);
                    }
                    return (false, "Hubo un error en la respuesta", new List<Categorias>());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return (false, ex.Message, new List<Categorias>());
                }
            }
        }
        public async Task<(bool estado, string mensaje, List<Categorias> lista)> Con_ObtenerCategoriasClientes()
        {
            using (var cliente = new HttpClient()) 
            {
                try
                {
                    string token = ObtenerTokenJWT();
                    var respuesta = await cliente.GetAsync($"https://muoqa.com/cliente-kiosco2026/obtener-categorias-clientes?token={token}");
                    if (respuesta.IsSuccessStatusCode)
                    {
                        string resultado = await respuesta.Content.ReadAsStringAsync();
                        var datos = JsonSerializer.Deserialize<ResCategorias>(resultado);
                        return (true, "ok", datos.Categorias);
                    }
                    return (false, "Hubo un error en la respuesta", new List<Categorias>());
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
            using (var cliente = new HttpClient()) 
            {
                try
                {

                    string token = ObtenerTokenJWT();
                    var respuesta = await cliente.GetAsync($"https://muoqa.com/cliente-kiosco2026/busqueda-categorias?token={token}&busqueda={busqueda}");
                    if (respuesta.IsSuccessStatusCode) 
                    {
                        string resultado = await respuesta.Content.ReadAsStringAsync();
                        var datos = JsonSerializer.Deserialize<ResCategorias>(resultado);
                        return (true, "ok", datos.Categorias);
                    }
                    return (false, "Hubo un error en la respuesta", new List<Categorias>());
                }catch(Exception ex)
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
                        var consultaCategorias = await Con_ObtenerCategoriasGenerales();
                        var consultaCategoriasClientes = await Con_ObtenerCategoriasClientes();
                        List<Categorias> listaGeneral = consultaCategorias.lista;
                        List<Categorias> listaCliente = consultaCategoriasClientes.lista;
                        List<Categorias> listaCompleta = new List<Categorias>();
                        for (int i = 0; i < listaGeneral.Count; i++)
                        {
                            listaCompleta.Add(listaGeneral[i]);
                        }
                        for (int i = 0; i < listaCliente.Count; i++)
                        {
                            listaCompleta.Add(listaCliente[i]);
                        }
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
                        if (!datos.estado)
                            return (false, datos.mensaje, datos.Productos);
                        return (true, "ok", datos.Productos);
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
        public async Task<(bool estado, string mensaje, List<Productos> lista)> Con_ProductosPorIngreso(string codigo)
        {
            using (var cliente = new HttpClient())
            {
                try
                {
                    string token = ObtenerTokenJWT();
                    var respuesta = await cliente.GetAsync($"https://muoqa.com/cliente-kiosco2026/obtener-productos-por-ingreso?token={token}&codigo={codigo}");
                    if (respuesta.IsSuccessStatusCode)
                    {
                        string resultado = await respuesta.Content.ReadAsStringAsync();
                        var datos = JsonSerializer.Deserialize<ResProductosDos>(resultado);
                        //Aca se obtiene el nombre de la categoria ya que en la tabla productos solo guardamos el ID de la categoria
                        var consultaCategorias = await Con_ObtenerCategoriasGenerales();
                        var consultaCategoriasClientes = await Con_ObtenerCategoriasClientes();
                        List<Categorias> listaGeneral = consultaCategorias.lista;
                        List<Categorias> listaCliente = consultaCategoriasClientes.lista;
                        List<Categorias> listaCompleta = new List<Categorias>();
                        for (int i = 0; i < listaGeneral.Count; i++)
                        {
                            listaCompleta.Add(listaGeneral[i]);
                        }
                        for (int i = 0; i < listaCliente.Count; i++)
                        {
                            listaCompleta.Add(listaCliente[i]);
                        }
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
                        if (!datos.estado)
                            return (false, datos.mensaje, datos.Productos);
                        return (true, datos.mensaje, datos.Productos);
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
