using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kiosco2026.ModeloDeLasVistas.Hijas
{
    public class ModeloConfiguracion : ModeloDeVistaBase
    {
        public ModeloConfiguracion()
        {
            VistaHijaActual = new ModeloConfBD();
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
    }
}
