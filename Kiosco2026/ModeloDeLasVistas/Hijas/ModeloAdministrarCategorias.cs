using Dominio;
using Entidades;
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
using System.Windows.Controls;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Kiosco2026.Clases.Colores;

namespace Kiosco2026.ModeloDeLasVistas.Hijas
{
    public class ModeloAdministrarCategorias : ModeloDeVistaBase
    {
        public ModeloAdministrarCategorias() 
        {
            ActualizarAnchoActual = new ComandoModeloDeVista(EjecutarActualizarAnchoActual);
            LlenarDatos = new ComandoModeloDeVista(EjecutarLlenarDatos);
            BuscarCategorias = new ComandoModeloDeVista(EjecutarBuscarCategorias);
            EditarCategoria = new ComandoModeloDeVista(EjecutarEditarCategoria);
            GotFocus = new ComandoModeloDeVista(EjecutarGotFocus);
            LostFocus = new ComandoModeloDeVista(EjecutarLostFocus);
            _adminProductos = new AdminProductos();
            _coloresEntradas = new ColoresProductos();
            SoloLectura = true;
            Busqueda = "Buscar por nombre";
            Edicion = "Editar nombre";
            CategoriaSeleccionada = "Ninguna seleccionada";
            _colores = new ColoresTexto();
            ColorMensajeEdicion = _colores.Normal;
            tiempo = new System.Timers.Timer();
            tiempo.Elapsed += ResetearMensaje;
            tiempo.Enabled = false;
            tiempo.AutoReset = true;
        }
        #region Propiedades
        private System.Timers.Timer tiempo;
        private string _edicion;
        private string _categoriaSeleccionada;
        private string _colorMensajeEditar;
        private string _mensajeEditar;
        private ColoresTexto _colores;
        private ColoresProductos _coloresEntradas;
        private AdminProductos _adminProductos;
        private ObservableCollection<Categorias> _listaCategorias;
        private string _busqueda;
        private double _eColumna_uno;
        private double _eColumna_dos;
        private double _eColumna_tres;
        private double _tColumna_uno;
        private double _tColumna_dos;
        private double _tColumna_tres;
        private bool _visualizacion;
        private bool _soloLectura;
        private string _colorBusqueda;
        public ColoresProductos ColoresProductos
        {
            get => _coloresEntradas;
            set
            {
                _coloresEntradas = value;
                OnPropertyChanged(nameof(ColoresProductos));
            }
        }
        public string ColorBusqueda
        {
            get => _colorBusqueda;
            set
            {
                _colorBusqueda = value;
                OnPropertyChanged(nameof(ColorBusqueda));
            }
        }
        public string Busqueda
        {
            get => _busqueda;
            set
            {
                _busqueda = value;
                OnPropertyChanged(nameof(Busqueda));
            }
        }
        public string MensajeEdicion
        {
            get => _mensajeEditar;
            set
            {
                _mensajeEditar = value;
                OnPropertyChanged(nameof(MensajeEdicion));
            }
        }
        public string ColorMensajeEdicion
        {
            get => _colorMensajeEditar;
            set
            {
                _colorMensajeEditar = value;
                OnPropertyChanged(nameof(ColorMensajeEdicion));
            }
        }
        public bool SoloLectura
        {
            get => _soloLectura;
            set
            {
                _soloLectura = value;
                OnPropertyChanged(nameof(SoloLectura));
            }
        }
        public ObservableCollection<Categorias> ListaCategorias
        {
            get => _listaCategorias;
            set
            {
                _listaCategorias = value;
                OnPropertyChanged(nameof(ListaCategorias));
            }
        }
        public bool Visualizacion
        {
            get => _visualizacion;
            set
            {
                _visualizacion = value;
                OnPropertyChanged(nameof(Visualizacion));
            }
        }
        public string CategoriaSeleccionada
        {
            get => _categoriaSeleccionada;
            set
            {
                _categoriaSeleccionada = value;
                OnPropertyChanged(nameof(CategoriaSeleccionada));
            }
        }
        public string Edicion
        {
            get => _edicion;
            set
            {
                _edicion = value;
                OnPropertyChanged(nameof(Edicion));
            }
        }
        public double EColumna_uno
        {
            get => _eColumna_uno;
            set
            {
                _eColumna_uno = value;
                OnPropertyChanged(nameof(EColumna_uno));
            }
        }
        public double EColumna_dos
        {
            get => _eColumna_dos;
            set
            {
                _eColumna_dos = value;
                OnPropertyChanged(nameof(EColumna_dos));
            }
        }
        public double EColumna_tres
        {
            get => _eColumna_tres;
            set
            {
                _eColumna_tres = value;
                OnPropertyChanged(nameof(EColumna_tres));
            }
        }
        public double TColumna_uno
        {
            get => _tColumna_uno;
            set
            {
                _tColumna_uno = value;
                OnPropertyChanged(nameof(TColumna_uno));
            }
        }
        public double TColumna_dos
        {
            get => _tColumna_dos;
            set
            {
                _tColumna_dos = value;
                OnPropertyChanged(nameof(TColumna_dos));
            }
        }
        public double TColumna_tres
        {
            get => _tColumna_tres;
            set
            {
                _tColumna_tres = value;
                OnPropertyChanged(nameof(TColumna_tres));
            }
        }
        #endregion
        #region Comandos
        public ICommand ActualizarAnchoActual { get; }
        public ICommand LlenarDatos { get; }
        public ICommand BuscarCategorias { get; }
        public ICommand EditarCategoria { get; }
        public ICommand GotFocus { get; }
        public ICommand LostFocus { get; }
        #endregion
        private async Task EjecutarBuscarCategorias(object arg)
        {
            string valor = "";
            if (Busqueda != "Buscar por nombre")
                valor = Busqueda;
            var res = await _adminProductos.BusquedaCategorias(valor);
            ListaCategorias = new ObservableCollection<Categorias>(res.lista);
        }
        private async Task EjecutarEditarCategoria(object arg)
        {
            try
            {
                string visualizacion;
                if (Visualizacion)
                    visualizacion = "si";
                else
                    visualizacion = "no";
                int idCategoria;
                if (!int.TryParse(CategoriaSeleccionada, out idCategoria))
                    throw new Exception("Algo salio mal, contactanos");
                Categorias cate = new Categorias
                {
                    Nombre = Edicion,
                    ID = idCategoria,
                    Mostrar = visualizacion
                };
                var respuesta = await _adminProductos.EditarCategoria(cate);
                if (respuesta.estado)
                {
                    Busqueda = "Buscar por nombre";
                    Edicion = "Editar nombre";
                    CategoriaSeleccionada = "Ninguna seleccionada";
                    ColoresProductos.BusuqedaUno = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
                    ColorMensajeEdicion = _colores.ColorExito;
                    EjecutarBuscarCategorias(null);
                }
                else
                {
                    ColorMensajeEdicion = _colores.ColorError;
                }
                MensajeEdicion = respuesta.mensaje;
            }
            catch (Exception ex) 
            {
                ColorMensajeEdicion = _colores.ColorError;
                MensajeEdicion = ex.Message;
            }
            finally
            {
                tiempo.Interval = 10000;
                tiempo.Enabled = true;
            }
        }
        private async Task EjecutarLlenarDatos(object arg)
        {
            Categorias categoria = (Categorias)arg;
            Edicion = categoria.Nombre;
            SoloLectura = false;
            if (categoria.Mostrar == "si")
                Visualizacion = true;
            else
                Visualizacion = false;
            ColoresProductos.BusuqedaUno = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            CategoriaSeleccionada = categoria.ID.ToString();
        }
        private async Task EjecutarActualizarAnchoActual(object arg)
        {
            double anchoActual;
            if (arg == null)
                anchoActual = 1100;
            else
                anchoActual = (double)arg;
            double quince = (anchoActual * 15) / 100;
            double treinta = (anchoActual * 25) / 100;
            double setenta = (anchoActual * 60) / 100;
            EColumna_uno = quince;
            EColumna_dos = treinta;
            EColumna_tres = setenta;
            TColumna_uno = quince;
            TColumna_dos = treinta;
            TColumna_tres = setenta;
        }
        private void ResetearMensaje(object source, ElapsedEventArgs e)
        {
            tiempo.Enabled = false;
            MensajeEdicion = "";
        }

        #region Grafica

        private async Task EjecutarGotFocus(object arg)
        {
            TextBox entrada = (TextBox)arg;
            if (RevisarEntradas(entrada))
            {
                switch (entrada.Name)
                {
                    case "entrada_busqueda": Busqueda = string.Empty; ColoresProductos.Busuqeda = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")); break;
                    case "entrada_edicion": Edicion = string.Empty; ColoresProductos.BusuqedaUno= new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")); break;
                }
            }
        }
        private async Task EjecutarLostFocus(object arg)
        {
            string nombre = (string)arg;
            switch (nombre)
            {
                case "entrada_busqueda": if (string.IsNullOrWhiteSpace(Busqueda) || Busqueda.Trim() == "Buscar por nombre") { Busqueda = "Buscar por nombre"; ColoresProductos.Busuqeda = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9")); } break;
                case "entrada_edicion": if (string.IsNullOrWhiteSpace(Edicion) || Edicion.Trim() == "Editar nombre") { Edicion = "Editar nombre"; ColoresProductos.BusuqedaUno = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9")); } break;
            }
        }
        private bool RevisarEntradas(TextBox entrada)
        {
            string[] valores = ["Editar nombre", "Buscar por nombre"];
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
