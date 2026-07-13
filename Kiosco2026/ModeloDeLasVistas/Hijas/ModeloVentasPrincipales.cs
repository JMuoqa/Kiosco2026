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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Kiosco2026.ModeloDeLasVistas.Hijas
{
    public class ModeloVentasPrincipales : ModeloDeVistaBase
    {
        public ModeloVentasPrincipales()
        {
            ActualizarAnchoActual = new ComandoModeloDeVista(EjecutarActualizarAnchoActual);
            SelectionChanged = new ComandoModeloDeVista(EjecutarSelected);
            ValoresPredeterminados = new ComandoModeloDeVista(EjecutarValoresPredeterminados);
            GotFocus = new ComandoModeloDeVista(EjecutarGotFocus);
            LostFocus = new ComandoModeloDeVista(EjecutarLostFocus);
            KeyDownCargarItem = new ComandoModeloDeVista(EjecutarKeyDownCargarItem);
            CargarItem = new ComandoModeloDeVista(EjecutarCargarItem);
            EliminarItem = new ComandoModeloDeVista(EjecutarEliminarItem);
            RestablecerValores = new ComandoModeloDeVista(EjecutarRestablecerValores);
            RegistrarVenta = new ComandoModeloDeVista(EjecutarRegistrarVenta);
            SelecionarTextBox = new ComandoModeloDeVista(EjecutarSelecionarTextBox);
            ListaProductos = new ObservableCollection<Productos>();
            _adminVenta = new AdminVentas();
            _adminProductos = new AdminProductos();
            ColoresEntradas = new ColoresProductos();
            _venta = new Ventas();
        }
        #region Columnas privadas
        private double _eColumna_uno;
        private double _eColumna_dos;
        private double _eColumna_tres;
        private double _eColumna_cuatro;
        private double _tColumna_uno;
        private double _tColumna_dos;
        private double _tColumna_tres;
        private double _tColumna_cuatro;
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
        #endregion
        private string _cantidadItems;
        private string _precioTotal;
        private string _cantidad;
        private string _codigo;
        private string _descuento;
        private string _subtotal;
        private string _cantidadProductos;
        private bool permitido = true;
        private Ventas _venta;
        private AdminVentas _adminVenta;
        private AdminProductos _adminProductos;
        private Categorias _categoria;
        private ColoresProductos _coloresEntradas;
        private string _formaDePago;
        public Ventas Venta
        {
            get => _venta;
            set
            {
                _venta = value;
                OnPropertyChanged(nameof(Venta));
            }
        }
        public string FormaDePago
        {
            get => _formaDePago;
            set
            {
                _formaDePago = value;
                OnPropertyChanged(nameof(FormaDePago));
            }
        }
        public string CantidadProductos
        {
            get => _cantidadProductos;
            set
            {
                _cantidadProductos = value;
                OnPropertyChanged(nameof(CantidadProductos));
            }
        }
        public string CodigoBarras
        {
            get => _codigo;
            set
            {
                _codigo = value;
                OnPropertyChanged(nameof(CodigoBarras));
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
        public ColoresProductos ColoresEntradas
        {
            get => _coloresEntradas;
            set
            {
                _coloresEntradas = value;
                OnPropertyChanged(nameof(_coloresEntradas));
            }
        }
        public string PrecioTotal
        {
            get => _precioTotal;
            set
            {
                _precioTotal = value;
                OnPropertyChanged(nameof(PrecioTotal));
            }
        }
        public string CantidadItems
        {
            get => _cantidadItems;
            set
            {
                _cantidadItems = value;
                OnPropertyChanged(nameof(CantidadItems));
            }
        }
        private ObservableCollection<Productos> _listaProductos;
        public ObservableCollection<Productos> ListaProductos
        {
            get => _listaProductos;
            set
            {
                _listaProductos = value;
                CantidadItems = _listaProductos.Count.ToString();
                decimal calculoTotal = CalcularTotal();
                int calculoCantidadProductos = CalcularCantidadProductos();
                PrecioTotal = "$" + calculoTotal.ToString("N2", new CultureInfo("es-AR"));
                CantidadProductos = calculoCantidadProductos.ToString();
                Venta = new Ventas(_listaProductos.ToList(), 0, calculoTotal, calculoTotal, _listaProductos.Count, calculoCantidadProductos, FormaDePago);
                OnPropertyChanged(nameof(ListaProductos));
            }
        }
        #region Comandos
        public ICommand ActualizarAnchoActual { get; }
        public ICommand SelectionChanged { get; }
        public ICommand RestablecerValores { get; }
        public ICommand ValoresPredeterminados { get; }
        public ICommand GotFocus { get; }
        public ICommand LostFocus { get; }
        public ICommand KeyDownCargarItem { get; }
        public ICommand CargarItem { get; }
        public ICommand EliminarItem { get; }
        public ICommand RegistrarVenta { get; }
        public ICommand SelecionarTextBox { get; }
        #endregion
        #region Funciones
        private async Task EjecutarSelecionarTextBox(object arg)
        {
            TextBox txt = (TextBox)arg;
            txt.Focus();
        }
        private async Task EjecutarRegistrarVenta(object arg)
        {
            try
            {
                DateTime hoy = DateTime.Now;
                string total = PrecioTotal.Replace("$", "");
                int cantidadTotal;
                decimal precioTotal;
                if (!int.TryParse(CantidadProductos, out cantidadTotal))
                    throw new Exception("Algo salio mal, informanos de este error");
                if(!decimal.TryParse(total, out precioTotal))
                    throw new Exception("Algo salio mal, informanos de este error");
                Ventas venta = new Ventas
                {
                    Fecha = hoy,
                    CantidadProductos = cantidadTotal,
                    Total = precioTotal,
                    FormaDePago = this.FormaDePago,
                };
                var res = await _adminVenta.RegistrarVenta(ListaProductos.ToList(), venta);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async Task EjecutarRestablecerValores(object arg)
        {
            var confirmacion = MessageBox.Show(
                $"Se reiniciara la tabla\r\n",
                "¿Quieres continuar?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );
            if(confirmacion == MessageBoxResult.Yes)
                ListaProductos = new ObservableCollection<Productos>(new List<Productos>());
        }
        private async Task EjecutarEliminarItem(object arg)
        {
            Productos p = (Productos)arg;
            var confirmacion = MessageBox.Show(
                $"Se va a eliminar el item: \"{p.Codigo}\" con la cantidad \"{p.Cantidad}\"?\r\n",
                "¿Quieres continuar?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );
            if (confirmacion == MessageBoxResult.Yes)
            {
                for (int i = 0; i < ListaProductos.Count; i++)
                {
                    if (p == ListaProductos[i])
                    {
                        ListaProductos.RemoveAt(i);
                        ListaProductos = ListaProductos;
                        break;
                    }
                }
            }
        }
        private int CalcularCantidadProductos()
        {
            int c = 0;
            for (int i = 0; i < ListaProductos.Count; i++)
            {
                c += ListaProductos[i].Cantidad;
            }
            return c;
        }
        private decimal CalcularTotal()
        {
            decimal total = 0;
            for(int i = 0; i < ListaProductos.Count; i++)
            {
                decimal n = 0;
                if (!decimal.TryParse(ListaProductos[i].Venta, NumberStyles.Any, new CultureInfo("es-AR"), out n))
                    throw new Exception("Hubo un problema");
                total += n * ListaProductos[i].Cantidad;    
            }
            return total;
        }
        private async Task EjecutarKeyDownCargarItem(object arg)
        {
            try
            {
                ColoresEntradas.Codigo = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
                CodigoBarras = CodigoBarras.Replace("Codigo de barras", "");
                if (arg is KeyEventArgs e)
                {
                    if (e.Key == Key.Enter)
                    {
                        EjecutarCargarItem(null);
                    }
                }
            }
            catch (Exception ex) 
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async Task EjecutarCargarItem(object arg)
        {
            try
            {
                if (permitido)
                {
                    permitido = false;
                    int c;
                    if (!int.TryParse(Cantidad, out c))
                        throw new Exception("El campo cantidad no es numerico");
                    if (c <= 0)
                        throw new Exception("El campo cantidad no puede ser menor ni igual a 0");
                    if (CodigoBarras == "Codigo de barras" || string.IsNullOrWhiteSpace(CodigoBarras))
                        throw new Exception("No pusiste ningun coodigo de barras");
                    var res = await _adminProductos.BusquedaProductoPorCodigoUltLote(CodigoBarras);
                    if (!res.estado)
                        throw new Exception(res.mensaje);
                    List<Productos> p = ListaProductos.ToList();
                    Productos productoElejido = res.lista[0];
                    productoElejido.Cantidad = c;
                    p.Add(productoElejido);
                    ListaProductos = new ObservableCollection<Productos>(p);
                    EjecutarValoresPredeterminados(null);
                    permitido = true;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                permitido = true;
            }
        }
        private async Task EjecutarSelected(object arg)
        {
            Categorias cate = (Categorias)arg;
            ColoresEntradas.Categoria = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
        }
        private async Task EjecutarActualizarAnchoActual(object arg)
        {
            double anchoActual;
            if (arg == null)
                anchoActual = 1100;
            else
                anchoActual = (double)arg;
            double tamano = (anchoActual * 25) / 100;
            EColumna_uno = tamano;
            EColumna_dos = tamano;
            EColumna_tres = tamano;
            EColumna_cuatro = tamano;
            TColumna_uno = tamano;
            TColumna_dos = tamano;
            TColumna_tres = tamano;
            TColumna_cuatro = tamano;
        }
        private async Task EjecutarValoresPredeterminados(object arg)
        {
            CodigoBarras = "Codigo de barras";
            Cantidad = "1";
            ColoresEntradas.Codigo = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
            ColoresEntradas.Cantidad = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            ColoresEntradas.Categoria = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
        }
        private async Task EjecutarGotFocus(object arg)
        {
            TextBox entrada = (TextBox)arg;
            if (entrada.Name == "entrada_cantidad")
                Cantidad = "";
            if (RevisarEntradas(entrada))
            {
                switch (entrada.Name)
                {
                    case "entrada_codigo": CodigoBarras = string.Empty; ColoresEntradas.Codigo = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")); break;
                }
            }
        }
        private async Task EjecutarLostFocus(object arg)
        {
            string nombre = (string)arg;
            switch (nombre)
            {
                case "entrada_codigo": if (string.IsNullOrWhiteSpace(CodigoBarras) || CodigoBarras.Trim() == "Codigo de barras") { CodigoBarras = "Codigo de barras"; ColoresEntradas.Codigo = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9")); } break;
                case "entrada_cantidad": if (string.IsNullOrWhiteSpace(Cantidad)) Cantidad = "1"; break;
            }
        }
        private bool RevisarEntradas(TextBox entrada)
        {
            string[] valores = ["Codigo de barras"];
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
