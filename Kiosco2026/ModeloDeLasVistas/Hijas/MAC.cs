using Dominio;
using Entidades;
using Kiosco2026.Clases.Colores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

        public MAC()
        {
            CargarCategoria = new ComandoModeloDeVista(EjecutarCargarCategoria);
            GotFocus = new ComandoModeloDeVista(EjecutarGotFocus);
            LostFocus = new ComandoModeloDeVista(EjecutarLostFocus);
            KeyDownCargar = new ComandoModeloDeVista(EjecutarKeyDownCargar);
            _adminProductos = new AdminProductos();
            _colores = new ColoresTexto();
            NombreCargar = "Nombre de la categoria";
            ColorBusqueda = "#B9B9B9";
            tiempo = new System.Timers.Timer();
            tiempo.Elapsed += ResetearMensaje;
            tiempo.Enabled = false;
            tiempo.AutoReset = true;
        }
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
        private async Task EjecutarKeyDownCargar(object arg)
        {

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
            NombreCargar = "Nombre de la categoria";
            ColorBusqueda = "#B9B9B9";
        }
        private async Task EjecutarCargarCategoria(object arg)
        {
            try
            {
                Categorias cate = new Categorias
                {
                    Nombre = NombreCargar.ToUpper()
                };
                var respuesta = await _adminProductos.CargarCategoria(cate);
                if (respuesta.estado){
                    ColorMensaje = _colores.ColorExito;
                    EjecutarPostCarga();
                }
                else{
                    ColorMensaje = _colores.ColorError;
                }
                Mensaje = respuesta.mensaje;
            }
            catch (Exception ex)
            {
                ColorMensaje = _colores.ColorError;
                Mensaje = ex.Message;
            }
            finally
            {
                tiempo.Interval = 10000;
                tiempo.Enabled = true;
            }
        }
        private async Task EjecutarGotFocus(object arg)
        {
            if (NombreCargar == "Nombre de la categoria")
            {
                NombreCargar = string.Empty;
                ColorBusqueda = "#FFFFFF";
            }
        }
        private async Task EjecutarLostFocus(object arg)
        {
            string nombre = (string)arg;
            if (string.IsNullOrWhiteSpace(NombreCargar) || NombreCargar.Trim() == "Nombre de la categoria") 
            { 
                NombreCargar = "Nombre de la categoria";
                ColorBusqueda = "#B9B9B9";
            }
        }
        private void ResetearMensaje(object source, ElapsedEventArgs e)
        {
            tiempo.Enabled = false;
            Mensaje = "";
        }
    }
}
