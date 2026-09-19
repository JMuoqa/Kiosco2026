using Dominio;
using Entidades;
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
    public class ModeloAgregarAlCarrito : ModeloDeVistaBase
    {
        public ModeloAgregarAlCarrito(List<Productos> listaProductos)
        {
            PreviewKeyDownParaCerrar = new ComandoModeloDeVista(EjecutarPreviewKeyDownParaCerrar);
            ActualizarAnchoActual = new ComandoModeloDeVista(EjecutarActualizarAnchoActual);
            SeleccionarProducto = new ComandoModeloDeVista(EjecutarSeleccionarProducto);
            _adminProductos = new AdminProductos();
            ListaProductos = new ObservableCollection<Productos>(listaProductos);
        }
        #region Columnas privadas
        private double _eColumna_uno;
        private double _eColumna_dos;
        private double _eColumna_tres;
        private double _eColumna_cuatro;
        private double _eColumna_cinco;
        private double _tColumna_uno;
        private double _tColumna_dos;
        private double _tColumna_tres;
        private double _tColumna_cuatro;
        private double _tColumna_cinco;
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
        #endregion
        #region Propiedades
        public Productos _productoSeleccionado;
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
        public event Action CerrarVentana;
        private AdminProductos _adminProductos;
        #endregion
        #region Comandos
        public ICommand PreviewKeyDownParaCerrar { get; }
        public ICommand ActualizarAnchoActual { get; }
        public ICommand SeleccionarProducto { get; }
        #endregion
        #region Funciones
        private async Task EjecutarSeleccionarProducto(object arg)
        {
            Debug.WriteLine(arg);
            Productos productoSeleccionado = (Productos)arg;
            _productoSeleccionado = productoSeleccionado;
            CerrarVentana?.Invoke();
        }
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
        private async Task EjecutarActualizarAnchoActual(object arg)
        {
            if (arg is Grid grid)
            {
                double anchoActual = grid.ActualWidth;
                double ancho1 = (anchoActual * 40) / 100;
                double ancho2 = (anchoActual * 10) / 100;
                EColumna_uno = ancho1;
                EColumna_dos = ancho1;
                EColumna_tres = ancho2;
                EColumna_cuatro = ancho2;
                TColumna_uno = ancho1;
                TColumna_dos = ancho1;
                TColumna_tres = ancho2;
                TColumna_cuatro = ancho2;
            }
        }
        #endregion
    }
}
