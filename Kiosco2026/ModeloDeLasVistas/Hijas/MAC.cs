using Dominio;
using Entidades;
using Kiosco2026.Clases.Colores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Kiosco2026.ModeloDeLasVistas.Hijas
{
    public class MAC : ModeloDeVistaBase
    {
        private Configuracion _configuracion { get; }
        string carpetaDatos = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Muoqa",
            "MuoqaStock"
        );
        public MAC()
        {
            _metodoEnEjecucion = false;
            string ruta = carpetaDatos + "\\config.json";
            string jsonDeConfiguracion = File.ReadAllText(ruta);
            _configuracion = JsonSerializer.Deserialize<Configuracion>(jsonDeConfiguracion);
            CargarCategoria = new ComandoModeloDeVista(EjecutarCargarCategoria);
            GotFocus = new ComandoModeloDeVista(EjecutarGotFocus);
            LostFocus = new ComandoModeloDeVista(EjecutarLostFocus);
            KeyDownCargar = new ComandoModeloDeVista(EjecutarKeyDownCargar);
            PreviewKeyDownParaCerrar = new ComandoModeloDeVista(EjecutarPreviewKeyDownParaCerrar);
            _adminProductos = new AdminProductos();
            _colores = new ColoresTexto();
            NombreCargar = "Nombre de la categoría";
            ColorBusqueda = "#9A9A9A";
            tiempo = new System.Timers.Timer();
            tiempo.Elapsed += ResetearMensaje;
            tiempo.Enabled = false;
            tiempo.AutoReset = true;
        }
        private bool _metodoEnEjecucion;
        public event Action CerrarVentana;
        private System.Timers.Timer tiempo;
        private AdminProductos _adminProductos;
        private ColoresTexto _colores;
        private string _colorBusqueda;
        private string _nombreCargar;
        private string _colorMensaje;
        private string _mensaje;
        public string ColorBusqueda
        {
            get => _colorBusqueda;
            set
            {
                _colorBusqueda = value;
                OnPropertyChanged(nameof(ColorBusqueda));
            }
        }
        public string Mensaje
        {
            get => _mensaje;
            set
            {
                _mensaje = value;
                OnPropertyChanged(nameof(Mensaje));
            }
        }
        public string ColorMensaje
        {
            get => _colorMensaje;
            set
            {
                _colorMensaje = value;
                OnPropertyChanged(nameof(ColorMensaje));
            }
        }
        public string NombreCargar
        {
            get => _nombreCargar;
            set
            {
                _nombreCargar = value;
                OnPropertyChanged(nameof(NombreCargar));
            }
        }
        public ICommand CargarCategoria { get; }
        public ICommand GotFocus { get; }
        public ICommand LostFocus { get; }
        public ICommand KeyDownCargar { get; }
        public ICommand PreviewKeyDownParaCerrar { get; }
        private DateTime? primeraPulsacion;
        private async Task EjecutarPreviewKeyDownParaCerrar(object arg)
        {
            if (arg is not System.Windows.Input.KeyEventArgs e)
                return;
            if (e.Key != Key.Escape) return;
            DateTime ahora = DateTime.Now;
            if (primeraPulsacion == null)
            {
                primeraPulsacion = ahora;
                return;
            }
            TimeSpan diferencia = ahora - primeraPulsacion.Value;
            if (diferencia.TotalSeconds <= 1)
            {
                primeraPulsacion = null;
                CerrarVentana?.Invoke();
            }
            else
            {
                primeraPulsacion = ahora;
            }
        }
        private async Task EjecutarKeyDownCargar(object arg)
        {
            ColorBusqueda = "#202020";
            NombreCargar = NombreCargar.Replace("Nombre de la categoría", "");
            if (arg is KeyEventArgs e)
            {
                if (e.Key == Key.Enter)
                {
                    EjecutarCargarCategoria(null);
                }
            }
        }
        private void EjecutarPostCarga()
        {
            NombreCargar = "Nombre de la categoría";
            ColorBusqueda = "#9A9A9A";
        }
        private async Task EjecutarCargarCategoria(object arg)
        {
            try
            {
                if (!_metodoEnEjecucion)
                {
                    _metodoEnEjecucion = true;
                    Categorias cate = new Categorias
                    {
                        Nombre = NombreCargar.ToUpper()
                    };
                    var respuesta = await _adminProductos.CargarCategoria(cate);
                    if (respuesta.estado)
                    {
                        ColorMensaje = _colores.ColorExito;
                        EjecutarPostCarga();
                    }
                    else
                    {
                        ColorMensaje = _colores.ColorError;
                    }
                    Mensaje = respuesta.mensaje;
                    _metodoEnEjecucion = false;
                }
            }
            catch (Exception ex)
            {
                ColorMensaje = _colores.ColorError;
                Mensaje = ex.Message;
                HistorialErrores error = new HistorialErrores(ex.Message, ex.GetType().FullName, ex.StackTrace, "Kiosco2026.ModeloDeLasVistas.Hijas", "MAC", "EjecutarCargarCategoria", _configuracion.NombreDeCaja);
                EscribirEnTXTError(error);
                _metodoEnEjecucion = false;
            }
            finally
            {
                tiempo.Interval = 10000;
                tiempo.Enabled = true;
            }
        }
        private async Task EjecutarGotFocus(object arg)
        {
            if (NombreCargar == "Nombre de la categoría")
            {
                NombreCargar = string.Empty;
                ColorBusqueda = "#202020";
            }
        }
        private async Task EjecutarLostFocus(object arg)
        {
            string nombre = (string)arg;
            if (string.IsNullOrWhiteSpace(NombreCargar) || NombreCargar.Trim() == "Nombre de la categoría") 
            { 
                NombreCargar = "Nombre de la categoría";
                ColorBusqueda = "#9A9A9A";
            }
        }
        private void ResetearMensaje(object source, ElapsedEventArgs e)
        {
            tiempo.Enabled = false;
            Mensaje = "";
        }
    }
}
