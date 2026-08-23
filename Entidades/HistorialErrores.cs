using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class HistorialErrores
    {
        public HistorialErrores() { }
        public HistorialErrores(string mensaje, string tipoError, string stackTrace, string nameSpace, string clase, string metodo, string cajero) 
        {
            Mensaje = mensaje;
            TipoError = tipoError;
            StackTrace = stackTrace;
            NameSpace = nameSpace;
            Clase = clase;
            Metodo = metodo;
            Cajero = cajero;
        } 
        public string Mensaje { get; set; } 
        public string TipoError { get; set; } 
        public string StackTrace { get; set; }
        public string NameSpace { get; set; } 
        public string Clase { get; set; } 
        public string Metodo { get; set;  }
        public string Cajero { get; set; }
    }
}
