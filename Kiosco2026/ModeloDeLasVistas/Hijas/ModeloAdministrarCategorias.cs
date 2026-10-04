using Dominio;
using Entidades;
using Kiosco2026.Clases.Colores;
using Kiosco2026.Vistas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Kiosco2026.ModeloDeLasVistas.Hijas
{
    public class ModeloAdministrarCategorias : ModeloDeVistaBase
    {
        private Configuracion _configuracion { get; }
        private bool _metodoEnEjecucion;
        string carpetaDatos = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Muoqa",
            "MuoqaStock"
        );
        public ModeloAdministrarCategorias()
        {
            _metodoEnEjecucion = false;
            string ruta = carpetaDatos+"\\config.json";
            string jsonDeConfiguracion = File.ReadAllText(ruta);
            _configuracion = JsonSerializer.Deserialize<Configuracion>(jsonDeConfiguracion);
            ActualizarAnchoActual = new ComandoModeloDeVista(EjecutarActualizarAnchoActual);
            LlenarDatos = new ComandoModeloDeVista(EjecutarLlenarDatos);
            BuscarCategorias = new ComandoModeloDeVista(EjecutarBuscarCategorias);
            EditarCategoria = new ComandoModeloDeVista(EjecutarEditarCategoria);
            GotFocus = new ComandoModeloDeVista(EjecutarGotFocus);
            Cancelar = new ComandoModeloDeVista(EjecutarCancelar);
            LostFocus = new ComandoModeloDeVista(EjecutarLostFocus);
            KeyDownBuscar = new ComandoModeloDeVista(EjecutarKeyDownBuscar);
            RestablecerTodo = new ComandoModeloDeVista(EjecutarRestablecerTodo);
            AbrirMAC = new ComandoModeloDeVista(EjecutarAbrirMAC);
            SelecionarTextBox = new ComandoModeloDeVista(EjecutarSelecionarTextBox);
            _adminProductos = new AdminProductos();
            _coloresEntradas = new ColoresProductos();
            SoloLectura = true;
            Busqueda = "Buscar por nombre";
            Edicion = "Nombre de la categoría";
            CategoriaSeleccionada = "Ninguna";
            _colores = new ColoresTexto();
            categoriaAEditar = null;
            ColorMensajeEdicion = _colores.Normal;
            tiempo = new System.Timers.Timer();
            tiempo.Elapsed += ResetearMensaje;
            tiempo.Enabled = false;
            tiempo.AutoReset = true;
        }
        #region Propiedades
        private System.Timers.Timer tiempo;
        private string _edicion;
        private string _categoriaSeleccionada;
        private Categorias categoriaAEditar;
        private string _colorMensajeEditar;
        private string _mensajeEditar;
        private ColoresTexto _colores;
        private ColoresProductos _coloresEntradas;
        private AdminProductos _adminProductos;
        private ObservableCollection<Categorias> _listaCategorias;
        private string _busqueda;
        private double _eColumna_uno;
        private double _eColumna_dos;
        private double _eColumna_tres;
        private double _tColumna_uno;
        private double _tColumna_dos;
        private double _tColumna_tres;
        private bool _visualizacion;
        private bool _soloLectura;
        private string _colorBusqueda;
        public ColoresProductos ColoresProductos
        {
            get => _coloresEntradas;
            set
            {
                _coloresEntradas = value;
                OnPropertyChanged(nameof(ColoresProductos));
            }
        }
        public string ColorBusqueda
        {
            get => _colorBusqueda;
            set
            {
                _colorBusqueda = value;
                OnPropertyChanged(nameof(ColorBusqueda));
            }
        }
        public string Busqueda
        {
            get => _busqueda;
            set
            {
                _busqueda = value;
                OnPropertyChanged(nameof(Busqueda));
            }
        }
        public string MensajeEdicion
        {
            get => _mensajeEditar;
            set
            {
                _mensajeEditar = value;
                OnPropertyChanged(nameof(MensajeEdicion));
            }
        }
        public string ColorMensajeEdicion
        {
            get => _colorMensajeEditar;
            set
            {
                _colorMensajeEditar = value;
                OnPropertyChanged(nameof(ColorMensajeEdicion));
            }
        }
        public bool SoloLectura
        {
            get => _soloLectura;
            set
            {
                _soloLectura = value;
                OnPropertyChanged(nameof(SoloLectura));
            }
        }
        public ObservableCollection<Categorias> ListaCategorias
        {
            get => _listaCategorias;
            set
            {
                _listaCategorias = value;
                OnPropertyChanged(nameof(ListaCategorias));
            }
        }
        public bool Visualizacion
        {
            get => _visualizacion;
            set
            {
                _visualizacion = value;
                OnPropertyChanged(nameof(Visualizacion));
            }
        }
        public string CategoriaSeleccionada
        {
            get => _categoriaSeleccionada;
            set
            {
                _categoriaSeleccionada = value;
                OnPropertyChanged(nameof(CategoriaSeleccionada));
            }
        }
        public string Edicion
        {
            get => _edicion;
            set
            {
                _edicion = value;
                OnPropertyChanged(nameof(Edicion));
            }
        }
        public double EColumna_uno
        {
            get => _eColumna_uno;
            set
            {
                _eColumna_uno = value;
                OnPropertyChanged(nameof(EColumna_uno));
            }
        }
        public double EColumna_dos
        {
            get => _eColumna_dos;
            set
            {
                _eColumna_dos = value;
                OnPropertyChanged(nameof(EColumna_dos));
            }
        }
        public double EColumna_tres
        {
            get => _eColumna_tres;
            set
            {
                _eColumna_tres = value;
                OnPropertyChanged(nameof(EColumna_tres));
            }
        }
        public double TColumna_uno
        {
            get => _tColumna_uno;
            set
            {
                _tColumna_uno = value;
                OnPropertyChanged(nameof(TColumna_uno));
            }
        }
        public double TColumna_dos
        {
            get => _tColumna_dos;
            set
            {
                _tColumna_dos = value;
                OnPropertyChanged(nameof(TColumna_dos));
            }
        }
        public double TColumna_tres
        {
            get => _tColumna_tres;
            set
            {
                _tColumna_tres = value;
                OnPropertyChanged(nameof(TColumna_tres));
            }
        }
        #endregion
        #region Comandos
        public ICommand ActualizarAnchoActual { get; }
        public ICommand LlenarDatos { get; }
        public ICommand BuscarCategorias { get; }
        public ICommand EditarCategoria { get; }
        public ICommand GotFocus { get; }
        public ICommand LostFocus { get; }
        public ICommand Cancelar { get; }
        public ICommand KeyDownBuscar { get; }
        public ICommand RestablecerTodo { get; }
        public ICommand AbrirMAC { get; }
        public ICommand SelecionarTextBox { get; }
        #endregion
        private async Task EjecutarSelecionarTextBox(object arg)
        {
            if (arg is TextBox txt)
            {
                await Task.Delay(100);
                txt.Focus();
                Keyboard.Focus(txt);
            }
        }
        private async Task EjecutarAbrirMAC(object arg)
        {
            AgregarCategoria window = new AgregarCategoria();
            window.Owner = Application.Current.MainWindow; //Aca le decimo cual es el padre de "window"
            window.Closed += (sender, e) =>
            {
                EjecutarRestablecerTodo(null);
            };
            window.ShowDialog();
        }
        private async Task EjecutarRestablecerTodo(object arg)
        {
            EjecutarCancelar(null);
            Busqueda = "";
            EjecutarBuscarCategorias(null);
        }
        private async Task EjecutarCancelar(object arg)
        {
            SoloLectura = true;
            Visualizacion = default;
            Edicion = "Nombre de la categoría";
            CategoriaSeleccionada = "Ninguna";
            categoriaAEditar = null;
            ColoresProductos.BusuqedaUno = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
        }
        private async Task EjecutarBuscarCategorias(object arg)
        {
            try
            {
                if (!_metodoEnEjecucion) 
                {
                    _metodoEnEjecucion = true;
                    string valor = "";
                    if (Busqueda != "Buscar por nombre")
                        valor = Busqueda;
                    var res = await _adminProductos.BusquedaCategorias(valor);
                    ListaCategorias = new ObservableCollection<Categorias>(res.lista);
                    _metodoEnEjecucion = false;
                }
            }
            catch (Exception ex)
            {
                ColorMensajeEdicion = _colores.ColorError;
                MensajeEdicion = ex.Message;
                HistorialErrores error = new HistorialErrores(ex.Message, ex.GetType().FullName, ex.StackTrace, "Kiosco2026.ModeloDeLasVistas.Hijas", "ModeloAdministrarCategorias", "EjecutarBuscarCategorias", _configuracion.NombreDeCaja);
                EscribirEnTXTError(error);
                _metodoEnEjecucion = false;
            }
            finally
            {
                tiempo.Interval = 10000;
                tiempo.Enabled = true;
            }
        }
        private async Task EjecutarKeyDownBuscar(object arg)
        {
            if (arg is KeyEventArgs e)
            {
                if (e.Key == Key.Enter)
                {
                    EjecutarBuscarCategorias(null);
                }
            }
        }
        private async Task EjecutarEditarCategoria(object arg)
        {
            try
            {
                if (!_metodoEnEjecucion)
                {
                    _metodoEnEjecucion = true;
                    if (categoriaAEditar == null)
                        throw new Exception("No hay ninguna categoria seleccionada");
                    int idCategoria = categoriaAEditar.ID;
                    Categorias cate = new Categorias
                    {
                        Nombre = Edicion.ToUpper(),
                        ID = idCategoria,
                        Mostrar = Visualizacion
                    };
                    var respuesta = await _adminProductos.EditarCategoria(cate);
                    _metodoEnEjecucion = false;
                    if (respuesta.estado)
                    {
                        Busqueda = "Buscar por nombre";
                        Edicion = "Nombre de la categoría";
                        CategoriaSeleccionada = "Ninguna";
                        categoriaAEditar = null;
                        ColoresProductos.BusuqedaUno = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A"));
                        ColorMensajeEdicion = _colores.ColorExito;
                        EjecutarBuscarCategorias(null);
                    }
                    else
                    {
                        ColorMensajeEdicion = _colores.ColorError;
                    }
                    MensajeEdicion = respuesta.mensaje;
                }
            }
            catch (Exception ex) 
            {
                ColorMensajeEdicion = _colores.ColorError;
                MensajeEdicion = ex.Message;
                HistorialErrores error = new HistorialErrores(ex.Message, ex.GetType().FullName, ex.StackTrace, "Kiosco2026.ModeloDeLasVistas.Hijas", "ModeloAdministrarCategorias", "EjecutarEditarCategoria", _configuracion.NombreDeCaja);
                EscribirEnTXTError(error);
                _metodoEnEjecucion = false;
            }
            finally
            {
                tiempo.Interval = 10000;
                tiempo.Enabled = true;
            }
        }
        private async Task EjecutarLlenarDatos(object arg)
        {
            Categorias categoria = (Categorias)arg;
            Edicion = categoria.Nombre;
            SoloLectura = false;
            Visualizacion = categoria.Mostrar;
            ColoresProductos.BusuqedaUno = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020"));
            categoriaAEditar = new Categorias
            {
                ID = categoria.ID,
            };
            CategoriaSeleccionada = $"{categoria.Nombre} (ID: {categoria.ID})";
        }
        private async Task EjecutarActualizarAnchoActual(object arg)
        {
            if(arg is Grid grid)
            {
                double anchoActual = grid.ActualWidth;
                double quince = (anchoActual * 15) / 100;
                double treinta = (anchoActual * 25) / 100;
                double setenta = (anchoActual * 60) / 100;
                EColumna_uno = quince;
                EColumna_dos = treinta;
                EColumna_tres = setenta;
                TColumna_uno = quince;
                TColumna_dos = treinta;
                TColumna_tres = setenta;
            }
        }
        private void ResetearMensaje(object source, ElapsedEventArgs e)
        {
            tiempo.Enabled = false;
            MensajeEdicion = "";
        }

        #region Grafica

        private async Task EjecutarGotFocus(object arg)
        {
            TextBox entrada = (TextBox)arg;
            if (RevisarEntradas(entrada))
            {
                switch (entrada.Name)
                {
                    case "entrada_busqueda": Busqueda = string.Empty; ColoresProductos.Busuqeda = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020")); break;
                    case "entrada_edicion": Edicion = string.Empty; ColoresProductos.BusuqedaUno= new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020")); break;
                }
            }
        }
        private async Task EjecutarLostFocus(object arg)
        {
            string nombre = (string)arg;
            switch (nombre)
            {
                case "entrada_busqueda": if (string.IsNullOrWhiteSpace(Busqueda) || Busqueda.Trim() == "Buscar por nombre") { Busqueda = "Buscar por nombre"; ColoresProductos.Busuqeda = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A")); } break;
                case "entrada_edicion": if (string.IsNullOrWhiteSpace(Edicion) || Edicion.Trim() == "Nombre de la categoría") { Edicion = "Nombre de la categoría"; ColoresProductos.BusuqedaUno = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A9A9A")); } break;
            }
        }
        private bool RevisarEntradas(TextBox entrada)
        {
            string[] valores = ["Nombre de la categoría", "Buscar por nombre"];
            foreach (string valor in valores)
            {
                if (valor == entrada.Text)
                    return true;
            }
            return false;

        }
        #endregion
    }
}
