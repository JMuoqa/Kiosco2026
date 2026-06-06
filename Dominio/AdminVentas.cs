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
        public async Task<(bool estado, string menasje)> RegistrarVenta(List<Productos> pr, Ventas venta)
        {
            return await _ventas.Con_RegistrarVenta(pr, venta);
        }
    }
}
