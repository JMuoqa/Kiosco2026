using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
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
            string contrasena = _configuracion.Contrasena;
            string baseDeDatos = _configuracion.BaseDeDatos;
            string usuario = _configuracion.Usuario;
            cadenaDeConexion = $"Server=127.0.0.1;Database={baseDeDatos};User={usuario};Password={contrasena};";
        }
    }
}
