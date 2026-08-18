namespace Entidades
{
    public class Productos
    {
        public Productos() { }
        public Productos(string nombre)
        {
            this.Nombre = nombre;
        }
        public int ID { get; set; }
        public int IDUsuario { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public int IDCategoria { get; set; }// esto es para convertir el json correctamente a este objeto
        public string Codigo { get; set; }
        private string _venta;

        public string Venta 
        {
            get => _venta;
            set 
            {
                decimal numero;
                if (!decimal.TryParse(value, out numero))
                    throw new Exception("No es un numero el valor que quieres cambiar");
                if (numero <= 0)
                    throw new Exception("El campo 'Precio de venta' no puede ser negativo");
                _venta = value;
            }
        }
        private string _compra;
        public string Compra
        {
            get => _compra;
            set
            {
                decimal numero;
                if (!decimal.TryParse(value, out numero))
                    throw new Exception("No es un numero el valor que quieres cambiar");
                if (numero <= 0)
                    throw new Exception("El campo 'Costo de compra' no puede ser negativo");
                _compra = value;
            }
        }
        private int _cantidad;
        public int Cantidad
        {
            get => _cantidad;
            set
            {
                if (value < 0)
                    throw new Exception("El campo 'Cantidad' no puede ser negativo");
                _cantidad = value;
            }
        }
        public string Proveedor { get; set; }
        private string _mostrar;
        public string Mostrar 
        {
            get => _mostrar.ToUpper();
            set { _mostrar = value; } 
        }
        public DateTime Ingreso { get; set; }
        public DateTime Vencimiento { get; set; }

        public string FechaIngresoARG
        {
            get
            {
                string fecha = $"{Ingreso.Day.ToString().PadLeft(2, '0')}/{Ingreso.Month.ToString().PadLeft(2, '0')}/{Ingreso.Year} {Ingreso.Hour.ToString().PadLeft(2, '0')}:{Ingreso.Minute.ToString().PadLeft(2, '0')}";
                return fecha;
            }
        }
        public string FechaVencimientoARG
        {
            get
            {
                string fecha = $"{Vencimiento.Day.ToString().PadLeft(2, '0')}/{Vencimiento.Month.ToString().PadLeft(2, '0')}/{Vencimiento.Year}";
                return fecha;
            }
        }
    }
    public class ProductosDto
    {
        public string ID { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public string Codigo { get; set; }
        public string Venta { get; set; }
        public string Compra { get; set; }
        public string Cantidad { get; set; }
        public string Proveedor { get; set; }
        public string Visibilidad { get; set; }
        public DateTime Ingreso { get; set; }
        public DateTime Vencimiento { get; set; }
    }
    public class Categorias
    {
        public Categorias() { }
        public Categorias(string cat)
        {
            this.Nombre = cat;
        }
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Mostrar { get; set; }
    }
}
