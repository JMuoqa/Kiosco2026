using Dominio;
using Entidades;
using Kiosco2026.Vistas.VENTAS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kiosco2026.ModeloDeLasVistas.Hijas
{
    public class ModeloHistorialVentas : ModeloDeVistaBase
    {
        private Configuracion _configuracion { get; }
        private bool _metodoEnEjecucion;
        string carpetaDatos = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Muoqa",
            "MuoqaStock"
        );
        public ModeloHistorialVentas()
        {
            _metodoEnEjecucion = false;
            string ruta = carpetaDatos + "\\config.json";
            string jsonDeConfiguracion = File.ReadAllText(ruta);
            _configuracion = JsonSerializer.Deserialize<Configuracion>(jsonDeConfiguracion);
            _adminVentas = new AdminVentas();
            BuscarVentas = new ComandoModeloDeVista(EjecutarBuscarVentas);
            VerMasInfo = new ComandoModeloDeVista(EjecutarVerMasInfo);
            ActualizarAnchoActual = new ComandoModeloDeVista(EjecutarActualizarAnchoActual);
            DateTime hoy = DateTime.Today;
            DateTime manana = hoy.AddDays(+1);
            FechaDesde = hoy;
            FechaHasta = manana;
        }
        #region Columnas privadas
        private double _eColumna_uno;
        private double _eColumna_dos;
        private double _eColumna_tres;
        private double _eColumna_cuatro;
        private double _eColumna_cinco;
        private double _eColumna_seis;
        private double _tColumna_uno;
        private double _tColumna_dos;
        private double _tColumna_tres;
        private double _tColumna_cuatro;
        private double _tColumna_cinco;
        private double _tColumna_seis;
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
        #endregion
        #region Propiedades
        private AdminVentas _adminVentas;
        private ObservableCollection<Ventas> _listaVentas;
        private string _precioTotal;
        public string PrecioTotal
        {
            get => _precioTotal;
            set
            {
                _precioTotal = value;
                OnPropertyChanged(nameof(PrecioTotal));
            }
        }
        public ObservableCollection<Ventas> ListaVentas
        {
            get => _listaVentas;
            set
            {
                _listaVentas = value;
                OnPropertyChanged(nameof(ListaVentas));
            }
        }
        private DateTime? _fechaDesde;
        public DateTime? FechaDesde
        {
            get => _fechaDesde;
            set
            {
                _fechaDesde = value;
                EjecutarBuscarVentas(null);
                OnPropertyChanged(nameof(FechaDesde));
            }
        }
        private DateTime? _fechaHasta;
        public DateTime? FechaHasta
        {
            get => _fechaHasta;
            set
            {
                _fechaHasta = value;
                EjecutarBuscarVentas(null);
                OnPropertyChanged(nameof(FechaHasta));
            }
        }
        #endregion
        #region Comandos
        public ICommand BuscarVentas { get; }
        public ICommand VerMasInfo { get; }
        public ICommand ActualizarAnchoActual { get; }
        #endregion
        #region Funciones 
        private async Task EjecutarBuscarVentas(object arg)
        {
            try
            {
                var res = await _adminVentas.BuscarVentas(FechaDesde, FechaHasta);
                List<Ventas> lista = res.ventas;
                decimal precioTotal = 0;
                for (int i = 0; i < lista.Count; i++)
                {
                    precioTotal += lista[i].Total;
                }
                PrecioTotal = precioTotal.ToString("C2", new CultureInfo("es-AR"));
                ListaVentas = new ObservableCollection<Ventas>(lista);
            }
            catch (Exception ex) 
            {
                HistorialErrores error = new HistorialErrores(ex.Message, ex.GetType().FullName, ex.StackTrace, "Kiosco2026.ModeloDeLasVistas.Hijas", "ModeloHistorialVentas", "EjecutarBuscarVentas", _configuracion.NombreDeCaja);
                EscribirEnTXTError(error);
            }
        }
        private async Task EjecutarVerMasInfo(object arg) 
        {
            try
            {
                if(arg is Ventas v)
                {

                    DetalleVenta window = new DetalleVenta(v);
                    window.Owner = Application.Current.MainWindow;
                    window.ShowDialog();
                }
            }
            catch(Exception ex)
            {

            }
        }
        #endregion
        #region Funciones graficas
        private async Task EjecutarActualizarAnchoActual(object arg)
        {
            if(arg is Grid grid)
            {
                double anchoActual = grid.ActualWidth;
                double nueve = (anchoActual * 9) / 100;
                double doce = (anchoActual * 12) / 100;
                double relleno = (anchoActual * 23.3) / 100;
                EColumna_uno = nueve;
                EColumna_dos = relleno;
                EColumna_tres = nueve;
                EColumna_cuatro = doce;
                EColumna_cinco = relleno;
                EColumna_seis = relleno;
                TColumna_uno = nueve;
                TColumna_dos = relleno;
                TColumna_tres = nueve;
                TColumna_cuatro = doce;
                TColumna_cinco = relleno;
                TColumna_seis = relleno;
            }
        }
        #endregion
    }
}
