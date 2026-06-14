using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Conexiones
{
    public class ConexionMySQL
    {
        protected readonly string cadenaDeConexion;
        public ConexionMySQL()
        {
            cadenaDeConexion = "Server=127.0.0.1;Database=muoqaclientes;User=root;Password=MyLH#1986J;";
        }
    }
}
