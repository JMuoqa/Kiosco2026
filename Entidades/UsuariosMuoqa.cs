using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class UsuarioMuoqa
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Usuario { get; set; }
        public string Email { get; set; }
        public string FotoDePerfil { get; set; }
        public string Contrasena { get; set; }
        public string Telefono { get; set; }
        public string Empresa { get; set; }
        public string Jerarquia { get; set; }
        public string Origen { get; set; }
        public string Estado { get; set; }
        public string Intentos { get; set; }
        public string Token { get; set; }
    }
    public class EstadoAut
    {
        public bool estado { get; set; }
        public string mensaje { get; set; }
        public string Token { get; set; }
    }
}
