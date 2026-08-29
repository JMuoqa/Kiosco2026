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
        #endregion
        #region Funciones 
        public async Task EjecutarMostrarConfBD (object arg)
        {
            VistaHijaActual = new ModeloConfBD();
        }
        public async Task EjecutarMostrarConfDN(object arg)
        {
            VistaHijaActual = new ModeloConfDN();
        }
        #endregion
    }
}
