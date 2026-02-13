using Conexiones;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Dominio
{
    public class AdminAut
    {
        private Con_Aut _aut= new Con_Aut();
        public async Task<(bool estado, string mensaje, string datosUsuario)> Con_IniciarSesion(UsuarioMuoqa usuario)
        {
            return await _aut.Con_IniciarSesion(usuario);
        }
    }
}
