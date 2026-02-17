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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Kiosco2026.ModeloDeLasVistas.Hijas
{
    public class ModeloEditarProducto : ModeloDeVistaBase
    {
        public ModeloEditarProducto() 
        {
            ActualizarAnchoActual = new ComandoModeloDeVista(EjecutarActualizarAnchoActual);
            ObtenerProductos = new ComandoModeloDeVista(EjecutarObtenerProductos);
            CargarDatos = new ComandoModeloDeVista(EjecutarCargarDatos);
            ObtenerCategorias = new ComandoModeloDeVista(EjecutarObtenerCategoriasGenerales);
            GuardarCmb = new ComandoModeloDeVista(EjecutarGuardarCmb);
            SelectionChanged = new ComandoModeloDeVista(EjecutarSelected);
            ValoresPredeterminados = new ComandoModeloDeVista(EjecutarValoresPredeterminados);
            GotFocus = new ComandoModeloDeVista(EjecutarGotFocus);
            LostFocus = new ComandoModeloDeVista(EjecutarLostFocus);
            _adminProductos = new AdminProductos();
            ColoresEntradas = new ColoresProductos();
            Producto = new Productos("-");
            cmbCat = new ComboBox();
            Marcado = default;
            Nombre = "Nombre";
            Venta = "Venta";
            Compra = "Compra";
            Cantidad = "Cantidad";
        }
        #region Propiedades
        #region Columnas privadas
        private double _eColumna_uno;
        private double _eColumna_dos;
        private double _eColumna_tres;
        private double _eColumna_cuatro;
        private double _eColumna_cinco;
        private double _eColumna_seis;
        private double _eColumna_siete;
        private double _eColumna_ocho;
        private double _tColumna_uno;
        private double _tColumna_dos;
        private double _tColumna_tres;
        private double _tColumna_cuatro;
        private double _tColumna_cinco;
        private double _tColumna_seis;
        private double _tColumna_siete;
        private double _tColumna_ocho;
        #endregion
        #region Columnas publicas
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
        public double EColumna_cuatro
        {
            get => _eColumna_cuatro;
            set
            {
                _eColumna_cuatro = value;
                OnPropertyChanged(nameof(EColumna_cuatro));
            }
        }
        public double EColumna_cinco
        {
            get => _eColumna_cinco;
            set
            {
                _eColumna_cinco = value;
                OnPropertyChanged(nameof(EColumna_cinco));
            }
        }
        public double EColumna_seis
        {
            get => _eColumna_seis;
            set
            {
                _eColumna_seis = value;
                OnPropertyChanged(nameof(EColumna_seis));
            }
        }
        public double EColumna_siete
        {
            get => _eColumna_siete;
            set
            {
                _eColumna_siete = value;
                OnPropertyChanged(nameof(EColumna_siete));
            }
        }
        public double EColumna_ocho
        {
            get => _eColumna_ocho;
            set
            {
                _eColumna_ocho = value;
                OnPropertyChanged(nameof(EColumna_ocho));
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
        public double TColumna_cuatro
        {
            get => _tColumna_cuatro;
            set
            {
                _tColumna_cuatro = value;
                OnPropertyChanged(nameof(TColumna_cuatro));
            }
        }
        public double TColumna_cinco
        {
            get => _tColumna_cinco;
            set
            {
                _tColumna_cinco = value;
                OnPropertyChanged(nameof(TColumna_cinco));
            }
        }
        public double TColumna_seis
        {
            get => _tColumna_seis;
            set
            {
                _tColumna_seis = value;
                OnPropertyChanged(nameof(TColumna_seis));
            }
        }
        public double TColumna_siete
        {
            get => _tColumna_siete;
            set
            {
                _tColumna_siete = value;
                OnPropertyChanged(nameof(TColumna_siete));
            }
        }
        public double TColumna_ocho
        {
            get => _tColumna_ocho;
            set
            {
                _tColumna_ocho = value;
                OnPropertyChanged(nameof(TColumna_ocho));
            }
        }
        #endregion
        private AdminProductos _adminProductos;
        private ObservableCollection<Categorias> _listaCategorias;
        private Categorias _categoria;
        private Productos _producto;
        private ComboBox cmbCat;
        private string _venta;
        private string _compra;
        private string _cantidad;
        private string _nombre;
        private bool _marcado;
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
        public Productos Producto
        {
            get => _producto;
            set
            {
                _producto = value;
                OnPropertyChanged(nameof(Producto));
            }
        }
        public bool Marcado
        {
            get => _marcado;
            set
            {
                _marcado = value;
                OnPropertyChanged(nameof(Marcado));
            }
        }
        public string Nombre
        {
            get => _nombre;
            set
            {
                _nombre = value;
                OnPropertyChanged(nameof(Nombre));
            }
        }
        public string Venta
        {
            get => _venta;
            set
            {
                _venta = value;
                OnPropertyChanged(nameof(Venta));
            }
        }
        public string Compra
        {
            get => _compra;
            set
            {
                _compra = value;
                OnPropertyChanged(nameof(Compra));
            }
        }
        public string Cantidad
        {
            get => _cantidad;
            set
            {
                _cantidad = value;
                OnPropertyChanged(nameof(Cantidad));
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
        //Queda hacer las propiedades publicas con getters y setters 13/2/2026??
        private ObservableCollection<Productos> _listaProductos;
        public ObservableCollection<Productos> ListaProductos
        {
            get => _listaProductos;
            set
            {
                _listaProductos = value;
                OnPropertyChanged(nameof(ListaProductos));
            }
        }
        public Categorias Categoria
        {
            get => _categoria;
            set
            {
                _categoria = value;
                OnPropertyChanged(nameof(Categoria));
            }
        }
        #endregion
        #region Comandos
        public ICommand ActualizarAnchoActual { get; }
        public ICommand ObtenerProductos { get; }
        public ICommand ObtenerCategorias { get; }
        public ICommand ValoresPredeterminados { get; }
        public ICommand CargarDatos { get; }
        public ICommand GotFocus { get; }
        public ICommand LostFocus { get; }
        public ICommand GuardarCmb { get; }
        public ICommand SelectionChanged { get; }
        #endregion
        #region Eventos
        private async Task EjecutarGuardarCmb(object arg)
        {
            ComboBox cmb = (ComboBox)arg;
            cmbCat = cmb;
        }
        private async Task EjecutarSelected(object arg)
        {
            Categorias cate = (Categorias)arg;
            string str = cate.Nombre.ToLower();
            if (str != "categoria*")
                ColoresEntradas.Categoria = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            else
                ColoresEntradas.Categoria = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
        }
        private async Task EjecutarCargarDatos(object obj)
        {
            Productos producto = obj as Productos;
            Producto = producto;
            Nombre = Producto.Nombre;
            Venta = Producto.Venta;
            Compra = Producto.Compra;
            Cantidad = Producto.Cantidad.ToString();
            for (int i = 0; i < cmbCat.Items.Count; i++) 
            {
                Categorias cate = (Categorias)cmbCat.Items[i];
                if(cate.ID == producto.IDCategoria)
                    cmbCat.SelectedIndex = i;
            }
            bool mostrar = false;
            if(Producto.Mostrar == "si")
                mostrar = true;
            Marcado = mostrar;
        }
        private async Task EjecutarObtenerProductos(object obj)
        {
            string valor = "";
            if (valor != "Buscar por nombre")
                valor = valor;
            var res = await _adminProductos.BusquedaProductos(valor);
            ListaProductos = new ObservableCollection<Productos>(res.lista);
        }
        private async Task EjecutarActualizarAnchoActual(object arg)
        {
            double anchoActual;
            if (arg == null)
                anchoActual = 1100;
            else
                anchoActual = (double)arg;
            double nueve = (anchoActual * 9) / 100;
            double relleno = (anchoActual * 13) / 100;
            EColumna_uno = nueve;
            EColumna_dos = relleno;
            EColumna_tres = relleno;
            EColumna_cuatro = relleno;
            EColumna_cinco = relleno;
            EColumna_seis = relleno;
            EColumna_siete = relleno;
            EColumna_ocho= relleno;
            TColumna_uno = nueve;
            TColumna_dos = relleno;
            TColumna_tres = relleno;
            TColumna_cuatro = relleno;
            TColumna_cinco = relleno;
            TColumna_seis = relleno;
            TColumna_siete = relleno;
            TColumna_ocho = relleno;
        }
        private async Task EjecutarObtenerCategoriasGenerales(object arg)
        {
            var respuesta = await _adminProductos.ObtenerCategoriasGenerales();
            var respuestaDos = await _adminProductos.ObtenerCategoriasCliente();
            List<Categorias> listaGeneral = respuesta.lista;
            List<Categorias> listaCliente = respuestaDos.lista;
            List<Categorias> listaCompleta = new List<Categorias>();
            for (int i = 0; i < listaGeneral.Count; i++)
            {
                listaCompleta.Add(listaGeneral[i]);
            }
            for (int i = 0; i < listaCliente.Count; i++)
            {
                listaCompleta.Add(listaCliente[i]);
            }
            ListaCategorias = new ObservableCollection<Categorias>(listaCompleta);
        }
        private async Task EjecutarValoresPredeterminados(object arg)
        {
            Nombre = "Nombre*";
            Venta = "Precio de venta*";
            Compra = "Costo de compra*";
            Cantidad = "Cantidad*";
            ColoresEntradas.Nombre = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
            ColoresEntradas.Venta = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
            ColoresEntradas.Compra = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
            ColoresEntradas.Cantidad = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
            ColoresEntradas.Categoria = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
        }
        #endregion
        #region Grafica

        private async Task EjecutarGotFocus(object arg)
        {
            TextBox entrada = (TextBox)arg;
            if (RevisarEntradas(entrada))
            {
                switch (entrada.Name)
                {
                    case "entrada_nombre": Nombre = string.Empty; ColoresEntradas.Nombre = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")); break;
                    case "entrada_venta": Venta = string.Empty; ColoresEntradas.Venta = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")); break;
                    case "entrada_compra": Compra = string.Empty; ColoresEntradas.Compra = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")); break;
                    case "entrada_cantidad": Cantidad = string.Empty; ColoresEntradas.Cantidad = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")); break;
                }
            }
        }
        private async Task EjecutarLostFocus(object arg)
        {
            string nombre = (string)arg;
            switch (nombre)
            {
                case "entrada_nombre": if (string.IsNullOrWhiteSpace(Nombre) || Nombre.Trim() == "Nombre*") { Nombre = "Nombre*"; ColoresEntradas.Nombre = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9")); } break;
                case "entrada_venta": if (string.IsNullOrWhiteSpace(Venta) || Venta.Trim() == "Precio de venta*") { Venta = "Precio de venta*"; ColoresEntradas.Venta = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9")); } break;
                case "entrada_compra": if (string.IsNullOrWhiteSpace(Compra) || Compra.Trim() == "Costo de compra*") { Compra = "Costo de compra*"; ColoresEntradas.Compra = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9")); } break;
                case "entrada_cantidad": if (string.IsNullOrWhiteSpace(Cantidad) || Cantidad.Trim() == "Cantidad*") { Cantidad = "Cantidad*"; ColoresEntradas.Cantidad = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9")); } break;
            }
        }
        private bool RevisarEntradas(TextBox entrada)
        {
            string[] valores = ["Nombre*", "Código*", "Precio de venta*", "Costo de compra*", "Cantidad*", "Proveedor"];
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
