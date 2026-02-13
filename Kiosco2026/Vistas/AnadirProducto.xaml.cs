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
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox entrada = (TextBox)sender;
            entrada.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            if (RevisarEntradas(entrada))
                entrada.Text = string.Empty;
        }
        private bool RevisarEntradas(TextBox entrada)
        {
            string[] valores = ["Nombre*", "Código*", "Precio de venta*", "Costo de compra*", "Cantidad*", "Proveedor"];
            foreach (string valor in valores)
            {
                if (valor == entrada.Text)
                    return true;
            }
            return false;

        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox entrada = (TextBox)sender;
            string nombre = entrada.Name;
            switch (nombre)
            {
                case "entrada_nombre": if (string.IsNullOrWhiteSpace(entrada.Text) || entrada.Text.Trim() == "Nombre*") { entrada.Text = "Nombre*"; entrada.Foreground = new SolidColorBrush(Color.FromArgb(255, 185, 185, 185)); } break;
                case "entrada_codigo": if (string.IsNullOrWhiteSpace(entrada.Text) || entrada.Text.Trim() == "Código*") { entrada.Text = "Código*"; entrada.Foreground = new SolidColorBrush(Color.FromArgb(255, 185, 185, 185)); } break;
                case "entrada_venta": if (string.IsNullOrWhiteSpace(entrada.Text) || entrada.Text.Trim() == "Precio de venta*") { entrada.Text = "Precio de venta*"; entrada.Foreground = new SolidColorBrush(Color.FromArgb(255, 185, 185, 185)); } break;
                case "entrada_compra": if (string.IsNullOrWhiteSpace(entrada.Text) || entrada.Text.Trim() == "Costo de compra*") { entrada.Text = "Costo de compra*"; entrada.Foreground = new SolidColorBrush(Color.FromArgb(255, 185, 185, 185)); } break;
                case "entrada_cantidad": if (string.IsNullOrWhiteSpace(entrada.Text) || entrada.Text.Trim() == "Cantidad*") { entrada.Text = "Cantidad*"; entrada.Foreground = new SolidColorBrush(Color.FromArgb(255, 185, 185, 185)); } break;
                case "entrada_proveedor": if (string.IsNullOrWhiteSpace(entrada.Text) || entrada.Text.Trim() == "Proveedor") { entrada.Text = "Proveedor"; entrada.Foreground = new SolidColorBrush(Color.FromArgb(255, 185, 185, 185)); } break;
            }
            entrada.Text.Trim();
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
        private void entradas_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsNumber(e.Text[0]))
                e.Handled = true;
        }
        private void cmb_categorias_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            cmb.IsDropDownOpen = true;
        }
    }
}
