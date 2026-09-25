using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;

namespace Kiosco2026
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Mutex? mutex;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e); 
            bool nuevaInstancia;

            mutex = new Mutex(
                true,
                "MuoqaStock",
                out nuevaInstancia
            ); 
            if (!nuevaInstancia)
            {
                Shutdown();
                return;
            }
            var login = new Autenticarse();
            bool? resultado = login.ShowDialog();
            if (resultado == true)
            {
                var mainWindow = new MainWindow();
                MainWindow = mainWindow;
                mainWindow.Closed += (sender, z) =>
                {
                    Shutdown();
                };
                mainWindow.Show();
            }
            else
            {
                Shutdown();
            }
        }
    }

}
