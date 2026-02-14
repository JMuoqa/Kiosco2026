using Kiosco2026.ModeloDeLasVistas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Kiosco2026.Clases.Colores
{
    // Esta clase debe implementar INotifyPropertyChanged por mas que se llame dentro de una calse que lo implementa
    public class ColoresProductos : ModeloDeVistaBase
    {
        private Brush _nombre;
        private Brush _categoria;
        private Brush _codigo;
        private Brush _compra;
        private Brush _venta;
        private Brush _cantidad;
        private Brush _proveedor;
        public Brush Nombre 
        {
            get => _nombre;
            set
            {
                _nombre = value;
                OnPropertyChanged(nameof(Nombre));
            } 
        }
        public Brush Categoria
        {
            get => _categoria;
            set
            {
                _categoria = value;
                OnPropertyChanged(nameof(Categoria));
            }
        }
        public Brush Codigo
        {
            get => _codigo;
            set
            {
                _codigo = value;
                OnPropertyChanged(nameof(Codigo));
            }
        }
        public Brush Compra
        {
            get => _compra;
            set
            {
                _compra = value;
                OnPropertyChanged(nameof(Compra));
            }
        }
        public Brush Venta
        {
            get => _venta;
            set
            {
                _venta = value;
                OnPropertyChanged(nameof(Venta));
            }
        }
        public Brush Cantidad
        {
            get => _cantidad;
            set
            {
                _cantidad = value;
                OnPropertyChanged(nameof(Cantidad));
            }
        }
        public Brush Proveedor
        {
            get => _proveedor;
            set
            {
                _proveedor = value;
                OnPropertyChanged(nameof(Proveedor));
            }
        }

    }
}
