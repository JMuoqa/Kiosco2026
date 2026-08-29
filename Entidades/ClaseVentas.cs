using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Ventas
    {
        
        public Ventas()
        {
            _productos = new List<Productos>();
        }
        public Ventas (List<Productos> productos, decimal descuento, decimal subtotal, decimal total, int items, int cantidadProductos, string formaDePago)
        {
            _productos = productos;
            Descuento = descuento;
            SubTotal = subtotal;
            Total = total;
            Items = items;
            CantidadProductos = cantidadProductos;
            FormaDePago = formaDePago ?? "";
        }
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string FechaARG
        {
            get
            {
                string fecha = $"{Fecha.Day.ToString().PadLeft(2, '0')}/{Fecha.Month.ToString().PadLeft(2, '0')}/{Fecha.Year} {Fecha.Hour.ToString().PadLeft(2, '0')}:{Fecha.Minute.ToString().PadLeft(2, '0')}";
                return fecha;
            }
        }
        public int Items { get; set;  }
        public int CantidadProductos { get; set; }
        private List<Productos> _productos;
        public List<Productos> Productos
        {
            get => _productos;
            set
            {
                _productos = value;
            }
        }
        private string _formaDePago;
        public string FormaDePago
        {
            get => _formaDePago;
            set
            {
                _formaDePago = value.ToUpper();
            }
        }
        public decimal Descuento { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
    }
}
