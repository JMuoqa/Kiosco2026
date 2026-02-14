using Entidades;
using Kiosco2026.ModeloDeLasVistas.Hijas;
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
using System.Windows.Shapes;

namespace Kiosco2026.Vistas
{
    /// <summary>
    /// Lógica de interacción para EditarLoteProducto.xaml
    /// </summary>
    public partial class EditarLoteProducto : Window
    {
        private Productos _pr;
        public EditarLoteProducto()
        {
            InitializeComponent();
            _pr = new Productos();
        }
        public EditarLoteProducto(Productos pr)
        {
            InitializeComponent();
            _pr = pr;
            this.DataContext = new ModeloEditarLoteProducto(_pr);
        }
    }
}
