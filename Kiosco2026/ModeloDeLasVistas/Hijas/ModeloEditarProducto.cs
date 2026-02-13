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
    public class ModeloEditarProducto : ModeloDeVistaBase
    {
        public ModeloEditarProducto() 
        {
            ActualizarAnchoActual = new ComandoModeloDeVista(EjecutarActualizarAnchoActual);
            ObtenerProductos = new ComandoModeloDeVista(EjecutarObtenerProductos);
            ObtenerProductosPorIngreso = new ComandoModeloDeVista(EjecutarObtenerProductosPorIngreso);
            _adminProductos = new AdminProductos();
        }
        #region Propiedades
        private AdminProductos _adminProductos;
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
        //Queda hacer las propiedades publicas con getters y setters
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
            get=> _eColumna_dos;
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
        #region Comandos
        public ICommand ActualizarAnchoActual { get; }
        public ICommand ObtenerProductos { get; }
        public ICommand ObtenerProductosPorIngreso { get; }
        #endregion
        #region Eventos
        private async Task EjecutarObtenerProductosPorIngreso(object obj)
        {
            Debug.WriteLine(obj);
            Productos producto = obj as Productos;
            Debug.WriteLine(producto.IDCategoria);
        }
        private async Task EjecutarObtenerProductos(object obj)
        {
            string valor = "";
            if (valor != "Buscar por nombre")
                valor = valor;
            var res = await _adminProductos.BusquedaProductos(valor);
            ListaProductos = new ObservableCollection<Productos>(res.lista);
            Debug.WriteLine(res);
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
        #endregion
    }
}
