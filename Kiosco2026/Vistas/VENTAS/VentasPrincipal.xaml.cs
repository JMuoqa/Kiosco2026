using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kiosco2026.Vistas.VENTAS
{
    /// <summary>
    /// Lógica de interacción para VentasPrincipal.xaml
    /// </summary>
    public partial class VentasPrincipal : UserControl
    {
        public VentasPrincipal()
        {
            InitializeComponent();
        }
        private void cmb_categorias_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((e.OriginalSource is TextBox || e.OriginalSource is Border) || e.OriginalSource is TextBlock)
            {
                ComboBox cmb = (ComboBox)sender;
                if (!cmb.IsDropDownOpen)
                    cmb.IsDropDownOpen = true;
                e.Handled = true;
            }
        }
        private void entradas_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsNumber(e.Text[0]))
                e.Handled = true;
        }
    }
}
