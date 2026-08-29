using Entidades;
using Kiosco2026.Clases.Colores;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Kiosco2026.ModeloDeLasVistas.Hijas
{
    public class ModeloConfBD : ModeloDeVistaBase
    {
        private Configuracion _configuracion;
        private string _ruta;
        public ModeloConfBD() 
        {
            _ruta = "config.json";
            string jsonDeConfiguracion = File.ReadAllText(_ruta);
            _configuracion = JsonSerializer.Deserialize<Configuracion>(jsonDeConfiguracion);
            ColoresEntradas = new ColoresProductos();
            GotFocus = new ComandoModeloDeVista(EjecutarGotFocus);
            LostFocus = new ComandoModeloDeVista(EjecutarLostFocus);
            GuardarDatos = new ComandoModeloDeVista(EjecutarGuardarDatos);
            EjecutarCargarDatos(null);
            ColoresEntradas.IpHost = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            ColoresEntradas.NombreBaseDatos = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            ColoresEntradas.UsuarioBD = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            ColoresEntradas.ContrasenaBD = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
        }
        #region Propiedades
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
        private ColoresProductos _coloresEntradas;
        public ColoresProductos ColoresEntradas
        {
            get => _coloresEntradas;
            set
            {
                _coloresEntradas = value;
                OnPropertyChanged(nameof(_coloresEntradas));
            }
        }
        #endregion
        #region Comandos
        public ICommand LostFocus { get; set; }
        public ICommand GotFocus { get; set; }
        public ICommand GuardarDatos { get; set; }
        #endregion
        #region Funciones
        private async Task EjecutarCargarDatos(object arg)
        {
            IpHost = _configuracion.IPHOST;
            NombreBaseDatos = _configuracion.BaseDeDatos;
            Usuario = _configuracion.UsuarioBD;
            Contrasena = _configuracion.ContrasenaBD;
        }
        private async Task EjecutarGuardarDatos (object arg)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NombreBaseDatos) || NombreBaseDatos == "Base de datos*")
                    throw new Exception("Hay campos vacios, llenalos para poder guardar");
                if (string.IsNullOrWhiteSpace(IpHost) || IpHost == "Server*")
                    throw new Exception("Hay campos vacios, llenalos para poder guardar");
                if (string.IsNullOrWhiteSpace(Usuario) || Usuario == "Usuario*")
                    throw new Exception("Hay campos vacios, llenalos para poder guardar");
                if (string.IsNullOrWhiteSpace(Contrasena) || Contrasena == "Contraseña*")
                    throw new Exception("Hay campos vacios, llenalos para poder guardar");
                _configuracion.BaseDeDatos = NombreBaseDatos;
                _configuracion.IPHOST = IpHost;
                _configuracion.UsuarioBD = Usuario;
                _configuracion.ContrasenaBD = Contrasena;
                //Convertimos de vuelta a formato json
                string nuevoJson = JsonSerializer.Serialize(_configuracion, new JsonSerializerOptions { WriteIndented = true });
                //Sobrescribimos el archivo con las nuevas modificaciones
                File.WriteAllText(_ruta, nuevoJson);
                MessageBox.Show(
                    "Se guardaron los datos correctamente",
                    "Guardado!",
                    MessageBoxButton.OK
                );
            }
            catch (Exception ex) 
            {
                MessageBox.Show(
                    ex.Message,
                    "Campos vacios",
                    MessageBoxButton.OK
                );
                HistorialErrores error = new HistorialErrores(ex.Message, ex.GetType().FullName, ex.StackTrace, "Kiosco2026.ModeloDeLasVistas.Hijas", "ModeloConfBD", "EjecutarGuardarDatos", _configuracion.NombreDeCaja);
                EscribirEnTXTError(error);
            }
        }
        #endregion
        #region Funciones graficas
        private async Task EjecutarGotFocus(object arg)
        {
            TextBox entrada = (TextBox)arg;
            if (RevisarEntradas(entrada))
            {
                switch (entrada.Name)
                {
                    case "entrada_iphost": IpHost = string.Empty; ColoresEntradas.IpHost = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")); break;
                    case "entrada_bd": NombreBaseDatos = string.Empty; ColoresEntradas.NombreBaseDatos = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")); break;
                    case "entrada_usuario": Usuario = string.Empty; ColoresEntradas.UsuarioBD = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")); break;
                    case "entrada_contrasena": Contrasena = string.Empty; ColoresEntradas.ContrasenaBD = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")); break;
                }
            }
        }
        private async Task EjecutarLostFocus(object arg)
        {
            string nombre = (string)arg;
            switch (nombre)
            {
                case "entrada_iphost": if (string.IsNullOrWhiteSpace(IpHost) || IpHost.Trim() == "Server*") { IpHost = "Server*"; ColoresEntradas.IpHost = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9")); } break;
                case "entrada_bd": if (string.IsNullOrWhiteSpace(NombreBaseDatos) || NombreBaseDatos.Trim() == "Base de datos*") { NombreBaseDatos = "Base de datos*"; ColoresEntradas.NombreBaseDatos = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9")); } break;
                case "entrada_usuario": if (string.IsNullOrWhiteSpace(Usuario) || Usuario.Trim() == "Usuario*") { Usuario = "Usuario*"; ColoresEntradas.UsuarioBD = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9")); } break;
                case "entrada_contrasena": if (string.IsNullOrWhiteSpace(Contrasena) || Contrasena.Trim() == "Contraseña*") { Contrasena = "Contraseña*"; ColoresEntradas.ContrasenaBD = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9")); } break;
            }
        }
        private bool RevisarEntradas(TextBox entrada)
        {
            string[] valores = ["Server*", "Base de datos*", "Usuario*", "Contraseña*"];
            foreach (string valor in valores)
            {
                if (valor == entrada.Text)
                    return true;
            }
            return false;

        }
        #endregion
    }
}
