using Entidades;
using Kiosco2026.Clases.Colores;
using System;
using System.Collections.Generic;
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
    public class ModeloConfDN : ModeloDeVistaBase
    {
        private Configuracion _configuracion;
        private string _ruta;
        string carpetaDatos = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Muoqa",
            "MuoqaStock"
        );
        public ModeloConfDN()
        {
            _ruta = carpetaDatos + "\\config.json";
            string jsonDeConfiguracion = File.ReadAllText(_ruta);
            _configuracion = JsonSerializer.Deserialize<Configuracion>(jsonDeConfiguracion);
            GotFocus = new ComandoModeloDeVista(EjecutarGotFocus);
            LostFocus = new ComandoModeloDeVista(EjecutarLostFocus);
            GuardarDatos = new ComandoModeloDeVista(EjecutarGuardarDatos);
            ColoresEntradas = new ColoresProductos();
            EjecutarCargarDatos(null);
        }
        #region Propiedades
        private string _nombreNegocio;
        public string NombreNegocio
        {
            get => _nombreNegocio;
            set
            {
                _nombreNegocio = value;
                OnPropertyChanged(nameof(NombreNegocio));
            }
        }
        private string _direccionNegocio;
        public string DireccionNegocio
        {
            get => _direccionNegocio;
            set
            {
                _direccionNegocio = value;
                OnPropertyChanged(nameof(DireccionNegocio));
            }
        }
        private string _numeroDeCaja;
        public string NumeroDeCaja
        {
            get => _numeroDeCaja;
            set
            {
                _numeroDeCaja = value;
                OnPropertyChanged(nameof(NumeroDeCaja));
            }
        }
        private string _nombreDeCaja;
        public string NombreDeCaja
        {
            get => _nombreDeCaja;
            set
            {
                _nombreDeCaja = value;
                OnPropertyChanged(nameof(NombreDeCaja));
            }
        }
        private string _numeroDeSucursal;
        public string NumeroDeSucursal
        {
            get => _numeroDeSucursal;
            set
            {
                _numeroDeSucursal = value;
                OnPropertyChanged(nameof(NumeroDeSucursal));
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
            NombreNegocio = _configuracion.NombreNegocio;
            DireccionNegocio = _configuracion.DireccionDelLocal;
            NumeroDeCaja = _configuracion.NumeroDeCaja.ToString();
            NombreDeCaja = _configuracion.NombreDeCaja;
            NumeroDeSucursal = _configuracion.NumeroDeSucursal.ToString();
            ColoresEntradas.NombreNegocio = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020"));
            ColoresEntradas.DireccionNegocio = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020"));
            ColoresEntradas.NumeroDeCaja = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020"));
            ColoresEntradas.NombreDeCaja = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020"));
            ColoresEntradas.NumeroDeSucursal = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020"));
        }
        private async Task EjecutarGuardarDatos(object arg)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NombreNegocio) || NombreNegocio == "Nombre del negocio")
                    throw new Exception("Hay campos vacios, llenalos para poder guardar");
                if (string.IsNullOrWhiteSpace(DireccionNegocio) || DireccionNegocio == "Dirección del negocio")
                    throw new Exception("Hay campos vacios, llenalos para poder guardar");
                if (string.IsNullOrWhiteSpace(NumeroDeCaja) || NumeroDeCaja == "Numero de caja")
                    throw new Exception("Hay campos vacios, llenalos para poder guardar");
                if (string.IsNullOrWhiteSpace(NombreDeCaja) || NombreDeCaja == "Cajero")
                    throw new Exception("Hay campos vacios, llenalos para poder guardar");
                if (string.IsNullOrWhiteSpace(NumeroDeSucursal) || NumeroDeSucursal == "Numero de sucursal")
                    throw new Exception("Hay campos vacios, llenalos para poder guardar");
                _configuracion.NombreNegocio = NombreNegocio;
                _configuracion.DireccionDelLocal = DireccionNegocio;
                _configuracion.NumeroDeCaja = int.Parse(NumeroDeCaja);
                _configuracion.NombreDeCaja = NombreDeCaja;
                _configuracion.NumeroDeSucursal = int.Parse(NumeroDeSucursal);
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
                    case "entrada_nombreNegocio": NombreNegocio = string.Empty; ColoresEntradas.NombreNegocio = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020")); break;
                    case "entrada_direccion": DireccionNegocio = string.Empty; ColoresEntradas.DireccionNegocio = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020")); break;
                    case "entrada_numeroCaja": NumeroDeCaja = string.Empty; ColoresEntradas.NumeroDeCaja = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020")); break;
                    case "entrada_nombreCaja": NombreDeCaja = string.Empty; ColoresEntradas.NombreDeCaja = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020")); break;
                    case "entrada_numeroSucursal": NumeroDeSucursal = string.Empty; ColoresEntradas.NumeroDeSucursal = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020")); break;
                }
            }
        }
        private async Task EjecutarLostFocus(object arg)
        {
            string nombre = (string)arg;
            switch (nombre)
            {
                case "entrada_nombreNegocio": if (string.IsNullOrWhiteSpace(NombreNegocio) || NombreNegocio.Trim() == "Nombre del negocio") { NombreNegocio = "Nombre del negocio"; ColoresEntradas.NombreNegocio = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A")); } break;
                case "entrada_direccion": if (string.IsNullOrWhiteSpace(DireccionNegocio) || DireccionNegocio.Trim() == "Dirección del negocio") { DireccionNegocio = "Dirección del negocio"; ColoresEntradas.DireccionNegocio = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A")); } break;
                case "entrada_numeroCaja": if (string.IsNullOrWhiteSpace(NumeroDeCaja) || NumeroDeCaja.Trim() == "Numero de caja") { NumeroDeCaja = "Numero de caja"; ColoresEntradas.NumeroDeCaja = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A")); } break;
                case "entrada_nombreCaja": if (string.IsNullOrWhiteSpace(NombreDeCaja) || NombreDeCaja.Trim() == "Cajero") { NombreDeCaja = "Cajero"; ColoresEntradas.NombreDeCaja = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A")); } break;
                case "entrada_numeroSucursal": if (string.IsNullOrWhiteSpace(NumeroDeSucursal) || NumeroDeSucursal.Trim() == "Numero de sucursal") { NumeroDeSucursal = "Numero de sucursal"; ColoresEntradas.NumeroDeSucursal = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A")); } break;
            }
        }
        private bool RevisarEntradas(TextBox entrada)
        {
            string[] valores = ["Nombre del negocio", "Dirección del negocio", "Cajero", "Numero de caja", "Numero de sucursal"];
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
