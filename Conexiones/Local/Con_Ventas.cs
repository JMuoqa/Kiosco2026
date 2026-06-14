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
        public async Task<(bool estado, string mensaje)> Con_RegistrarVenta(List<Productos> pr, Ventas venta)
        {
            using (var conexion = new MySqlConnection(cadenaDeConexion))
            {
                try
                {
                    string registrarUsuario = "INSERT INTO ventas (fecha, items, cantidad_total_productos, forma_pago, total, id_usuario) VALUES( @fecha, @items, @cantidad_total, @forma_pago, @total);";
                    MySqlCommand cmd = new MySqlCommand(registrarUsuario, conexion);
                    cmd.Parameters.AddWithValue("@fecha", venta.Fecha);
                    cmd.Parameters.AddWithValue("@items", pr.Count);
                    cmd.Parameters.AddWithValue("@cantidad_total", venta.CantidadProductos);
                    cmd.Parameters.AddWithValue("@forma_pago", venta.FormaDePago);
                    cmd.Parameters.AddWithValue("@total", venta.Total);
                    conexion.Open(); 
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
        public string ObtenerTokenJWT()
        {
            string ruta = "config.json";
            string jsonDeConfiguracion = File.ReadAllText(ruta);
            Configuracion configuracion = JsonSerializer.Deserialize<Configuracion>(jsonDeConfiguracion);
            return configuracion.CodigoSeguridad;
        }
    }
}
