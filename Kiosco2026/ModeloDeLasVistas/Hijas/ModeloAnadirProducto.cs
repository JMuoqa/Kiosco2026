using Dominio;
using Entidades;
using Kiosco2026.Clases.Colores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
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
        public ModeloAnadirProducto()
        {
            ColoresEntradas = new ColoresProductos();
            ObtenerCategoriasGenerales = new ComandoModeloDeVista(EjecutarObtenerCategoriasGenerales);
            CargarProducto = new ComandoModeloDeVista(EjecutarCargarProducto);
            ValoresPredeterminados = new ComandoModeloDeVista(EjecutarValoresPredeterminados);
            GotFocus = new ComandoModeloDeVista(EjecutarGotFocus);
            LostFocus = new ComandoModeloDeVista(EjecutarLostFocus);
            SelectionChanged = new ComandoModeloDeVista(EjecutarSelected);
            _colores = new ColoresTexto();
            _adminProductos = new AdminProductos();
            ColorMensaje = _colores.Normal;
            tiempo = new System.Timers.Timer();
            tiempo.Elapsed += ResetearMensaje;
            tiempo.Enabled = false;
            tiempo.AutoReset = true;
        }
        #region Propiedades
        private System.Timers.Timer tiempo;
        private string _nombre;
        private string _codigo;
        private Categorias _categoria;
        private ColoresTexto _colores;
        private string _colorMensaje;
        private string _venta;
        private string _compra;
        private string _cantidad;
        private string _mensaje;
        private string _proveedor;
        private DateTime _ingreso;
        private DateTime _vencimiento;
        private AdminProductos _adminProductos;
        private ObservableCollection<Categorias> _listaCategorias;
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
        public ICommand ObtenerCategoriasGenerales { get; }
        public ICommand CargarProducto { get; }
        public ICommand ValoresPredeterminados { get; }
        public ICommand EditarCategoria { get; }
        public ICommand GotFocus { get; }
        public ICommand LostFocus { get; }
        public ICommand SelectionChanged { get; }
        #endregion 
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
        private async Task EjecutarCargarProducto(object arg)
        {
            try
            {
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
                    Venta = this.Venta,
                    Compra = this.Compra,
                    Cantidad = this.Cantidad,
                    Proveedor = this.Proveedor,
                    Ingreso = fechaSeleccionadaHoraActual,
                    Vencimiento = this.Vencimiento
                };
                var respuesta = await _adminProductos.CargarProducto(producto);
                if (respuesta.estado){
                    ColorMensaje = _colores.ColorExito;
                    PostCarga();
                    cmb.SelectedIndex = 0;
                }
                else
                    ColorMensaje = _colores.ColorError;
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
        #region Grafica

        private void ResetearMensaje(object source, ElapsedEventArgs e)
        {
            tiempo.Enabled = false;
            Mensaje = "";
        }
        private async Task EjecutarValoresPredeterminados(object arg)
        {
            Nombre = "Nombre*";
            Codigo = "Código*";
            Venta = "Precio de venta*";
            Compra = "Costo de compra*";
            Cantidad = "Cantidad*";
            Proveedor = "Proveedor";
            ColoresEntradas.Nombre = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
            ColoresEntradas.Codigo = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
            ColoresEntradas.Venta = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
            ColoresEntradas.Compra = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
            ColoresEntradas.Cantidad = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
            ColoresEntradas.Proveedor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
            ColoresEntradas.Categoria = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
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
                ColoresEntradas.Categoria = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            else
                ColoresEntradas.Categoria= new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
        }
        private async Task EjecutarGotFocus(object arg)
        {
            TextBox entrada = (TextBox)arg;
            if (RevisarEntradas(entrada))
            {
                switch (entrada.Name)
                {
                    case "entrada_nombre": Nombre = string.Empty; ColoresEntradas.Nombre = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")); break;
                    case "entrada_codigo": Codigo = string.Empty; ColoresEntradas.Codigo = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")); break;
                    case "entrada_venta": Venta = string.Empty; ColoresEntradas.Venta = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")); break;
                    case "entrada_compra": Compra = string.Empty; ColoresEntradas.Compra = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")); break;
                    case "entrada_cantidad": Cantidad = string.Empty; ColoresEntradas.Cantidad = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")); break;
                    case "entrada_proveedor": Proveedor = string.Empty; ColoresEntradas.Proveedor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")); break;
                }
            }
        }
        private async Task EjecutarLostFocus(object arg)
        {
            string nombre = (string)arg;
            switch (nombre)
            {
                case "entrada_nombre": if (string.IsNullOrWhiteSpace(Nombre) || Nombre.Trim() == "Nombre*") { Nombre = "Nombre*"; ColoresEntradas.Nombre = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9")); } break;
                case "entrada_codigo": if (string.IsNullOrWhiteSpace(Codigo) || Codigo.Trim() == "Código*") { Codigo = "Código*"; ColoresEntradas.Codigo = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9")); } break;
                case "entrada_venta": if (string.IsNullOrWhiteSpace(Venta) || Venta.Trim() == "Precio de venta*") { Venta= "Precio de venta*"; ColoresEntradas.Venta = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9")); } break;
                case "entrada_compra": if (string.IsNullOrWhiteSpace(Compra) || Compra.Trim() == "Costo de compra*") { Compra = "Costo de compra*"; ColoresEntradas.Compra = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9")); } break;
                case "entrada_cantidad": if (string.IsNullOrWhiteSpace(Cantidad) || Cantidad.Trim() == "Cantidad*") { Cantidad = "Cantidad*"; ColoresEntradas.Cantidad = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9")); } break;
                case "entrada_proveedor": if (string.IsNullOrWhiteSpace(Proveedor) || Proveedor.Trim() == "Proveedor") { Proveedor = "Proveedor"; ColoresEntradas.Proveedor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9")); } break;
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
        private void PostCarga()
        {
            Nombre = "Nombre*";
            Codigo = "Código*";
            Venta = "Precio de venta*";
            Compra = "Costo de compra*";
            Cantidad = "Cantidad*";
            Proveedor = "Proveedor";
            Categoria = new Categorias("Categoria*");
            ColoresEntradas.Nombre = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
            ColoresEntradas.Codigo = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
            ColoresEntradas.Venta = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
            ColoresEntradas.Compra = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
            ColoresEntradas.Cantidad = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
            ColoresEntradas.Proveedor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
            ColoresEntradas.Categoria = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
            DateTime hoy = DateTime.Today;
            DateTime AnoDespues = hoy.AddYears(1);
            Ingreso = hoy;
            Vencimiento = AnoDespues;
        }
        #endregion
    }
}
