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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kiosco2026.Vistas
{
    /// <summary>
    /// Lógica de interacción para AdministrarCategorias.xaml
    /// </summary>
    public partial class AdministrarCategorias : UserControl
    {
        public AdministrarCategorias()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox entrada = (TextBox)sender;
            entrada.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            if (RevisarEntradas(entrada))
                entrada.Text = string.Empty;
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox entrada = (TextBox)sender;
            string nombre = entrada.Name;
            switch (nombre)
            {
                case "entrada_categoria_carga": if (string.IsNullOrWhiteSpace(entrada.Text) || entrada.Text.Trim() == "Nombre de la categoria") { entrada.Text = "Nombre de la categoria"; entrada.Foreground = new SolidColorBrush(Color.FromArgb(255, 185, 185, 185)); } break;
                case "entrada_busqueda": if (string.IsNullOrWhiteSpace(entrada.Text) || entrada.Text.Trim() == "Buscar por nombre") { entrada.Text = "Buscar por nombre"; entrada.Foreground = new SolidColorBrush(Color.FromArgb(255, 185, 185, 185)); } break;
                case "entrada_edicion": if (string.IsNullOrWhiteSpace(entrada.Text) || entrada.Text.Trim() == "Editar nombre") { entrada.Text = "Editar nombre"; entrada.Foreground = new SolidColorBrush(Color.FromArgb(255, 185, 185, 185)); } break;
            }
            entrada.Text.Trim();
        }
        private bool RevisarEntradas(TextBox entrada)
        {
            string[] valores = ["Nombre de la categoria", "Buscar por nombre", "Editar nombre"];
            foreach (string valor in valores)
            {
                if (valor == entrada.Text)
                    return true;
            }
            return false;

        }
    }
}
