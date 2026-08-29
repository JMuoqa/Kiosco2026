using Entidades;
using Kiosco2026.Clases.Colores;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    public class ModeloConfImpresora : ModeloDeVistaBase
    {
        private ConfiguracionImpresora _configuracionImpresora;
        private Configuracion  _configuracion;
        private string _rutaImpresora;
        private string _rutaConf;
        public ModeloConfImpresora()
        {
            _rutaConf = "config.json";
            _rutaImpresora = "configImpresora.json";
            string jsonDeConfiguracion = File.ReadAllText(_rutaConf);
            _configuracion = JsonSerializer.Deserialize<Configuracion>(jsonDeConfiguracion);
            string jsonDeConfiguracionImpresora = File.ReadAllText(_rutaImpresora);
            _configuracionImpresora = JsonSerializer.Deserialize<ConfiguracionImpresora>(jsonDeConfiguracionImpresora);
            ColoresEntradas = new ColoresProductos();
            GotFocus = new ComandoModeloDeVista(EjecutarGotFocus);
            LostFocus = new ComandoModeloDeVista(EjecutarLostFocus);
            GuardarDatos = new ComandoModeloDeVista(EjecutarGuardarDatos);
            EjecutarCargarDatos(null);
        }
        #region Propiedades
        private string _nombreImpresora;
        public string NombreImpresora
        {
            get => _nombreImpresora;
            set
            {
                _nombreImpresora = value;
                OnPropertyChanged(nameof(NombreImpresora));
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
            NombreImpresora = _configuracionImpresora.NombreImpresora;
            ColoresEntradas.NombreImpresora = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
        }
        private async Task EjecutarGuardarDatos(object arg)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NombreImpresora) || NombreImpresora == "Nombre de la impresora")
                    throw new Exception("Hay campos vacios, llenalos para poder guardar");
                _configuracionImpresora.NombreImpresora = NombreImpresora;
                //Convertimos de vuelta a formato json
                string nuevoJson = JsonSerializer.Serialize(_configuracionImpresora, new JsonSerializerOptions { WriteIndented = true });
                //Sobrescribimos el archivo con las nuevas modificaciones
                File.WriteAllText(_rutaImpresora, nuevoJson);
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
                        case "entrada_nombreImpresora": NombreImpresora = string.Empty; ColoresEntradas.NombreImpresora = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")); break;
                    }
                }
            }
            private async Task EjecutarLostFocus(object arg)
            {
                string nombre = (string)arg;
                switch (nombre)
                {
                    case "entrada_nombreImpresora": if (string.IsNullOrWhiteSpace(NombreImpresora) || NombreImpresora.Trim() == "Nombre de la impresora") { NombreImpresora = "Nombre de la impresora"; ColoresEntradas.NombreImpresora = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9")); } break;
                }
            }
            private bool RevisarEntradas(TextBox entrada)
            {
                string[] valores = ["Nombre de la impresora"];
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
