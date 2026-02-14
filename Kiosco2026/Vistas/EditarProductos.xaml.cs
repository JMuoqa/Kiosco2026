using Entidades;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

namespace Kiosco2026.Vistas
{
    /// <summary>
    /// Lógica de interacción para EditarProductos.xaml
    /// </summary>
    public partial class EditarProductos : UserControl
    {
        public EditarProductos()
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
                            container.Selected += ComboBoxItem_Selected_Categoria;
                        }
                    }
                    cmb_categorias.SelectedIndex = 0;
                    cmb_categorias.IsDropDownOpen = false;
                }
            };
            cmb_categorias.IsDropDownOpen = true;
        }
        private void ComboBoxItem_Selected_Categoria(object sender, RoutedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem)sender;
            Categorias cate = (Categorias)item.Content;
            string str = cate.Nombre.ToLower();
            if (str != "categoria*")
                cmb_categorias.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            else
                cmb_categorias.Foreground = new SolidColorBrush(Color.FromArgb(255, 185, 185, 185));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            // Hace lo mismo que arriba pero diferente sintaxis
            Productos pr = btn.Tag as Productos; 
            EditarLoteProducto window = new EditarLoteProducto(pr); 
            // Le dice que el padre de esta ventana es MainWindow
            window.Owner = Application.Current.MainWindow;
            window.Show();
        }
    }
}
