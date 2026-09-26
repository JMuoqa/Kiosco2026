using Dominio;
using Entidades;
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
using System.Windows.Input;

namespace Kiosco2026.ModeloDeLasVistas.Hijas
{
    public class ModeloDetallesVenta : ModeloDeVistaBase
    {
        private Configuracion _configuracion { get; }
        private bool _metodoEnEjecucion;
        string carpetaDatos = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Muoqa",
            "MuoqaStock"
        );
        public ModeloDetallesVenta(Ventas venta)
        {
            _metodoEnEjecucion = false;
            string ruta = carpetaDatos + "\\config.json";
            string jsonDeConfiguracion = File.ReadAllText(ruta);
            _configuracion = JsonSerializer.Deserialize<Configuracion>(jsonDeConfiguracion);
            _venta = venta;
            _adminVentas = new AdminVentas();
            ObtenerDetallesDeUnaVenta = new ComandoModeloDeVista(EjecutarObtenerDetallesDeUnaVenta);
            PreviewKeyDownParaCerrar = new ComandoModeloDeVista(EjecutarPreviewKeyDownParaCerrar);
        }
        #region Propiedades
        public event Action CerrarVentana;
        private AdminVentas _adminVentas;
        private Ventas _venta;
        public string Dia
        {
            get
            {
                string[] str = _venta.FechaARG.Split(' ');
                return str[0];
            }
        }
        public string Hora { 
            get
            {
                string[] str = _venta.FechaARG.Split(' ');
                return str[1];
            } 
        }
        private string _caja;
        public string Caja
        {
            get => _caja;
            set
            {
                _caja = value;
                OnPropertyChanged(nameof(Caja));
            }
        }
        private string _cajero;
        public string Cajero
        {
            get => _cajero;
            set
            {
                _cajero = value;
                OnPropertyChanged(nameof(Cajero));
            }
        }
        private string _cantidadDeItems;
        public string CantidadDeItems {
            get => _cantidadDeItems;
            set
            {
                _cantidadDeItems = value;
                OnPropertyChanged(nameof(CantidadDeItems));
            }
        }
        private string _cantidadDeProductos;
        public string CantidadDeProductos 
        {
            get => _cantidadDeProductos;
            set
            {
                _cantidadDeProductos = value;
                OnPropertyChanged(nameof(CantidadDeProductos));
            } 
        }
        private ObservableCollection<Productos> _listaProductos;
        public ObservableCollection<Productos> ListaProductos
        {
            get => _listaProductos;
            set
            {
                _listaProductos = value;
                decimal n = CalcularTotal();
                SubTotal = n.ToString("N2", new CultureInfo("es-AR"));
                OnPropertyChanged(nameof(ListaProductos));
            }
        }
        private string _subTotal;
        public string SubTotal
        {
            get => _subTotal;
            set
            {
                _subTotal = value;
                OnPropertyChanged(nameof(SubTotal));
            }
        }
        private string _descuento;
        public string Descuento
        {
            get => _descuento;
            set
            {
                _descuento = value;
                OnPropertyChanged(nameof(Descuento));
            }
        }
        private string _total;
        public string Total
        {
            get => _total;
            set
            {
                _total = value;
                OnPropertyChanged(nameof(Total));
            }
        }
        #endregion
        #region Comandos
        public ICommand ObtenerDetallesDeUnaVenta { get; }
        public ICommand PreviewKeyDownParaCerrar { get; }
        #endregion
        #region Funciones
        private DateTime? primeraPulsacion;
        private async Task EjecutarPreviewKeyDownParaCerrar(object arg)
        {
            if (arg is not System.Windows.Input.KeyEventArgs e)
                return;
            if (e.Key != Key.Escape) return;
            DateTime ahora = DateTime.Now;
            if (primeraPulsacion == null)
            {
                primeraPulsacion = ahora;
                return;
            }
            TimeSpan diferencia = ahora - primeraPulsacion.Value;
            if (diferencia.TotalSeconds <= 1)
            {
                primeraPulsacion = null;
                CerrarVentana?.Invoke();
            }
            else
            {
                primeraPulsacion = ahora;
            }
        }
        private decimal CalcularTotal()
        {
            decimal total = 0;
            for (int i = 0; i < ListaProductos.Count; i++)
            {
                total += ListaProductos[i].SubTotal;
            }
            return total;
        }
        private async Task EjecutarObtenerDetallesDeUnaVenta(object arg)
        {
            try
            {
                var res = await _adminVentas.BuscarDetalles(_venta);
                _venta.Productos = res.venta.Productos;
                ListaProductos = new ObservableCollection<Productos>(_venta.Productos);
                Descuento = _venta.Descuento.ToString("N2", new CultureInfo("es-AR"));
                Cajero = _configuracion.NombreDeCaja;
                Total = _venta.Total.ToString("N2", new CultureInfo("es-AR"));
                Caja = _configuracion.NumeroDeCaja.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                HistorialErrores error = new HistorialErrores(ex.Message, ex.GetType().FullName, ex.StackTrace, "Kiosco2026.ModeloDeLasVistas.Hijas", "ModeloDetallesVenta", "EjecutarObtenerDetallesDeUnaVenta", _configuracion.NombreDeCaja);
                EscribirEnTXTError(error);
            }
        }
        #endregion
    }
}
