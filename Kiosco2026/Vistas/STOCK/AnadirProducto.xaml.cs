using Entidades;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Kiosco2026.Vistas
{
    public partial class AnadirProducto : UserControl
    {
        public AnadirProducto()
        {
            InitializeComponent(); 
            EstiloItems();
        }

        private void EstiloItems()
        {
            //Este evento se ejecuta cuando el itemcontainer cambia de estado (inicio, generando, terminado)
            //pero solo empieza a cambiar de estados cuando se abre por primera vez el dropdown
            cmb_categorias.ItemContainerGenerator.StatusChanged += (s, e) =>
            {
                if (cmb_categorias.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                {
                    //No podemos usar foreach por que los comboboxitem estan dentro de un ¿datatemplate?
                    for (int i = 0; i < cmb_categorias.Items.Count; i++)
                    {
                        //Aca accedemos al cmbitem nº'i'
                        var container = cmb_categorias.ItemContainerGenerator.ContainerFromIndex(i) as ComboBoxItem; 
                        if (container != null)
                        {
                            container.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                        }
                    }
                    cmb_categorias.SelectedIndex = 0;
                    cmb_categorias.IsDropDownOpen = false;
                }
            };
            cmb_categorias.IsDropDownOpen = true;
        }
        private void entradas_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsNumber(e.Text[0]))
                e.Handled = true;
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
    }
}
