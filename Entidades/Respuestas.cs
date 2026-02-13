using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class ResCategorias
    {
        public bool estado { get; set; }
        public string mensaje { get; set; }
        public List<Categorias> Categorias { get; set; }
    }
    public class ResProductos
    {
        public bool estado { get; set; }
        public string mensaje { get; set; }
    }
    public class ResProductosDos
    {
        public bool estado { get; set; }
        public string mensaje { get; set; }
        public List<Productos> Productos { get; set; }
    }
    public class MiSecreto
    {
        public string Mi_secreto { get; set; }
    }
}
