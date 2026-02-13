using Conexiones;
using Entidades;

namespace Dominio
{
    public class AdminProductos
    {
        private Con_Productos _productos = new Con_Productos();
        public async Task<(bool estado, string mensaje, List<Categorias> lista)> ObtenerCategoriasGenerales()
        {
            return await _productos.Con_ObtenerCategoriasGenerales();
        }
        public async Task<(bool estado, string mensaje)> CargarProducto(ProductosDto productoDto)
        {
            double precioVenta;
            double precioCompra;
            int cantidadProductos;
            if (!double.TryParse(productoDto.Venta, out precioVenta))
                throw new Exception("El campo de 'Precio de venta' no es numerico");
            if (!double.TryParse(productoDto.Compra, out precioCompra))
                throw new Exception("El campo de 'Costo de compra' no es numerico");
            if (!int.TryParse(productoDto.Cantidad, out cantidadProductos))
                throw new Exception("El campo de 'Cantidad' no es numerico");
            if (productoDto.Proveedor == "Proveedor")
                productoDto.Proveedor = "";
            await ValidarDatosProducto(productoDto);
            Productos producto = new Productos
            {
                Nombre = productoDto.Nombre,
                Categoria = productoDto.Categoria, //Es texto pero en el backend de la web conseguimos el id
                Codigo = productoDto.Codigo,
                Venta = precioVenta.ToString(),// esto tengo que corregirlo por que me parece que es rendundante 3/2/2026
                Compra = precioCompra.ToString(),
                Cantidad = cantidadProductos,
                Proveedor = productoDto.Proveedor,
                Ingreso = productoDto.Ingreso,
                Vencimiento = productoDto.Vencimiento
            };
            return await _productos.Con_CargarProducto(producto);
        }
        public async Task<(bool estado, string mensaje, List<Categorias> lista)> BusquedaCategorias(string busqueda) 
        {
            return await _productos.Con_BusquedaCategorias(busqueda);
        }
        public async Task<(bool estado, string mensaje, List<Productos> lista)> BusquedaProductos(string busqueda)
        {
            return await _productos.Con_BusquedaProductos(busqueda);
        }
        public async Task<(bool estado, string mensaje, List<Productos> lista)> BusquedaPorIngreso(string codigo)
        {
            return await _productos.Con_ProductosPorIngreso(codigo);
        }
        public async Task<(bool estado, string mensaje, List<Categorias> lista)> ObtenerCategoriasCliente()
        {
            return await _productos.Con_ObtenerCategoriasClientes();
        }
        public async Task<(bool estado, string mensaje)> CargarCategoria(Categorias categoria)
        {
            await ValidarDatosCategoria(categoria);
            return await _productos.Con_CargarCategoria(categoria);
        }
        public async Task<(bool estado, string mensaje)> EditarCategoria(Categorias categoria) 
        {
            await ValidarDatosDeEditarCategoria(categoria);
            return await _productos.Con_EditarCategoria(categoria);
        }
        private async Task ValidarDatosDeEditarCategoria(Categorias categoria)
        {
            if (categoria == null)
                throw new Exception("No hay datos del producto");
            if (string.IsNullOrWhiteSpace(categoria.Nombre) || categoria.Nombre == "Nombre de la categoria")
                throw new Exception("El campo 'Nombre', del formulario de carga, esta vacio");
            if (string.IsNullOrWhiteSpace(categoria.Mostrar))
                throw new Exception("El campo 'Mostrar' no esta definido.");
            if (categoria.ID <= 0)
                throw new Exception("No hay ninguna categoria seleccionada");
        }
        private async Task ValidarDatosCategoria(Categorias categoria)
        {
            if (categoria == null)
                throw new Exception("No hay datos del producto");
            if(string.IsNullOrWhiteSpace(categoria.Nombre) ||categoria.Nombre == "Nombre de la categoria")
                throw new Exception("El campo 'Nombre', del formulario de carga, esta vacio");
        }
        private async Task ValidarDatosProducto(ProductosDto prod)
        {
            if (prod == null)
                throw new Exception ("No hay datos del producto");
            if (string.IsNullOrWhiteSpace(prod.Nombre) || prod.Nombre == "Nombre*")
                throw new Exception("El campo 'Nombre' esta vacio");
            if (string.IsNullOrWhiteSpace(prod.Categoria) || prod.Categoria == "Categoria*")
                throw new Exception("El campo 'Categoria' esta vacio");
            if (string.IsNullOrWhiteSpace(prod.Codigo) || prod.Codigo == "Código*")
                throw new Exception("El campo 'Codigo' esta vacio");
            if (string.IsNullOrWhiteSpace(prod.Venta.ToString()))
                throw new Exception("El campo 'Venta' esta vacio");
            if (string.IsNullOrWhiteSpace(prod.Compra.ToString()))
                throw new Exception("El campo 'Compra' esta vacio");
            if (string.IsNullOrWhiteSpace(prod.Cantidad.ToString()))
                throw new Exception("El campo 'Cantidad' esta vacio");
            if(prod.Ingreso >= prod.Vencimiento)
                throw new Exception("La fecha de vencimiento es inferior o igual a la de ingreso");
            return;
        }
    }
}
