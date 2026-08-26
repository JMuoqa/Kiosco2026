using Entidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kiosco2026.ModeloDeLasVistas.Hijas
{
    public class ModeloConfBD : ModeloDeVistaBase
    {
        private Configuracion _configuracion;
        public ModeloConfBD() 
        {
            string ruta = "config.json";
            string jsonDeConfiguracion = File.ReadAllText(ruta);
            _configuracion = JsonSerializer.Deserialize<Configuracion>(jsonDeConfiguracion);
            IpHost = _configuracion.IPHOST;
            NombreBaseDatos = _configuracion.BaseDeDatos;
            Usuario = _configuracion.UsuarioBD;
            Contrasena = _configuracion.ContrasenaBD;
        }
        private string _ipHost;
        public string IpHost
        {
            get => _ipHost;
            set
            {
                _ipHost = value;
                OnPropertyChanged(nameof(IpHost));
            }
        }
        private string _nombreBaseDatos;
        public string NombreBaseDatos
        {
            get => _nombreBaseDatos;
            set
            {
                _nombreBaseDatos = value;
                OnPropertyChanged(nameof(NombreBaseDatos));
            }
        }
        private string _usuario;
        public string Usuario
        {
            get => _usuario;
            set
            {
                _usuario = value;
                OnPropertyChanged(nameof(Usuario));
            }
        }
        private string _contrasena;
        public string Contrasena
        {
            get => _contrasena;
            set
            {
                _contrasena = value;
                OnPropertyChanged(nameof(Contrasena));
            }
        }
    }
}
