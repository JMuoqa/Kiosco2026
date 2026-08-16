using Conexiones;
using Entidades;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace Dominio
{
    public class AdminProductos
    {
        private Con_Productos _productos = new Con_Productos();
        public async Task<(bool estado, string mensaje)> CargarProducto(ProductosDto productoDto)
        {
            double precioVenta;
            double precioCompra;
            int cantidadProductos;
            if (!double.TryParse(productoDto.Venta, NumberStyles.Any, new CultureInfo("es-AR"), out precioVenta))
                throw new Exception("El campo de 'Precio de venta' no es numerico");
            if (!double.TryParse(productoDto.Compra, NumberStyles.Any, new CultureInfo("es-AR"), out precioCompra))
                throw new Exception("El campo de 'Costo de compra' no es numerico");
            if (!int.TryParse(productoDto.Cantidad, NumberStyles.Any, new CultureInfo("es-AR"), out cantidadProductos))
                throw new Exception("El campo de 'Cantidad' no es numerico");
            if (productoDto.Proveedor == "Proveedor")
                productoDto.Proveedor = "";
            await ValidarDatosProducto(productoDto);
            if (precioCompra >= precioVenta)
                throw new Exception("El campo de 'Venta' es menor al campo de 'Compra'");
            Productos producto = new Productos
            {
                Nombre = productoDto.Nombre,
                Categoria = productoDto.Categoria, //Es texto pero en el backend de la web conseguimos el id
                Codigo = productoDto.Codigo,
                Venta = precioVenta.ToString(), // esto tengo que corregirlo por que me parece que es rendundante 3/2/2026
                Compra = precioCompra.ToString(),
                Cantidad = cantidadProductos,
                Proveedor = productoDto.Proveedor,
                Ingreso = productoDto.Ingreso,
                Vencimiento = productoDto.Vencimiento
            };
            return await _productos.Con_CargarProducto(producto);
        }
        public async Task<(bool estado, bool existencia, string mensaje, Productos producto)> RevisarExistenciaProducto(string codigo)
        {
            return await _productos.Con_RevisarExistenciaProducto(codigo);
        }
        public async Task<(bool estado, string menasje)> EditarLote(Productos producto)
        {
            await ValidarDatosEditarLote(producto);
            return await _productos.Con_EditarLote(producto);
        }
        public async Task<(bool estado, string mensaje)> EditarProductoPorCodigo(Productos producto)
        {
            await ValidarDatosParaEdicionProducto(producto);
            double precioVenta;
            double precioCompra;
            if (!double.TryParse(producto.Venta, NumberStyles.Any, new CultureInfo("es-AR"), out precioVenta))
                throw new Exception("El campo de 'Precio de venta' no es numerico");
            if (!double.TryParse(producto.Compra, NumberStyles.Any, new CultureInfo("es-AR"), out precioCompra))
                throw new Exception("El campo de 'Costo de compra' no es numerico");
            if (precioCompra >= precioVenta)
                throw new Exception("El campo de 'Venta' es menor al campo de 'Compra'");
            return await _productos.Con_EditarProductoPorCodigo(producto);
        } 
        public async Task ValidarDatosParaEdicionProducto(Productos producto)
        {
            if (string.IsNullOrWhiteSpace(producto.Nombre) || producto.Nombre == "Nombre*")
                throw new Exception("El campo 'Nombre' esta vacio");
            if (string.IsNullOrWhiteSpace(producto.Categoria) || producto.Categoria == "Categoria*")
                throw new Exception("El campo 'Categoria' esta vacio");
            if (string.IsNullOrWhiteSpace(producto.Compra) || producto.Compra == "Costo de compra*")
                throw new Exception("El campo 'Compra' esta vacio");
            if (string.IsNullOrWhiteSpace(producto.Venta) || producto.Venta == "Precio de venta*")
                throw new Exception("El campo 'Venta' esta vacio");
        }
        public async Task<(bool estado, string mensaje, List<Productos> lista)> BusquedaProductoPorCodigoUltLote(string codigo)
        {
            return await _productos.Con_BusquedaPorCodigoUltLote(codigo);
        }
        public async Task<(bool estado, string mensaje, List<Categorias> lista)> BusquedaCategorias(string busqueda) 
        {
            return await _productos.Con_BusquedaCategorias(busqueda);
        }
        public async Task<(bool estado, string mensaje, List<Productos> lista)> Con_ObtenerUltLotePorNombre(string busqueda)
        {
            return await _productos.Con_ObtenerUltLotePorNombre(busqueda);
        }
        public async Task<(bool estado, string mensaje, List<Productos> lista)> BusquedaPorIngreso(string codigo, DateTime? busqueda)
        {
            return await _productos.Con_LotesPorIngreso(codigo, busqueda);
        }
        public async Task<(bool estado, string mensaje, List<Categorias> lista)> ObtenerCategoriasCliente()
        {
            return await _productos.Con_ObtenerCategorias();
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
        private async Task ValidarDatosEditarLote(Productos prod)
        {
            if (prod == null) throw new Exception("No hay datos del producto");
            if (string.IsNullOrWhiteSpace(prod.Nombre) || prod.Nombre == "Nombre*")
                throw new Exception("El campo 'Nombre' esta vacio");
            if (string.IsNullOrWhiteSpace(prod.Venta.ToString()))
                throw new Exception("El campo 'Venta' esta vacio");
            if (string.IsNullOrWhiteSpace(prod.Compra.ToString()))
                throw new Exception("El campo 'Compra' esta vacio");
            if (string.IsNullOrWhiteSpace(prod.Cantidad.ToString()))
                throw new Exception("El campo 'Cantidad' esta vacio");
            return;
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
            return;
        }
        private async Task ValidarDatosCategoria(Categorias categoria)
        {
            if (categoria == null)
                throw new Exception("No hay datos del producto");
            if(string.IsNullOrWhiteSpace(categoria.Nombre) ||categoria.Nombre == "Nombre de la categoria")
                throw new Exception("El campo 'Nombre', del formulario de carga, esta vacio");
            return;
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
