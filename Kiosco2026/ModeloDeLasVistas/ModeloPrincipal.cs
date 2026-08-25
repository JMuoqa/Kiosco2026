using Dominio;
using Entidades;
using Kiosco2026.ModeloDeLasVistas.Hijas;
using Kiosco2026.Vistas.CONFIGURACION;
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
            _aut = new AdminAut();
            EjecutarIniciarSession(null);
        }
        private AdminAut _aut;
        private ModeloDeVistaBase _vistaHijaActual;
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
