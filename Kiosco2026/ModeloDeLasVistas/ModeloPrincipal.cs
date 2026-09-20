using Dominio;
using Entidades;
using Kiosco2026.ModeloDeLasVistas.Hijas;
using Kiosco2026.Vistas.CONFIGURACION;
using Kiosco2026.Vistas.INFORMACION;
using Kiosco2026.Vistas.VENTAS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Kiosco2026.ModeloDeLasVistas
{
    public class ModeloPrincipal : ModeloDeVistaBase
    {
        public ModeloPrincipal()
        {
            MostrarVistaAdicionProducto = new ComandoModeloDeVista(EjecutarMostrarVistaAdicionProducto);
            MostrarAdministrarCategorias = new ComandoModeloDeVista(EjecutarMostrarAdministrarCategorias);
            MostrarEdicionProducto = new ComandoModeloDeVista(EjecutarMostrarEdicionProducto);
            IniciarSession = new ComandoModeloDeVista(EjecutarIniciarSession);
            MostrarVentaPrincipal = new ComandoModeloDeVista(EjecutarMostrarVentaPrincipal);
            AbrirConfiguracion = new ComandoModeloDeVista(EjecutarAbrirConfiguracion);
            MostrarHistorialVentas = new ComandoModeloDeVista(EjecutarMostrarHistorialVentas);
            AbrirVentanaInfo = new ComandoModeloDeVista(EjecutarAbrirVentanaInfo);
            _aut = new AdminAut();
            EjecutarIniciarSession(null);
        }
        private AdminAut _aut;
        private ModeloDeVistaBase _vistaHijaActual;
        private Window _ventanaInfo;
        private string _nombreVentana;
        public string NombreVentana
        {
            get => _nombreVentana;
            set
            {
                _nombreVentana = value;
                OnPropertyChanged(nameof(NombreVentana));
            }
        }
        public ModeloDeVistaBase VistaHijaActual
        {
            get => _vistaHijaActual;
            set
            {
                _vistaHijaActual = value;
                OnPropertyChanged(nameof(VistaHijaActual));
            }
        }
        public ICommand MostrarVistaAdicionProducto { get; }
        public ICommand MostrarEdicionProducto { get; }
        public ICommand MostrarAdministrarCategorias { get; }
        public ICommand AbrirConfiguracion { get; }
        public ICommand IniciarSession { get; }
        public ICommand MostrarVentaPrincipal { get; }
        public ICommand MostrarHistorialVentas { get; }
        public ICommand AbrirVentanaInfo { get; }
        private async Task EjecutarAbrirVentanaInfo(object arg)
        {
            if (_ventanaInfo != null)
            {
                _ventanaInfo.Activate();
                return;
            }
            if (VistaHijaActual is ModeloVentasPrincipales)
            {
                PrepararVentanaInfo(new InformacionVentasPrincipal());
            }
            else if (VistaHijaActual is ModeloAnadirProducto)
            {
                PrepararVentanaInfo(new InformacionAnadirProducto());
            }
            else if (VistaHijaActual is ModeloAdministrarCategorias)
            {
                PrepararVentanaInfo(new InformacionAdministrarCategorias());
            }
            else if (VistaHijaActual is ModeloEditarProducto)
            {
                PrepararVentanaInfo(new InformacionEditarProducto());
            }
            else
            {
                return;
            }
            _ventanaInfo.Show();
        }
        private void PrepararVentanaInfo(Window ventana)
        {
            _ventanaInfo?.Close();
            _ventanaInfo = ventana;
            _ventanaInfo.Owner = Application.Current.MainWindow;
            _ventanaInfo.Closed += (s, e) =>
            {
                _ventanaInfo = null;
            };
        }
        private async Task EjecutarMostrarHistorialVentas(object arg)
        {
            VistaHijaActual = new ModeloHistorialVentas();
            NombreVentana = "Ventas | Busqueda";
        }
        private async Task EjecutarAbrirConfiguracion(object arg)
        {
            MenuConfiguracion window = new MenuConfiguracion();
            window.Owner = Application.Current.MainWindow;
            window.ShowDialog();
        }
        private async Task EjecutarMostrarVentaPrincipal(object arg)
        {
            //El nombre del modelo esta mal "redactado" seria la ventana principal de la sección ventas
            VistaHijaActual = new ModeloVentasPrincipales();
            NombreVentana = "Ventas | Principal";
        }
        private async Task EjecutarMostrarVistaAdicionProducto(object arg)
        {
            VistaHijaActual = new ModeloAnadirProducto();
            NombreVentana = "Productos | Añadir producto";
        }
        private async Task EjecutarMostrarEdicionProducto(object arg)
        {
            VistaHijaActual = new ModeloEditarProducto();
            NombreVentana = "Productos | Editar producto";
        }
        private async Task EjecutarMostrarAdministrarCategorias(object arg)
        {
            VistaHijaActual = new ModeloAdministrarCategorias();
            NombreVentana = "Productos | Administrar categorais";
        }
        private async Task EjecutarIniciarSession(object arg)
        {
            /*
            UsuarioMuoqa usr = new UsuarioMuoqa
            {
                Usuario = "Muoqa",
                Contrasena = "MuoqaMR1986" 
            };
            var res = await _aut.Con_IniciarSesion(usr);
            string codigo = res.datosUsuario;
            bool tarea = await ActualizarCodigo(codigo);*/
            if (true)
                EjecutarMostrarVentaPrincipal(null);
        }
        private async Task<bool> ActualizarCodigo(string codigoNuevo)
        {
            string ruta = "config.json";
            //Leemos el json
            string jsonDeConfiguracion = File.ReadAllText(ruta);
            //Lo transfomamos a un objeto
            Configuracion configuracion = JsonSerializer.Deserialize<Configuracion>(jsonDeConfiguracion);
            //Modificamos las propiedades
            configuracion.CodigoSeguridad = codigoNuevo; 
            //Convertimos de vuelta a formato json
            string nuevoJson = JsonSerializer.Serialize(configuracion, new JsonSerializerOptions { WriteIndented = true });
            //Sobrescribimos el archivo con las nuevas modificaciones
            File.WriteAllText(ruta, nuevoJson);
            return true;
        }
    }
}
