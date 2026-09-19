using Kiosco2026.ModeloDeLasVistas.Hijas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kiosco2026.Vistas
{
    /// <summary>
    /// Lógica de interacción para AgregarCategoria.xaml
    /// </summary>
    public partial class AgregarCategoria : Window
    {
        public AgregarCategoria()
        {
            InitializeComponent();
            var _viewModel = new MAC();
            this.DataContext = _viewModel;
            _viewModel.CerrarVentana += CerrarVentaana;
        }
        private void CerrarVentaana()
        {
            this.Close();
        }
    }
}
