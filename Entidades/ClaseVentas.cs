using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Ventas
    {
        public int Id;
        public DateTime Fecha;
        public int Items;
        public int CantidadProductos;
        private string _formaDePago;
        public string FormaDePago
        {
            get => _formaDePago;
            set
            {
                _formaDePago = value.ToLower();
            }
        }
        public double Total;
    }
    public class VentaDetalles
    {

    }
}
