using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Conexiones
{
    public class ConexionMySQL
    {
        protected readonly string cadenaDeConexion;
        private Configuracion _configuracion;
        public ConexionMySQL()
        {
            string ruta = "config.json";
            string jsonDeConfiguracion = File.ReadAllText(ruta);
            _configuracion = JsonSerializer.Deserialize<Configuracion>(jsonDeConfiguracion);
            string contrasena = _configuracion.ContrasenaBD;
            string baseDeDatos = _configuracion.BaseDeDatos;
            string usuario = _configuracion.UsuarioBD;
            cadenaDeConexion = $"Server={_configuracion.IPHOST};Database={baseDeDatos};User={usuario};Password={contrasena};";
        }
        /*stackTrace informe que muestra la lista de funciones o métodos que se estaban ejecutando en un programa justo cuando ocurrió un error o una excepción*/

        private void EscribirEnTXTError(HistorialErrores ERROR)
        {
            string ruta = "log_error.txt";
            DateTime hoy = DateTime.Now;
            string logError = File.ReadAllText(ruta);
            logError += "{" + $"fecha:{hoy.ToString("dd-MM-yyyy HH:mm:ss")}, " +
                $"mensaje: {ERROR.Mensaje}," +
                $"tipo_error: {ERROR.TipoError}," +
                $"stack_trace: {ERROR.StackTrace}," +
                $"namespace: {ERROR.NameSpace}," +
                $"clase: {ERROR.Clase}," +
                $"metodo: {ERROR.Metodo}," +
                $"usuario: {ERROR.Cajero}" + "}" + "\n\n";
            File.WriteAllText(ruta, logError);
        }
        protected async Task RegistrarErrorBDLocal(HistorialErrores ERROR)
        {
            try
            {
                using (var con = new MySqlConnection(cadenaDeConexion))
                {
                    con.Open();
                    string registrarError = "INSERT INTO historial_errores (mensaje, tipo_error, stack_trace, namespace, clase, metodo, usuario) VALUES(@mensaje, @tipo_error, @stack_trace, @namespace, @clase, @metodo, @usuario);";
                    MySqlCommand cmd = new MySqlCommand(registrarError, con);
                    cmd.Parameters.AddWithValue("@mensaje", ERROR.Mensaje);
                    cmd.Parameters.AddWithValue("@tipo_error", ERROR.TipoError);
                    cmd.Parameters.AddWithValue("@stack_trace", ERROR.StackTrace);
                    cmd.Parameters.AddWithValue("@namespace", ERROR.NameSpace);
                    cmd.Parameters.AddWithValue("@clase", ERROR.Clase);
                    cmd.Parameters.AddWithValue("@metodo", ERROR.Metodo);
                    cmd.Parameters.AddWithValue("@usuario", ERROR.Cajero);
                    int n = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                string ruta = "config.json";
                string jsonDeConfiguracion = File.ReadAllText(ruta);
                _configuracion = JsonSerializer.Deserialize<Configuracion>(jsonDeConfiguracion);
                HistorialErrores error = new HistorialErrores(ex.Message, ex.GetType().FullName, ex.StackTrace, "Conexiones", "ConexionMySQL", "RegistrarErrorBDLocal", _configuracion.NombreDeCaja);
                EscribirEnTXTError(error);
            }
        }
    }
}
