using Dominio;
using Entidades;
using Kiosco2026.Clases.Colores;
using Kiosco2026.Vistas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Kiosco2026.ModeloDeLasVistas.Hijas
{
    public class ModeloEditarProducto : ModeloDeVistaBase
    {
        private Configuracion _configuracion { get; }
        private bool _metodoEnEjecucion;
        public ModeloEditarProducto()
        {
            _metodoEnEjecucion = false;
            string ruta = "config.json";
            string jsonDeConfiguracion = File.ReadAllText(ruta);
            _configuracion = JsonSerializer.Deserialize<Configuracion>(jsonDeConfiguracion);
            ActualizarAnchoActual = new ComandoModeloDeVista(EjecutarActualizarAnchoActual);
            ObtenerProductos = new ComandoModeloDeVista(EjecutarObtenerProductos);
            CargarDatos = new ComandoModeloDeVista(EjecutarCargarDatos);
            GuardarCmb = new ComandoModeloDeVista(EjecutarGuardarCmb);
            ObtenerCategorias = new ComandoModeloDeVista(EjecutarObtenerCategoriasGenerales);
            GuardarCmbParaBusqueda = new ComandoModeloDeVista(EjecutarGuardarCmbParaBusqueda);
            SelectionChanged = new ComandoModeloDeVista(EjecutarSelected);
            SelectionChangedParaBusqueda = new ComandoModeloDeVista(EjecutarSelectedParaBusqueda);
            ValoresPredeterminados = new ComandoModeloDeVista(EjecutarValoresPredeterminados);
            GotFocus = new ComandoModeloDeVista(EjecutarGotFocus);
            LostFocus = new ComandoModeloDeVista(EjecutarLostFocus);
            EditarProducto = new ComandoModeloDeVista(EjecutarEditarProducto);
            BusquedaPorCodigo = new ComandoModeloDeVista(EjecutarBusquedaPorCodigoUltLote);
            ResetearBusqueda = new ComandoModeloDeVista(ResetearTabla);
            KeyDownObtenerProductos = new ComandoModeloDeVista(EjecutarKeyDownObtenerProductos);
            _adminProductos = new AdminProductos();
            ColoresEntradas = new ColoresProductos();
            Producto = new Productos("-");
            cmbCat = new ComboBox();
            _colores = new ColoresTexto();
            ColorMensaje = _colores.Normal;
            tiempo = new System.Timers.Timer();
            tiempo.Elapsed += ResetearMensaje;
            tiempo.Enabled = false;
            tiempo.AutoReset = true;
            BusquedaCodigo = "Buscar por codigo";
            Busqueda = "Buscar por nombre";
            NombreProducto = "Ninguno";
        }
        #region Columnas privadas
        private double _eColumna_uno;
        private double _eColumna_dos;
        private double _eColumna_tres;
        private double _eColumna_cuatro;
        private double _eColumna_cinco;
        private double _eColumna_seis;
        private double _eColumna_siete;
        private double _tColumna_uno;
        private double _tColumna_dos;
        private double _tColumna_tres;
        private double _tColumna_cuatro;
        private double _tColumna_cinco;
        private double _tColumna_seis;
        private double _tColumna_siete;
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
        #endregion
        #region Propiedades
        private System.Timers.Timer tiempo;
        private AdminProductos _adminProductos;
        private ColoresTexto _colores;
        private ObservableCollection<Categorias> _listaCategorias;
        private ObservableCollection<Categorias> _listaCategoriasParaBusquedas;
        public ObservableCollection<Categorias> ListaCategoriasParaBusquedas
        {
            get => _listaCategoriasParaBusquedas;
            set
            {
                _listaCategoriasParaBusquedas = value;
                OnPropertyChanged(nameof(ListaCategoriasParaBusquedas));
            }
        }
        private Categorias _categoria;
        private Productos _producto;
        private ComboBox cmbCat;
        private ComboBox cmbCatParaBusqueda;
        private string _colorMensaje;
        private string _venta;
        private string _compra;
        private string _nombre;
        private bool _marcado;
        private string _mensaje;
        private string _busquedaCodigo;
        private string _busqueda;
        private string _nombreProducto;
        private bool _verProductosOcultos;
        public bool VerProductosOcultos
        {
            get => _verProductosOcultos;
            set
            {
                _verProductosOcultos = value;
                EjecutarObtenerProductos(null);
                OnPropertyChanged(nameof(VerProductosOcultos));
            }
        }
        public string NombreProducto
        {
            get => _nombreProducto;
            set
            {
                _nombreProducto = value;
                OnPropertyChanged(nameof(NombreProducto));
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
        public string BusquedaCodigo
        {
            get => _busquedaCodigo;
            set
            {
                _busquedaCodigo = value;
                OnPropertyChanged(nameof(BusquedaCodigo));
            }
        }
        private int _idProducto { get; set; }
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
        private ColoresProductos _coloresEntradas;
        public ColoresProductos ColoresEntradas
        {
            get => _coloresEntradas;
            set
            {
                _coloresEntradas = value;
                OnPropertyChanged(nameof(ColoresEntradas));
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
        //Marcado sirve para controlar el checbox dependiendo de que producto se seleccione 
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
        private Categorias _categoriaParaBusqueda;
        public Categorias CategoriaParaBusqueda
        {
            get => _categoriaParaBusqueda;
            set
            {
                _categoriaParaBusqueda = value;
                OnPropertyChanged(nameof(_categoriaParaBusqueda));
            }
        }
        #endregion
        #region Comandos
        public ICommand ActualizarAnchoActual { get; }
        public ICommand ObtenerProductos { get; }
        public ICommand ResetearBusqueda { get; }
        public ICommand BusquedaPorCodigo { get; }
        public ICommand ObtenerCategorias { get; }
        public ICommand ValoresPredeterminados { get; }
        public ICommand CargarDatos { get; }
        public ICommand GotFocus { get; }
        public ICommand LostFocus { get; }
        public ICommand GuardarCmb { get; }
        public ICommand GuardarCmbParaBusqueda { get; }
        public ICommand SelectionChanged { get; }
        public ICommand SelectionChangedParaBusqueda { get; }
        public ICommand EditarProducto { get; }
        public ICommand KeyDownObtenerProductos { get; }
        #endregion
        #region Eventos
        public async Task EjecutarKeyDownObtenerProductos(object arg)
        {
            if(arg is KeyEventArgs e)
            {
                if(e.Key == Key.Enter)
                {
                    EjecutarObtenerProductos(null);
                }
            }
        }
        public async Task EjecutarEditarProducto(object arg)
        {
            try
            {
                if (!_metodoEnEjecucion)
                {
                    _metodoEnEjecucion = true;
                    if (Producto == null)
                        throw new Exception("No hay ningun producto seleccionado");
                    var confirmacion = MessageBox.Show(
                        $"Los campos Nombre y Categoría afectan a todos los lotes asociados al producto con el código de barras: {Producto.Codigo}.\r\n",
                        "¿Quieres continuar?",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question
                    );
                    if (confirmacion != MessageBoxResult.Yes)
                        throw new Exception("Cancelaste la operación");
                    Productos producto = new Productos
                    {
                        Nombre = this.Nombre.ToUpper(),
                        Categoria = this.Categoria.Nombre,
                        Venta = this.Venta.Replace(".", ""),
                        Compra = this.Compra.Replace(".", ""),
                        Codigo = this.Producto.Codigo,
                        ID = this._idProducto,
                        Mostrar = Marcado
                    };
                    var res = await _adminProductos.EditarProductoPorCodigo(producto);
                    _metodoEnEjecucion = false;
                    if (res.estado)
                    {
                        ColorMensaje = _colores.ColorExito;
                        cmbCat.SelectedIndex = 0;
                        EjecutarValoresPredeterminados(null);
                        EjecutarObtenerProductos(null);
                    }
                    else
                    {
                        ColorMensaje = _colores.ColorError;
                    }
                    Mensaje = res.mensaje;
                }
            }
            catch (Exception ex)
            {
                ColorMensaje = _colores.ColorError;
                Mensaje = ex.Message;
                _metodoEnEjecucion = false;
            }
            finally
            {
                tiempo.Interval = 10000;
                tiempo.Enabled = true;
            }
        }
        private void ResetearMensaje(object source, ElapsedEventArgs e)
        {
            tiempo.Enabled = false;
            Mensaje = "";
        }
        private async Task EjecutarGuardarCmb(object arg)
        {
            ComboBox cmb = (ComboBox)arg;
            cmbCat = cmb;
        }
        private async Task EjecutarGuardarCmbParaBusqueda(object arg)
        {
            ComboBox cmb = (ComboBox)arg;
            cmbCatParaBusqueda = cmb;
        }
        private async Task EjecutarSelected(object arg)
        {
            Categorias cate = (Categorias)arg;
            string str = cate.Nombre.ToLower();
            if (str != "categoria*")
                ColoresEntradas.Categoria = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020"));
            else
                ColoresEntradas.Categoria = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
        }
        private async Task EjecutarSelectedParaBusqueda(object arg)
        {
            Categorias cate = (Categorias)arg;
            string str = cate.Nombre.ToLower();
            if (str != "categoria*")
                ColoresEntradas.CategoriaParaBusqueda = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020"));
            else
                ColoresEntradas.CategoriaParaBusqueda = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
        }
        private async Task EjecutarCargarDatos(object obj)
        {
            Productos producto = obj as Productos;
            Producto = producto;
            Nombre = Producto.Nombre;
            Venta = Producto.Venta;
            Compra = Producto.Compra;
            NombreProducto = Producto.Nombre;
            _idProducto = Producto.ID;
            ColoresEntradas.Nombre = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020"));
            ColoresEntradas.Venta = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020"));
            ColoresEntradas.Compra = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020"));
            ColoresEntradas.Cantidad = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020"));
            for (int i = 0; i < cmbCat.Items.Count; i++) 
            {
                Categorias cate = (Categorias)cmbCat.Items[i];
                if(cate.ID == producto.IDCategoria)
                    cmbCat.SelectedIndex = i;
            }
            bool mostrar = false;
            Marcado = Producto.Mostrar;
        }
        private async Task ResetearTabla(object arg)
        {
            BusquedaCodigo = "Buscar por codigo";
            NombreProducto = "Ninguno";
            ColoresEntradas.BusuqedaUno = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
            Busqueda = "Buscar por nombre";
            ColoresEntradas.Busuqeda = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
            EjecutarObtenerProductos(null);
        }
        private async Task EjecutarObtenerProductos(object obj)
        {

            try
            {
                
                string valor;
                if (!_metodoEnEjecucion && (BusquedaCodigo == "Buscar por codigo" || string.IsNullOrWhiteSpace(BusquedaCodigo)))
                {
                    _metodoEnEjecucion = true;
                    valor = Busqueda != "Buscar por nombre" ? Busqueda : "";
                    ObtenerProductosPorNombreYCategoria(valor);
                }
                else if(!_metodoEnEjecucion)
                {
                    _metodoEnEjecucion = true;
                    EjecutarBusquedaPorCodigoUltLote(null);
                }
            }
            catch (Exception ex)
            {
                ColorMensaje = _colores.ColorError;
                Mensaje = ex.Message;
                HistorialErrores error = new HistorialErrores(ex.Message, ex.GetType().FullName, ex.StackTrace, "Kiosco2026.ModeloDeLasVistas.Hijas", "ModeloEditarProducto", "EjecutarObtenerProductos", _configuracion.NombreDeCaja);
                EscribirEnTXTError(error);
            }
            finally
            {
                tiempo.Interval = 10000;
                tiempo.Enabled = true;
                _metodoEnEjecucion = false;
            }
        }
        private async Task ObtenerProductosPorNombreYCategoria(string valor)
        {
            string categoria = CategoriaParaBusqueda is Categorias ? CategoriaParaBusqueda.Nombre : "";
            BusquedaCodigo = "Buscar por codigo";
            ColoresEntradas.BusuqedaUno = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
            int? visible = VerProductosOcultos ? null : 1;
            var res = await _adminProductos.Con_ObtenerUltLotePorNombre(valor, categoria, visible);
            ListaProductos = new ObservableCollection<Productos>(res.lista);
        }
        private async Task EjecutarBusquedaPorCodigoUltLote(object obj)
        {
            Busqueda = "Buscar por nombre";
            cmbCatParaBusqueda.SelectedIndex = 0;
            ColoresEntradas.Busuqeda = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
            int? visible = VerProductosOcultos ? null : 1;
            var res = await _adminProductos.BusquedaProductoPorCodigoUltLote(BusquedaCodigo, visible);
            ListaProductos = new ObservableCollection<Productos>(res.lista);
        }
        private async Task EjecutarActualizarAnchoActual(object arg)
        {
            if(arg is Grid grid)
            {
                double anchoActual = grid.ActualWidth;
                double ancho1 = (anchoActual * 8) / 100;
                double ancho2 = (anchoActual * 14) / 100;
                double ancho3 = (anchoActual * 22) / 100;
                EColumna_uno    = ancho1;
                EColumna_dos    = ancho3;
                EColumna_tres   = ancho2;
                EColumna_cuatro = ancho2;
                EColumna_cinco  = ancho2;
                EColumna_seis   = ancho2;
                EColumna_siete  = ancho2;

                TColumna_uno    = ancho1;
                TColumna_dos    = ancho3;
                TColumna_tres   = ancho2;
                TColumna_cuatro = ancho2;
                TColumna_cinco  = ancho2;
                TColumna_seis   = ancho2;
                TColumna_siete  = ancho2;
            }
        }
        private async Task EjecutarObtenerCategoriasGenerales(object arg)
        {
            try
            {
                var respuesta = await _adminProductos.ObtenerCategoriasCliente();
                List<Categorias> lista = respuesta.lista;
                lista.Insert(0, new Categorias { Nombre = "Categoria*" });
                ListaCategoriasParaBusquedas = new ObservableCollection<Categorias>(lista);
                ListaCategorias = new ObservableCollection<Categorias>(lista);
            }
            catch (Exception ex) 
            {
                HistorialErrores error = new HistorialErrores(ex.Message, ex.GetType().FullName, ex.StackTrace, "Kiosco2026.ModeloDeLasVistas.Hijas", "ModeloEditarProducto", "EjecutarObtenerCategoriasGenerales", _configuracion.NombreDeCaja);
                EscribirEnTXTError(error);
            }
        }
        private async Task EjecutarValoresPredeterminados(object arg)
        {
            Nombre = "Nombre*";
            NombreProducto = "Ninguno";
            Venta = "Precio de venta*";
            Compra = "Costo de compra*";
            Marcado = default;
            Producto = null;
            ColoresEntradas.Nombre = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
            ColoresEntradas.Venta = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
            ColoresEntradas.Compra = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
            ColoresEntradas.Categoria = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
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
                    case "entrada_busqueda": Busqueda = string.Empty; ColoresEntradas.Busuqeda = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020")); break;
                    case "entrada_nombre": Nombre = string.Empty; ColoresEntradas.Nombre = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020")); break;
                    case "entrada_venta": Venta = string.Empty; ColoresEntradas.Venta = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020")); break;
                    case "entrada_compra": Compra = string.Empty; ColoresEntradas.Compra = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020")); break;
                    case "entrada_codigo": BusquedaCodigo = string.Empty; ColoresEntradas.BusuqedaUno = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020")); break;
                }
            }
        }
        private async Task EjecutarLostFocus(object arg)
        {
            string nombre = (string)arg;
            switch (nombre)
            {
                case "entrada_busqueda": if (string.IsNullOrWhiteSpace(Busqueda) || Busqueda.Trim() == "Buscar por nombre") { Busqueda = "Buscar por nombre"; ColoresEntradas.Busuqeda = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A")); } break;
                case "entrada_nombre": if (string.IsNullOrWhiteSpace(Nombre) || Nombre.Trim() == "Nombre*") { Nombre = "Nombre*"; ColoresEntradas.Nombre = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A")); } break;
                case "entrada_venta": if (string.IsNullOrWhiteSpace(Venta) || Venta.Trim() == "Precio de venta*") { Venta = "Precio de venta*"; ColoresEntradas.Venta = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A")); } break;
                case "entrada_compra": if (string.IsNullOrWhiteSpace(Compra) || Compra.Trim() == "Costo de compra*") { Compra = "Costo de compra*"; ColoresEntradas.Compra = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A")); } break;
                case "entrada_codigo": if (string.IsNullOrWhiteSpace(BusquedaCodigo) || BusquedaCodigo.Trim() == "Buscar por codigo") { BusquedaCodigo = "Buscar por codigo"; ColoresEntradas.BusuqedaUno = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A")); } break;
            }
        }
        private bool RevisarEntradas(TextBox entrada)
        {
            string[] valores = ["Nombre*", "Código*", "Precio de venta*", "Costo de compra*", "Cantidad*", "Proveedor", "Buscar por nombre", "Buscar por codigo"];
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
