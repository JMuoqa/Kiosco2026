using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kiosco2026.ModeloDeLasVistas.Hijas
{
    public class ModeloConfiguracion : ModeloDeVistaBase
    {
        public ModeloConfiguracion()
        {
            VistaHijaActual = new ModeloConfBD();
            MostrarConfBD = new ComandoModeloDeVista(EjecutarMostrarConfBD);
            MostrarConfDN = new ComandoModeloDeVista(EjecutarMostrarConfDN);
            MostrarConfImpresora = new ComandoModeloDeVista(EjecutarMostrarConfImpresora);
            PreviewKeyDownParaCerrar= new ComandoModeloDeVista(EjecutarPreviewKeyDownParaCerrar);
        }
        private ModeloDeVistaBase _vistaHijaActual;
        public ModeloDeVistaBase VistaHijaActual
        {
            get => _vistaHijaActual;
            set
            {
                _vistaHijaActual = value;
                OnPropertyChanged(nameof(VistaHijaActual));
            }
        }
        #region Comandos
        public ICommand MostrarConfBD { get; }
        public ICommand MostrarConfDN { get; }
        public ICommand MostrarConfImpresora { get; }
        public ICommand PreviewKeyDownParaCerrar { get; }
        #endregion
        #region Funciones 
        public event Action CerrarVentana;
        private DateTime? primeraPulsacion; 
        private async Task EjecutarPreviewKeyDownParaCerrar(object arg)
        {
            if (arg is not KeyEventArgs e)
                return;
            if (e.Key != Key.Escape)
                return;
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
        public async Task EjecutarMostrarConfBD (object arg)
        {
            VistaHijaActual = new ModeloConfBD();
        }
        public async Task EjecutarMostrarConfDN(object arg)
        {
            VistaHijaActual = new ModeloConfDN();
        }
        public async Task EjecutarMostrarConfImpresora(object arg)
        {
            VistaHijaActual = new ModeloConfImpresora();
        }
        #endregion
    }
}
