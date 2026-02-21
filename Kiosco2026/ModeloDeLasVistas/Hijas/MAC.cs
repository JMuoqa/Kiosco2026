using Dominio;
using Entidades;
using Kiosco2026.Clases.Colores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            _adminProductos = new AdminProductos();
            _colores = new ColoresTexto();
            NombreCargar = "Buscar";
            ColorBusqueda = "#B9B9B9";
            
        }
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
        private async Task EjecutarCargarCategoria(object arg)
        {
            try
            {
                Categorias cate = new Categorias
                {
                    Nombre = NombreCargar
                };
                var respuesta = await _adminProductos.CargarCategoria(cate);
                if (respuesta.estado)
                    ColorMensaje = _colores.ColorExito;
                else
                    ColorMensaje = _colores.ColorError;
                Mensaje = respuesta.mensaje;
            }
            catch (Exception ex)
            {
                ColorMensaje = _colores.ColorError;
                Mensaje = ex.Message;
            }
        }

        private async Task EjecutarGotFocus(object arg)
        {
            if (NombreCargar == "Buscar"){
                NombreCargar = string.Empty;
                ColorBusqueda = "#FFFFFF";
            }
        }
        private async Task EjecutarLostFocus(object arg)
        {
            string nombre = (string)arg;
            if (string.IsNullOrWhiteSpace(NombreCargar) || NombreCargar.Trim() == "Buscar") 
            { 
                NombreCargar = "Buscar";
                ColorBusqueda = "#FFFFFF";
            } 
        }
    }
}
