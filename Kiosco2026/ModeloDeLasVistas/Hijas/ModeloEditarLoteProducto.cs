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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Kiosco2026.ModeloDeLasVistas.Hijas
{
    public class ModeloEditarLoteProducto : ModeloDeVistaBase
    {
        private Configuracion _configuracion { get; }
        public ModeloEditarLoteProducto(Productos pr)
        {
            string ruta = "config.json";
            string jsonDeConfiguracion = File.ReadAllText(ruta);
            _configuracion = JsonSerializer.Deserialize<Configuracion>(jsonDeConfiguracion);
            ActualizarAnchoActual = new ComandoModeloDeVista(EjecutarActualizarAnchoActual);
            ObtenerProductos = new ComandoModeloDeVista(EjecutarObtenerProductos);
            GotFocus = new ComandoModeloDeVista(EjecutarGotFocus);
            LostFocus = new ComandoModeloDeVista(EjecutarLostFocus);
            EditarLote = new ComandoModeloDeVista(EjecutarEditarLote);
            CargarDatosLote = new ComandoModeloDeVista(EjecutarCargarDatosLote);
            BuscarProductos = new ComandoModeloDeVista(EjecutarBusquedaLotes);
            _pr = pr;
            Producto = new Productos("-");
            _adminProductos = new AdminProductos();
            ColoresEntradas = new ColoresProductos();
            tiempo = new System.Timers.Timer();
            tiempo.Elapsed += ResetearMensaje;
            tiempo.Enabled = false;
            tiempo.AutoReset = true;
            EjecutarRestablecerEntradas();
        }
        #region Propiedades
        private System.Timers.Timer tiempo;
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
        private Productos _pr;
        private Productos _producto;
        private string _venta;
        private string _compra;
        private string _cantidad;
        private string _nombre;
        private string _mensaje;
        private Brush _colorMensaje;
        private DateTime? _ingreso;
        private DateTime? _vencimiento;
        private ColoresProductos _coloresEntradas;
        private DateTime? _busquedaIngreso;
        private AdminProductos _adminProductos;
        private ObservableCollection<Productos> _listaProductos;
        public Brush ColorMensaje
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
        public DateTime? Ingreso
        {
            get => _ingreso;
            set
            {
                _ingreso = value;
                OnPropertyChanged(nameof(Ingreso));
            }
        }
        public DateTime? Vencimiento
        {
            get => _vencimiento;
            set
            {
                _vencimiento = value;
                OnPropertyChanged(nameof(Vencimiento));
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
        public Productos Producto
        {
            get => _producto;
            set
            {
                _producto = value;
                OnPropertyChanged(nameof(Producto));
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
        public ObservableCollection<Productos> ListaProductos
        {
            get => _listaProductos;
            set
            {
                _listaProductos = value;
                OnPropertyChanged(nameof(ListaProductos));
            }
        }
        public DateTime? BusquedaIngreso
        {
            get => _busquedaIngreso;
            set
            {
                _busquedaIngreso = value;
                OnPropertyChanged(nameof(_busquedaIngreso));
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
        #endregion
        #region Comandos
        public ICommand ObtenerProductos { get; }
        public ICommand BuscarProductos { get; }
        public ICommand EditarLote { get; }
        public ICommand CargarDatosLote { get; }
        public ICommand ActualizarAnchoActual { get; }
        public ICommand GotFocus { get; }
        public ICommand LostFocus { get; }
        public ICommand RestablecerEntradas { get; }
        public ICommand ResetearBusqueda { get; }
        #endregion
        #region Eventos y funciones
        private async Task EjecutarCargarDatosLote(object obj)
        {
            Productos productoSeleccionado = obj as Productos;
            Producto = productoSeleccionado;
            Nombre = productoSeleccionado.Nombre;
            Compra = productoSeleccionado.Compra;
            Venta = productoSeleccionado.Venta;
            Cantidad = productoSeleccionado.Cantidad.ToString();
            Ingreso = productoSeleccionado.Ingreso;
            Vencimiento = productoSeleccionado.Vencimiento; 
            ColoresEntradas.Nombre = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            ColoresEntradas.Compra = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            ColoresEntradas.Venta = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            ColoresEntradas.Cantidad = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
        }
        private async Task EjecutarObtenerProductos(object obj)
        {
            try
            {
                var res = await _adminProductos.BusquedaPorIngreso(_pr.Codigo, null);
                ListaProductos = new ObservableCollection<Productos>(res.lista);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                HistorialErrores error = new HistorialErrores(ex.Message, ex.GetType().FullName, ex.StackTrace, "Kiosco2026.ModeloDeLasVistas.Hijas", "ModeloEditarLoteProducto", "EjecutarObtenerProductos", _configuracion.NombreDeCaja);
                EscribirEnTXTError(error);
            }
        }
        private async Task EjecutarBusquedaLotes(object obj)
        {
            try
            {
                var res = await _adminProductos.BusquedaPorIngreso(_pr.Codigo, BusquedaIngreso);
                ListaProductos = new ObservableCollection<Productos>(res.lista);
            }
            catch (Exception ex) 
            {
                Debug.WriteLine(ex.Message);
                HistorialErrores error = new HistorialErrores(ex.Message, ex.GetType().FullName, ex.StackTrace, "Kiosco2026.ModeloDeLasVistas.Hijas", "ModeloEditarLoteProducto", "EjecutarBusquedaLotes", _configuracion.NombreDeCaja);
                EscribirEnTXTError(error);
            }
        }
        private async Task EjecutarEditarLote(object obj)
        {
            try
            {
                if (Producto.ID < 1)
                    throw new Exception("No hay ningun lote seleccionado");
                int cantidad;
                if (!int.TryParse(Cantidad, out cantidad))
                    throw new Exception("El campo 'cantidad' no es un numero");
                Productos nuevoProducto = new Productos
                {
                    Nombre = this.Nombre.ToUpper(),
                    Cantidad = cantidad,
                    Compra = this.Compra,
                    Venta = this.Venta,
                    Vencimiento = this.Vencimiento ?? new DateTime(),
                    Ingreso = this.Ingreso ?? new DateTime(),
                    ID = this.Producto.ID,
                };
                var res = await _adminProductos.EditarLote(nuevoProducto);
                ColorMensaje = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#009933"));
                if (!res.estado)
                    ColorMensaje = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#990000"));
                Mensaje = res.menasje;
                EjecutarObtenerProductos(null);
                EjecutarRestablecerEntradas();
            }catch (Exception ex)
            {
                ColorMensaje = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#990000"));
                Mensaje = ex.Message;
                HistorialErrores error = new HistorialErrores(ex.Message, ex.GetType().FullName, ex.StackTrace, "Kiosco2026.ModeloDeLasVistas.Hijas", "ModeloEditarLoteProducto", "EjecutarEditarLote", _configuracion.NombreDeCaja);
                EscribirEnTXTError(error);
            }
            finally
            {
                tiempo.Interval = 10000;
                tiempo.Enabled = true;
            }
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
            EColumna_ocho = relleno;
            TColumna_uno = nueve;
            TColumna_dos = relleno;
            TColumna_tres = relleno;
            TColumna_cuatro = relleno;
            TColumna_cinco = relleno;
            TColumna_seis = relleno;
            TColumna_siete = relleno;
            TColumna_ocho = relleno;
        }
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
                case "entrada_cantidad": if (string.IsNullOrWhiteSpace(Compra) || Compra.Trim() == "Cantidad*") { Compra = "Cantidad*"; ColoresEntradas.Compra = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9")); } break;
            }
        }
        private bool RevisarEntradas(TextBox entrada)
        {
            string[] valores = ["Nombre*", "Precio de venta*", "Costo de compra*", "Cantidad*" ];
            foreach (string valor in valores)
            {
                if (valor == entrada.Text)
                    return true;
            }
            return false;
        }
        private void EjecutarRestablecerEntradas()
        {
            Nombre = "Nombre*";
            Compra = "Costo de compra*";
            Venta = "Precio de venta*";
            Cantidad = "Cantidad*";
            Ingreso = null;
            Vencimiento = null;
            BusquedaIngreso = null;
            ColoresEntradas.Nombre = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
            ColoresEntradas.Compra = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
            ColoresEntradas.Venta = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
            ColoresEntradas.Cantidad = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
        }
        private void ResetearMensaje(object source, ElapsedEventArgs e)
        {
            tiempo.Enabled = false;
            Mensaje = "";
        }
        #endregion
    }
}
