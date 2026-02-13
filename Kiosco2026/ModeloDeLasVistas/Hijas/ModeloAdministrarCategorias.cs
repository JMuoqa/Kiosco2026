using Dominio;
using Entidades;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kiosco2026.ModeloDeLasVistas.Hijas
{
    public class ModeloAdministrarCategorias : ModeloDeVistaBase
    {
        public ModeloAdministrarCategorias() 
        {
            ActualizarAnchoActual = new ComandoModeloDeVista(EjecutarActualizarAnchoActual);
            LlenarDatos = new ComandoModeloDeVista(EjecutarLlenarDatos);
            BuscarCategorias = new ComandoModeloDeVista(EjecutarBuscarCategorias);
            CargarCategoria = new ComandoModeloDeVista(EjecutarCargarCategoria);
            EditarCategoria = new ComandoModeloDeVista(EjecutarEditarCategoria);
            _adminProductos = new AdminProductos();
            SoloLectura = true;
            Busqueda = "Buscar por nombre";
            Edicion = "Editar nombre";
            NombreCargar = "Nombre de la categoria";
            CategoriaSeleccionada = "Ninguna seleccionada";
            _colores = new ColoresTexto();
            ColorMensaje = _colores.Normal;
            ColorEdicion = "#B9B9B9";
        }
        #region Propiedades
        private string _busqueda;
        private string _nombreCargar;
        private string _edicion;
        private string _mensaje;
        private string _categoriaSeleccionada;
        private string _colorEdicion;
        private string _colorMensaje;
        private string _idEditar;
        private ColoresTexto _colores;
        private string _colorMensajeEditar;
        private string _mensajeEditar;
        private bool _visualizacion;
        private bool _soloLectura;
        private double _eColumna_uno;
        private double _eColumna_dos;
        private double _eColumna_tres; 
        private double _tColumna_uno;
        private double _tColumna_dos;
        private double _tColumna_tres;
        private AdminProductos _adminProductos;
        private ObservableCollection<Categorias> _listaCategorias;
        private string IdEditar
        {
            get => _idEditar;
            set
            {
                _idEditar = value;
                OnPropertyChanged(nameof(IdEditar));
            }
        }
        public string MensajeEditar
        {
            get => _mensajeEditar;
            set
            {
                _mensajeEditar = value;
                OnPropertyChanged(nameof(MensajeEditar));
            }
        }
        public string ColorMensajeEditar
        {
            get => _colorMensajeEditar;
            set
            {
                _colorMensajeEditar = value;
                OnPropertyChanged(nameof(ColorMensajeEditar));
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
        public string ColorEdicion
        {
            get => _colorEdicion;
            set
            {
                _colorEdicion = value;
                OnPropertyChanged(nameof(ColorEdicion));
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
        public string ColorMensaje
        {
            get => _colorMensaje;
            set
            {
                _colorMensaje = value;
                OnPropertyChanged(nameof(ColorMensaje));
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
        public string NombreCargar
        {
            get => _nombreCargar;
            set
            {
                _nombreCargar = value;
                OnPropertyChanged(nameof(NombreCargar));
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
        public string Busqueda
        {
            get => _busqueda;
            set
            {
                _busqueda = value;
                OnPropertyChanged(nameof(Busqueda));
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
        public ICommand CargarCategoria { get; }
        public ICommand EditarCategoria { get; }
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
                if (!int.TryParse(IdEditar, out idCategoria))
                    throw new Exception("Algo salio mal, contactanos");
                Categorias cate = new Categorias
                {
                    Nombre = Edicion,
                    ID = idCategoria,
                    Mostrar = visualizacion
                };
                var respuesta = await _adminProductos.EditarCategoria(cate);
                Debug.Write(respuesta);
            }
            catch (Exception ex) 
            {
                ColorMensajeEditar = _colores.ColorError;
                MensajeEditar = ex.Message;
            }
        }
        private async Task EjecutarLlenarDatos(object arg)
        {
            Categorias categoria = (Categorias)arg;
            Edicion = categoria.Nombre;
            if (categoria.Mostrar == "si")
                Visualizacion = true;
            else
                Visualizacion = false;
            ColorEdicion = "#FFFFFF";
            CategoriaSeleccionada = categoria.ID.ToString();
        }
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
    }
}
