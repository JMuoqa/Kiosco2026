using Entidades;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Conexiones
{
    public class Con_Ventas
    {
        public async Task<(bool estado, string mensaje)> Con_RegistrarVenta(List<Productos> pr, Ventas venta)
        {
            using (var cliente = new HttpClient()) 
            {
                try
                {
                    var json = JsonSerializer.Serialize(new
                    {
                        listaItems = pr,
                        cantidadTotal = venta.CantidadProductos,
                        precioTotal = venta.Total,
                        formaPago = venta.FormaDePago,
                        fecha = venta.Fecha,
                        token = ObtenerTokenJWT()
                    });
                    var contenido = new StringContent(json, Encoding.UTF8, "application/json");
                    var respuesta = await cliente.PostAsync("https://muoqa.com/cliente-kiosco2026/registrar-venta", contenido);
                    if (respuesta.IsSuccessStatusCode)
                    {
                        string resultado = await respuesta.Content.ReadAsStringAsync();
                        var datos = JsonSerializer.Deserialize<ResProductos>(resultado);
                        return (datos.estado, datos.mensaje);
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
        public string ObtenerTokenJWT()
        {
            string ruta = "config.json";
            string jsonDeConfiguracion = File.ReadAllText(ruta);
            Configuracion configuracion = JsonSerializer.Deserialize<Configuracion>(jsonDeConfiguracion);
            return configuracion.CodigoSeguridad;
        }
    }
}
