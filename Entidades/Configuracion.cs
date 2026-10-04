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
        public string IPHOST { get; set; }
        public string NombreNegocio { get; set; }
        public int NumeroDeCaja { get; set; }
        public int NumeroDeSucursal { get; set; }
        public string NombreDeCaja { get; set; }
        public string DireccionDelLocal { get; set; }
        public string BaseDeDatos { get; set; }
        public string Usuario { get; set; }
        public string UsuarioBD { get; set; }
        public string ContrasenaBD { get; set; }
        public string VersionActual { get; set; }
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
    public class ConfiguracionImpresora
    {
        public string NombreImpresora { get; set;  }
        public int AnchoPapel { get; set;  }
        public bool ImprimirAutomaticamente { get;set; }
        public int Copias { get; set; }
    }
}
