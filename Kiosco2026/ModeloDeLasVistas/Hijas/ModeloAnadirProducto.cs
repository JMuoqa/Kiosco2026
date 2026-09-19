using Dominio;
using Entidades;
using Kiosco2026.Clases.Colores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Kiosco2026.ModeloDeLasVistas.Hijas
{
    public class ModeloAnadirProducto : ModeloDeVistaBase
    {
        private Configuracion _configuracion { get; }
        #region Propiedades
        private bool _metodoEnEjecucion;
        private string _precioVentaEnVivo;
        public string PrecioVentaEnVivo
        {
            get => _precioVentaEnVivo;
            set
            {
                _precioVentaEnVivo = value;
                OnPropertyChanged(nameof(PrecioVentaEnVivo));
            }
        }
        private ComboBoxItem _comboBoxItem;
        public ComboBoxItem ComboBoxItem
        {
            get => _comboBoxItem;
            set
            {
                _comboBoxItem = value;
                string texto = _comboBoxItem.Content.ToString();
                UnidadMedida = texto;
                OnPropertyChanged(nameof(ComboBoxItem));
            }
        }
        private string _unidadMedida;
        public string UnidadMedida
        {
            get => _unidadMedida;
            set
            {
                _unidadMedida = value;
                OnPropertyChanged(nameof(UnidadMedida));
            }
        }
        private System.Timers.Timer tiempo;
        private string _nombre;
        private string _codigo;
        private Categorias _categoria;
        private ColoresTexto _colores;
        private ComboBox _cmbCat;
        private ComboBox _cmbUni;
        private string _colorMensaje;
        private string _margen;
        private string _compra;
        private string _cantidad;
        private string _mensaje;
        private string _proveedor;
        private string _productoNuevo;
        private DateTime _ingreso;
        private DateTime _vencimiento;
        private AdminProductos _adminProductos;
        private ObservableCollection<Categorias> _listaCategorias;
        private ColoresProductos _coloresEntradas;
        public string ProductoNuevo
        {
            get => _productoNuevo;
            set
            {
                _productoNuevo = value;
                OnPropertyChanged(nameof(ProductoNuevo));
            }
        }
        public ColoresProductos ColoresEntradas
        {
            get => _coloresEntradas;
            set
            {
                _coloresEntradas = value;
                OnPropertyChanged(nameof(_coloresEntradas));
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
        public string ColorMensaje
        {
            get => _colorMensaje;
            set
            {
                _colorMensaje = value;
                OnPropertyChanged(nameof(ColorMensaje));
            }
        }
        public string Codigo
        {
            get => _codigo;
            set
            {
                _codigo = value;
                OnPropertyChanged(nameof(Codigo));
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
        public Categorias Categoria
        {
            get => _categoria;
            set
            {
                _categoria = value;
                OnPropertyChanged(nameof(Categoria));
            }
        }
        public string Margen
        {
            get => _margen;
            set
            {
                _margen = value;
                PrecioEnVivo();
                OnPropertyChanged(nameof(Margen));
            }
        }
        public string Compra
        {
            get => _compra;
            set
            {
                _compra = value;
                PrecioEnVivo();
                OnPropertyChanged(nameof(Compra));
            }
        }
        public string Cantidad
        {
            get => _cantidad;
            set
            {
                _cantidad = value;
                PrecioEnVivo();
                OnPropertyChanged(nameof(Cantidad));
            }
        }
        public string Proveedor
        {
            get => _proveedor;
            set
            {
                _proveedor = value;
                OnPropertyChanged(nameof(Proveedor));
            }
        }
        public DateTime Ingreso
        {
            get => _ingreso;
            set
            {
                _ingreso = value;
                OnPropertyChanged(nameof(Ingreso));
            }
        }
        public DateTime Vencimiento
        {
            get => _vencimiento;
            set
            {
                _vencimiento = value;
                OnPropertyChanged(nameof(Vencimiento));
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
        private TextBox txtBox;
        #endregion
        public ModeloAnadirProducto()
        {
            _metodoEnEjecucion = false;
            string ruta = "config.json";
            string jsonDeConfiguracion = File.ReadAllText(ruta);
            _configuracion = JsonSerializer.Deserialize<Configuracion>(jsonDeConfiguracion);
            ColoresEntradas = new ColoresProductos();
            ObtenerCategoriasGenerales = new ComandoModeloDeVista(EjecutarObtenerCategoriasGenerales);
            CargarProducto = new ComandoModeloDeVista(EjecutarCargarProducto);
            ValoresPredeterminados = new ComandoModeloDeVista(EjecutarValoresPredeterminados);
            GotFocus = new ComandoModeloDeVista(EjecutarGotFocus);
            LostFocus = new ComandoModeloDeVista(EjecutarLostFocus);
            SelectionChanged = new ComandoModeloDeVista(EjecutarSelected);
            SelectionChangedUnidadMedida = new ComandoModeloDeVista(EjecutarSelectedUnidadMedida);
            ComprobarExistencia = new ComandoModeloDeVista(EjecutarComprobarExistencia);
            GuardarCmb = new ComandoModeloDeVista(EjecutarGuardarCmbCat);
            GuardarCmbUni= new ComandoModeloDeVista(EjecutarGuardarCmbUni);
            SelecionarTextBox = new ComandoModeloDeVista(EjecutarSelecionarTextBox);
            _colores = new ColoresTexto();
            ColorMensaje = _colores.Normal;
            _adminProductos = new AdminProductos();
            _cmbCat = new ComboBox();
            tiempo = new System.Timers.Timer(); 
            tiempo.Elapsed += ResetearMensaje;
            tiempo.Enabled = false;
            tiempo.AutoReset = true;
        }
        #region Comandos
        public ICommand ComprobarExistencia { get; }
        public ICommand ObtenerCategoriasGenerales { get; }
        public ICommand CargarProducto { get; }
        public ICommand ValoresPredeterminados { get; }
        public ICommand EditarCategoria { get; }
        public ICommand GotFocus { get; }
        public ICommand LostFocus { get; }
        public ICommand SelectionChanged { get; }
        public ICommand SelectionChangedUnidadMedida { get; }
        public ICommand GuardarCmb { get; }
        public ICommand GuardarCmbUni { get; }
        public ICommand SelecionarTextBox { get; }
        #endregion
        #region Funciones
        private void PrecioEnVivo()
        {
            decimal d = 0;
            if ((decimal.TryParse(Compra, out decimal n1) && decimal.TryParse(Cantidad, out decimal n2)) && decimal.TryParse(Margen, out decimal n3))
            {
                d = (n1 + (n1 * n3 / 100)) / n2;
            }
            PrecioVentaEnVivo = "$" + d.ToString("N2", new CultureInfo("es-AR"));
        }
        private async Task EjecutarGuardarCmbCat(object arg)
        {
            ComboBox cmb = (ComboBox)arg;
            _cmbCat = cmb;
        }
        private async Task EjecutarGuardarCmbUni(object arg)
        {
            ComboBox cmb = (ComboBox)arg;
            _cmbUni = cmb;
        }
        private async Task RevisarExistencia()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Codigo) || Codigo == "Código de barras*") return;
                if (!_metodoEnEjecucion)
                {
                    _metodoEnEjecucion = true;
                    var res = await _adminProductos.RevisarExistenciaProducto(Codigo);
                    if (!res.estado)
                        throw new Exception("Ocurrio un error. Contacta a soporte para solucionarlo");
                    if (res.existencia)
                    {
                        ProductoNuevo = res.mensaje;
                        Codigo = res.producto.Codigo;
                        Nombre = res.producto.Nombre;
                        Proveedor = res.producto.Proveedor;
                        ColoresEntradas.Nombre = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020"));
                        ColoresEntradas.Codigo = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020"));
                        ColoresEntradas.UnidadMedida = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020"));
                        ColoresEntradas.Proveedor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020"));
                        for (int i = 0; i < _cmbCat.Items.Count; i++)
                        {
                            Categorias cate = (Categorias)_cmbCat.Items[i];
                            if (cate.ID == res.producto.IDCategoria)
                                _cmbCat.SelectedIndex = i;
                        }
                        for (int i = 0; i < _cmbUni.Items.Count; i++)
                        {
                            ComboBoxItem cmbItem = (ComboBoxItem)_cmbUni.Items[i];
                            string texto = cmbItem.Content.ToString();
                            if (texto == res.producto.UnidadMedida)
                                _cmbUni.SelectedIndex = i;
                        }
                    }
                    else
                    {
                        ProductoNuevo = res.mensaje;
                    }
                    _metodoEnEjecucion = false;
                }
            }
            catch (Exception ex)
            {
                ColorMensaje = _colores.ColorError;
                Mensaje = ex.Message;
                HistorialErrores error = new HistorialErrores(ex.Message, ex.GetType().FullName, ex.StackTrace, "Kiosco2026.ModeloDeLasVistas.Hijas", "ModeloAnadirProducto", "EjecutarComprobarExistencia", _configuracion.NombreDeCaja);
                EscribirEnTXTError(error);
                _metodoEnEjecucion = false;
            }
            finally
            {
                tiempo.Interval = 15000;
                tiempo.Enabled = true;
            }
        }
        private async Task EjecutarComprobarExistencia(object arg)
        {
            try
            {
                var e = (KeyEventArgs)arg;
                if (e.Key == Key.Enter)
                {
                    if (string.IsNullOrWhiteSpace(Codigo) || Codigo == "Código de barras*")
                        throw new Exception("Para buscar si existe un producto, pon el código de barras");
                    RevisarExistencia();
                }
            }
            catch (Exception ex)
            {
                ColorMensaje = _colores.ColorError;
                Mensaje = ex.Message;
                HistorialErrores error = new HistorialErrores(ex.Message, ex.GetType().FullName, ex.StackTrace, "Kiosco2026.ModeloDeLasVistas.Hijas", "ModeloAnadirProducto", "EjecutarComprobarExistencia", _configuracion.NombreDeCaja);
                EscribirEnTXTError(error);
                _metodoEnEjecucion = false;
            }
            finally
            {
                tiempo.Interval = 15000;
                tiempo.Enabled = true;
            }
        }
        private async Task EjecutarObtenerCategoriasGenerales(object arg)
        {
            var respuesta = await _adminProductos.ObtenerCategoriasCliente();
            List<Categorias> lista = respuesta.lista; 
            lista.Insert(0, new Categorias { Nombre = "Categoria*" });
            ListaCategorias = new ObservableCollection<Categorias>(lista);
        }
        private async Task EjecutarCargarProducto(object arg)
        {
            try
            {
                if (!_metodoEnEjecucion)
                {
                    _metodoEnEjecucion = true;
                    ComboBox cmb = (ComboBox)arg; // ComboBox cmb = arg as Combobox;
                    DateTime horaActual = DateTime.Now;
                    DateTime fechaSeleccionadaHoraActual = new DateTime(
                        Ingreso.Year,
                        Ingreso.Month,
                        Ingreso.Day,
                        horaActual.Hour,
                        horaActual.Minute,
                        horaActual.Second
                    );
                    ProductosDto producto = new ProductosDto
                    {
                        Nombre = this.Nombre,
                        Categoria = this.Categoria.Nombre, //Categoria deberia ser un numero
                        Codigo = this.Codigo,
                        Margen = this.Margen,
                        Compra = this.Compra.Replace(".", ""),
                        Cantidad = this.Cantidad,
                        Proveedor = this.Proveedor.ToUpper(),
                        Ingreso = fechaSeleccionadaHoraActual,
                        Vencimiento = this.Vencimiento,
                        UnidadMedida = this.UnidadMedida
                    };
                    var respuesta = await _adminProductos.CargarProducto(producto);
                    if (respuesta.estado)
                    {
                        ColorMensaje = _colores.ColorExito;
                        PostCarga();
                        cmb.SelectedIndex = 0;
                    }
                    else
                        ColorMensaje = _colores.ColorError;
                    Mensaje = respuesta.mensaje;
                    _metodoEnEjecucion = false;
                }
            }
            catch (Exception ex) 
            {
                ColorMensaje = _colores.ColorError;
                Mensaje = ex.Message;
                HistorialErrores error = new HistorialErrores(ex.Message, ex.GetType().FullName, ex.StackTrace, "Kiosco2026.ModeloDeLasVistas.Hijas", "ModeloAnadirProducto", "EjecutarCargarProducto", _configuracion.NombreDeCaja);
                EscribirEnTXTError(error);
                _metodoEnEjecucion = false;
            }
            finally
            {
                ProductoNuevo = "";
                tiempo.Interval = 15000;
                tiempo.Enabled = true;
            }
        }
        #endregion
        #region Grafica
        private async Task EjecutarSelecionarTextBox(object arg)
        {
            if (arg is TextBox txt)
            {
                await Task.Delay(100);
                txtBox = (TextBox)txt;
                txt.Focus();
                Keyboard.Focus(txt);
            }

        }

        private void ResetearMensaje(object source, ElapsedEventArgs e)
        {
            tiempo.Enabled = false;
            Mensaje = "";
        }
        private async Task EjecutarValoresPredeterminados(object arg)
        {
            Nombre = "Nombre*";
            Codigo = "Código de barras*";
            Margen = "Ganancia sobre costo (%)*";
            Compra = "Costo total de compra*";
            Cantidad = "Cantidad*";
            Proveedor = "Proveedor";
            ColoresEntradas.Nombre = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
            ColoresEntradas.Codigo = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
            ColoresEntradas.Venta = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
            ColoresEntradas.Compra = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
            ColoresEntradas.Cantidad = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
            ColoresEntradas.Proveedor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
            ColoresEntradas.Categoria = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
            ColoresEntradas.UnidadMedida = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
            DateTime hoy = DateTime.Today;
            DateTime AnoDespues = hoy.AddYears(1);
            Ingreso = hoy;
            Vencimiento = AnoDespues;
            ColorMensaje = _colores.Normal;
            EjecutarObtenerCategoriasGenerales(null);
        }
        private async Task EjecutarSelected (object arg)
        {
            Categorias cate = (Categorias)arg;
            string str = cate.Nombre.ToLower();
            if (str != "categoria*")
                ColoresEntradas.Categoria = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020"));
            else
                ColoresEntradas.Categoria= new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
        }
        private async Task EjecutarSelectedUnidadMedida (object arg)
        {
            ComboBoxItem item = (ComboBoxItem)arg;
            string texto = (string)item.Content;
            if(texto != "Unidad de medida*")
                ColoresEntradas.UnidadMedida = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020"));
            else
                ColoresEntradas.UnidadMedida = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
        }
        private async Task EjecutarGotFocus(object arg)
        {
            TextBox entrada = (TextBox)arg;
            if (RevisarEntradas(entrada))
            {
                switch (entrada.Name)
                {
                    case "entrada_nombre": Nombre = string.Empty; ColoresEntradas.Nombre = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020")); break;
                    case "entrada_codigo": Codigo = string.Empty; ColoresEntradas.Codigo = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020")); break;
                    case "entrada_venta": Margen = string.Empty; ColoresEntradas.Venta = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020")); break;
                    case "entrada_compra": Compra = string.Empty; ColoresEntradas.Compra = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020")); break;
                    case "entrada_cantidad": Cantidad = string.Empty; ColoresEntradas.Cantidad = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020")); break;
                    case "entrada_proveedor": Proveedor = string.Empty; ColoresEntradas.Proveedor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020")); break;
                }
            }
        }
        private async Task EjecutarLostFocus(object arg)
        {
            RevisarExistencia();
            string nombre = (string)arg;
            switch (nombre)
            {
                case "entrada_nombre": if (string.IsNullOrWhiteSpace(Nombre) || Nombre.Trim() == "Nombre*") { Nombre = "Nombre*"; ColoresEntradas.Nombre = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A")); } break;
                case "entrada_codigo": if (string.IsNullOrWhiteSpace(Codigo) || Codigo.Trim() == "Código de barras*") { Codigo = "Código de barras*"; ColoresEntradas.Codigo = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A")); } break;
                case "entrada_venta": if (string.IsNullOrWhiteSpace(Margen) || Margen.Trim() == "Ganancia sobre costo (%)*") { Margen = "Ganancia sobre costo (%)*"; ColoresEntradas.Venta = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A")); } break;
                case "entrada_compra": if (string.IsNullOrWhiteSpace(Compra) || Compra.Trim() == "Costo total de compra*") { Compra = "Costo total de compra*"; ColoresEntradas.Compra = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A")); } break;
                case "entrada_cantidad": if (string.IsNullOrWhiteSpace(Cantidad) || Cantidad.Trim() == "Cantidad*") { Cantidad = "Cantidad*"; ColoresEntradas.Cantidad = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A")); } break;
                case "entrada_proveedor": if (string.IsNullOrWhiteSpace(Proveedor) || Proveedor.Trim() == "Proveedor") { Proveedor = "Proveedor"; ColoresEntradas.Proveedor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A")); } break;
            }
        }
        private bool RevisarEntradas(TextBox entrada)
        {
            string[] valores = ["Nombre*", "Código de barras*", "Ganancia sobre costo (%)*", "Costo total de compra*", "Cantidad*", "Proveedor"];
            foreach (string valor in valores)
            {
                if (valor == entrada.Text)
                    return true;
            }
            return false;

        }
        private void PostCarga()
        {
            Nombre = "Nombre*";
            Codigo = "Código de barras*";
            Margen = "Ganancia sobre costo (%)*";
            Compra = "Costo total de compra*";
            Cantidad = "Cantidad*";
            Proveedor = "Proveedor";
            Categoria = new Categorias("Categoria*");
            _cmbUni.SelectedIndex = 0;
            ColoresEntradas.Nombre = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
            ColoresEntradas.Codigo = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
            ColoresEntradas.Venta = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
            ColoresEntradas.Compra = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
            ColoresEntradas.Cantidad = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
            ColoresEntradas.Proveedor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
            ColoresEntradas.Categoria = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
            DateTime hoy = DateTime.Today;
            DateTime AnoDespues = hoy.AddYears(1);
            Ingreso = hoy;
            Vencimiento = AnoDespues;
        }
        #endregion
    }
}
