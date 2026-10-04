using Dominio;
using Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kiosco2026.ModeloDeLasVistas.Autenticarse
{
    public class ModeloAutenticarse : ModeloDeVistaBase
    {
        private Configuracion _configuracion { get; }
        string carpetaDatos = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Muoqa",
            "MuoqaStock"
        );
        public ModeloAutenticarse() 
        {
            string ruta = carpetaDatos + "\\config.json";
            string jsonDeConfiguracion = File.ReadAllText(ruta);
            _configuracion = JsonSerializer.Deserialize<Configuracion>(jsonDeConfiguracion);
            IniciarSession = new ComandoModeloDeVista(EjecutarIniciarSession);
            IniciarKeyDown = new ComandoModeloDeVista(EjecutarIniciarKeyDown);
            _enProceso = false;
            _aut = new AdminAut();
        }

        private AdminAut _aut;
        private string _mensajeRespuesta;
        public string MensajeRespuesta
        {
            get => _mensajeRespuesta;
            set
            {
                _mensajeRespuesta = value;
                OnPropertyChanged(nameof(MensajeRespuesta));
            }
        }
        private string _nombreUsuario;
        public string NombreUsuario
        {
            get => _nombreUsuario;
            set
            {
                _nombreUsuario = value;
                OnPropertyChanged(nameof(NombreUsuario));
            }
        }
        private string _contrasena;
        public string Contrasena
        {
            get => _contrasena;
            set
            {
                _contrasena = value;
                OnPropertyChanged(nameof(Contrasena));
            }
        }
        public ICommand IniciarSession { get; }
        public ICommand IniciarKeyDown { get; }
        public event Action IniciarAplicacion;

        private async Task EjecutarIniciarKeyDown(object arg)
        {
            if (arg is KeyEventArgs e)
            {
                if (e.Key == Key.Enter)
                {
                    EjecutarIniciarSession(null);
                }
            }
        }
        private bool _enProceso;
        private async Task EjecutarIniciarSession(object arg)
        {
            try
            {
                if (!_enProceso)
                {
                    _enProceso=true;
                    UsuarioMuoqa usr = new UsuarioMuoqa
                    {
                        Usuario = NombreUsuario,
                        Contrasena = Contrasena
                    };
                    var res = await _aut.Con_IniciarSesion(usr);
                    if (!res.estado) throw new Exception(res.mensaje);
                    MensajeRespuesta = res.mensaje;
                    string codigo = res.datosUsuario;
                    bool tarea = await ActualizarCodigo(codigo);
                    if (tarea) IniciarAplicacion?.Invoke();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MensajeRespuesta = ex.Message;
                HistorialErrores error = new HistorialErrores(ex.Message, ex.GetType().FullName, ex.StackTrace, "Kiosco2026.ModeloDeLasVistas.Hijas", "ModeloHistorialVentas", "EjecutarBuscarVentas", _configuracion.NombreDeCaja);
                EscribirEnTXTError(error);
            }
            finally { _enProceso = false; }
        }
        private async Task<bool> ActualizarCodigo(string codigoNuevo)
        {
            string ruta = carpetaDatos+"\\config.json";
            //Leemos el json
            string jsonDeConfiguracion = File.ReadAllText(ruta);
            //Lo transfomamos a un objeto
            Configuracion configuracion = JsonSerializer.Deserialize<Configuracion>(jsonDeConfiguracion);
            //Modificamos las propiedades
            configuracion.CodigoSeguridad = codigoNuevo;
            Debug.WriteLine(codigoNuevo);
            //Convertimos de vuelta a formato json
            string nuevoJson = JsonSerializer.Serialize(configuracion, new JsonSerializerOptions { WriteIndented = true });
            //Sobrescribimos el archivo con las nuevas modificaciones
            File.WriteAllText(ruta, nuevoJson);
            return true;
        }
    }
}
