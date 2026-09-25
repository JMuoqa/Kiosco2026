using Kiosco2026.ModeloDeLasVistas.Autenticarse;
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

namespace Kiosco2026
{
    /// <summary>
    /// Lógica de interacción para Autenticarse.xaml
    /// </summary>
    public partial class Autenticarse : Window
    {
        public Autenticarse()
        {
            InitializeComponent();
            var _viewModel = new ModeloAutenticarse();
            this.DataContext = _viewModel;
            _viewModel.IniciarAplicacion += IniciarAplicacion;
        }
        private void IniciarAplicacion()
        {
            DialogResult = true;
        }
        private void VistaAut_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void boton_minimizar_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void boton_cerrar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
