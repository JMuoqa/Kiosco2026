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

namespace Kiosco2026.Vistas.INFORMACION
{
    /// <summary>
    /// Lógica de interacción para InformacionEditarProducto.xaml
    /// </summary>
    public partial class InformacionEditarProducto : Window
    {
        public InformacionEditarProducto()
        {
            InitializeComponent();
        }

        private DateTime? primeraPulsacion;
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
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
                this.Close();
            }
            else
            {
                primeraPulsacion = ahora;
            }
        }
    }
}
