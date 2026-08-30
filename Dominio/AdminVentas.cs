using Conexiones;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class AdminVentas
    {
        private Con_Ventas _ventas = new Con_Ventas();
        public async Task<(bool estado, string menasje, long id)> RegistrarVenta(Ventas venta)
        {
            return await _ventas.Con_RegistrarVenta(venta);
        }
        public async Task<(bool estado, string mensaje, List<Ventas> ventas)> BuscarVentas(DateTime? desde, DateTime? hasta)
        {
            return await _ventas.Con_BuscarVentas(desde, hasta);
        }
        public async Task<(bool estado, string mensaje, Ventas venta)> BuscarDetalles(Ventas ventaGuia)
        {
            return await _ventas.Con_BuscarDetalles(ventaGuia);
        }
    }
}
