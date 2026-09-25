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

namespace WPFApp_Muestra.ControlesPer
{
    /// <summary>
    /// Lógica de interacción para CuadroContrasenaEnlazable.xaml
    /// </summary>
    public partial class CuadroContrasenaEnlazable : UserControl
    {
        public static readonly DependencyProperty PropiedadContrasena =
            DependencyProperty.Register(
                "Contrasena",
                typeof(string),
                typeof(CuadroContrasenaEnlazable),
                new PropertyMetadata(string.Empty, OnContrasenaCambiada));

        private static void OnContrasenaCambiada(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CuadroContrasenaEnlazable control)
            {
                // Actualiza el PasswordBox solo si es diferente
                var nueva = e.NewValue as string ?? string.Empty;
                if (control.entrada_contraseña_elazable.Password != nueva)
                    control.entrada_contraseña_elazable.Password = nueva;
            }
        }

        public string Contrasena
        {
            get => (string)GetValue(PropiedadContrasena);
            set => SetValue(PropiedadContrasena, value);
        }
        public CuadroContrasenaEnlazable()
        {
            InitializeComponent();
            entrada_contraseña_elazable.PasswordChanged += CambioLaContrasena;
        }
        private void CambioLaContrasena(object sender, RoutedEventArgs e)
        {
            Contrasena = entrada_contraseña_elazable.Password;
        }
    }
}
