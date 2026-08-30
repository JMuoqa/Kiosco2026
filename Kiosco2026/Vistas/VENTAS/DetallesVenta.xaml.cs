using Entidades;
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

namespace Kiosco2026.Vistas.VENTAS
{
    /// <summary>
    /// Lógica de interacción para DetalleVenta.xaml
    /// </summary>
    public partial class DetalleVenta : Window
    {
        public DetalleVenta()
        {
            InitializeComponent();
        }
        public DetalleVenta(Ventas v)
        {
            InitializeComponent();
            this.DataContext = new ModeloDetallesVenta(v);
        }
    }
}
