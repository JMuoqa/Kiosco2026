using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entidades
{
    public class Configuracion
    {
        public string CodigoSeguridad { get; set; }
        public string NombreNegocio { get; set; }
        public int NumeroDeCaja { get; set; }
        public int NumeroDeSucursal { get; set; }
        public string NombreDeCaja { get; set; }
        public string DireccionDelLocal { get; set; }
        public string BaseDeDatos { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
    }
    public class ColoresTexto
    {
        public ColoresTexto()
        {
            ColorError = "#990000";
            ColorExito = "#009933";
            Normal = "#000000";
        }
        public string ColorError;
        public string ColorExito;
        public string Normal;
    }
}
