using Dominio;
using Entidades;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kiosco2026.ModeloDeLasVistas.Hijas
{
    public class ModeloAnadirProducto : ModeloDeVistaBase
    {
        public ModeloAnadirProducto() 
        {
            ObtenerCategoriasGenerales = new ComandoModeloDeVista(EjecutarObtenerCategoriasGenerales);
            CargarProducto = new ComandoModeloDeVista(EjecutarCargarProducto);
            ValoresPredeterminados = new ComandoModeloDeVista(EjecutarValoresPredeterminados);
            _colores = new ColoresTexto();
            _adminProductos = new AdminProductos();
            ColorMensaje = _colores.Normal;
        }
        #region Propiedades
        private string _nombre;
        private string _codigo;
        private Categorias _categoria;
        private ColoresTexto _colores;
        private string _colorMensaje;
        private string _venta;
        private string _compra;
        private string _cantidad;
        private string _mensaje;
        private string _proveedor;
        private DateTime _ingreso;
        private DateTime _vencimiento;
        private AdminProductos _adminProductos;
        private ObservableCollection<Categorias> _listaCategorias;
        public string Nombre
        {
            get => _nombre;
            set
            {
                _nombre = value;
                OnPropertyChanged(nameof(Nombre));
            }
        }
        public string ColorMensaje
        {
            get => _colorMensaje;
            set
            {
                _colorMensaje = value;
                OnPropertyChanged(nameof(ColorMensaje));
            }
        }
        public string Codigo
        {
            get => _codigo;
            set
            {
                _codigo = value;
                OnPropertyChanged(nameof(Codigo));
            }
        }
        public string Mensaje
        {
            get => _mensaje;
            set
            {
                _mensaje = value;
                OnPropertyChanged(nameof(Mensaje));
            }
        }
        public Categorias Categoria
        {
            get => _categoria;
            set
            {
                _categoria = value;
                OnPropertyChanged(nameof(Categoria));
            }
        }
        public string Venta
        {
            get => _venta;
            set
            {
                _venta = value;
                OnPropertyChanged(nameof(Venta));
            }
        }
        public string Compra
        {
            get => _compra;
            set
            {
                _compra = value;
                OnPropertyChanged(nameof(Compra));
            }
        }
        public string Cantidad
        {
            get => _cantidad;
            set
            {
                _cantidad = value;
                OnPropertyChanged(nameof(Cantidad));
            }
        }
        public string Proveedor
        {
            get => _proveedor;
            set
            {
                _proveedor = value;
                OnPropertyChanged(nameof(Proveedor));
            }
        }
        public DateTime Ingreso
        {
            get => _ingreso;
            set
            {
                _ingreso = value;
                OnPropertyChanged(nameof(Ingreso));
            }
        }
        public DateTime Vencimiento
        {
            get => _vencimiento;
            set
            {
                _vencimiento = value;
                OnPropertyChanged(nameof(Vencimiento));
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
        public ICommand ObtenerCategoriasGenerales { get; }
        public ICommand CargarProducto { get; }
        public ICommand ValoresPredeterminados { get; }
        public ICommand EditarCategoria { get; }
        #endregion 
        private async Task EjecutarObtenerCategoriasGenerales(object arg)
        {
            var respuesta = await _adminProductos.ObtenerCategoriasGenerales();
            var respuestaDos = await _adminProductos.ObtenerCategoriasCliente();
            List<Categorias> listaGeneral = respuesta.lista;
            List<Categorias> listaCliente = respuestaDos.lista;
            List<Categorias> listaCompleta = new List<Categorias>();
            for (int i = 0; i < listaGeneral.Count; i++) 
            {
                listaCompleta.Add(listaGeneral[i]);
            }
            for (int i = 0; i < listaCliente.Count; i++)
            {
                listaCompleta.Add(listaCliente[i]);
            }
            ListaCategorias = new ObservableCollection<Categorias>(listaCompleta);
        }
        private async Task EjecutarCargarProducto(object arg)
        {
            try
            {
                ProductosDto producto = new ProductosDto
                {
                    Nombre = this.Nombre,
                    Categoria = this.Categoria.Nombre, //Categoria deberia ser un numero
                    Codigo = this.Codigo,
                    Venta = this.Venta,
                    Compra = this.Compra,
                    Cantidad = this.Cantidad,
                    Proveedor = this.Proveedor,
                    Ingreso = this.Ingreso,
                    Vencimiento = this.Vencimiento
                };
                var respuesta = await _adminProductos.CargarProducto(producto);
                if (respuesta.estado)
                    ColorMensaje = _colores.ColorExito;
                else
                    ColorMensaje = _colores.ColorError;
                Mensaje = respuesta.mensaje;
            }
            catch (Exception ex) 
            {
                ColorMensaje = _colores.ColorError;
                Mensaje = ex.Message;
            }
        }
        private async Task EjecutarValoresPredeterminados(object arg)
        {
            Nombre = "Nombre*";
            Codigo = "Código*";
            Venta = "Precio de venta*";
            Compra = "Costo de compra*";
            Cantidad = "Cantidad*";
            Proveedor = "Proveedor";
            DateTime hoy = DateTime.Today;
            DateTime AnoDespues = hoy.AddYears(1);
            Ingreso = hoy;
            Vencimiento = AnoDespues;
            ColorMensaje = _colores.Normal;
            EjecutarObtenerCategoriasGenerales(null);
        }
    }
}
