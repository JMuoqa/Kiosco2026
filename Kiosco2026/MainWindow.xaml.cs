using FontAwesome.Sharp;
using Kiosco2026.ModeloDeLasVistas;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kiosco2026
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool bandera;
        public MainWindow()
        {
            ModeloPrincipal mainWindow = new ModeloPrincipal();
            this.DataContext = mainWindow;
            InitializeComponent();
            bandera = false;
        }
        private void MoverVentana(object sender, MouseButtonEventArgs e)
        {
            if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void NuevoEstado(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Normal || WindowState == WindowState.Maximized)
            {
                Storyboard sb = (Storyboard)FindResource("RestaurarVentana");
                sb.Begin();
            }
        }
        private void PantallaCompleta()
        {
            Window ventana = Window.GetWindow(this);
            var escala = ventana.RenderTransform as ScaleTransform;
            if (WindowState != WindowState.Maximized)
            {
                double ancho = ventana.Width;
                double alto = ventana.Height;
                escala.ScaleY = 0.8;
                escala.ScaleX = 0.8;
                ventana.Width += 150;
                ventana.Height += 150;
                Storyboard sb = (Storyboard)FindResource("MaximizarVentana");
                sb.Completed += (s, ev) => //Ek evento completed se ejecuta una vez una termine la tarea
                {
                    escala.ScaleY = 1;
                    escala.ScaleX = 1;
                    this.WindowState = WindowState.Maximized;
                    ventana.Width = ancho;
                    ventana.Height = alto;
                };
                sb.Begin(this);
            }
            else
            {
                Storyboard sb = (Storyboard)FindResource("NormalizarVentana");
                sb.Completed += (s, ev) => //Ek evento completed se ejecuta una vez una termine la tarea
                {
                    this.WindowState = WindowState.Normal;
                    Storyboard sb2 = (Storyboard)FindResource("RestaurarVentana");
                    sb2.Begin(this);
                };
                sb.Begin(this);
            }
        }
        private void Btn_PantallaCompleta(object sender, RoutedEventArgs e)
        {
            PantallaCompleta();
        }

        private void MinimizarApp(object sender, RoutedEventArgs e)
        {
            Window ventana = Window.GetWindow(this);
            ///Ahora la escala la controla las animaciones
            //Para poder acceder a la propedad "escala 'x' e 'y'" de la ventana
            //var escala = ventana.RenderTransform as ScaleTransform; 
            Storyboard sb = (Storyboard)FindResource("MinimizarVentana");
            sb.Completed += (s, ev) => //Ek evento completed se ejecuta una vez una termine la tarea
            {
                this.WindowState = WindowState.Minimized;
            };
            sb.Begin(this);
        }
        private void CerrarApp(object sender, RoutedEventArgs e)
        {
            Storyboard sb = (Storyboard)FindResource("CerrarVentana");
            sb.Completed += (s, ev) =>
            {
                Application.Current.Shutdown();
            };
            sb.Begin(this);
        }
        private void ActualizarBordesGrid(object sender, SizeChangedEventArgs e)
        {
            EscalaVentana.CenterX = this.ActualWidth / 2;
            var grid = (Grid)sender; //Es igual a "Grid grilla = sender as Grid;" pero este lanza una excepcion si "sender" no es un Grid
            if (WindowState == WindowState.Maximized)
            {
                grid.Clip = new RectangleGeometry
                {
                    RadiusX = 0,
                    RadiusY = 0,
                    Rect = new Rect(0, 0, grid.ActualWidth, grid.ActualHeight)
                };
                return;
            }
            grid.Clip = new RectangleGeometry
            {
                RadiusX = 12,
                RadiusY = 12,
                Rect = new Rect(0, 0, grid.ActualWidth, grid.ActualHeight)
            };
        }
    }
}