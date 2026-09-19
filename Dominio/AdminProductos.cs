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
            int margenGanancia;
            decimal precioCompra;
            int cantidadProductos;
            if (!int.TryParse(productoDto.Margen, out margenGanancia))
                throw new Exception("El campo de 'Margen de ganancia' no es numerico");
            if (!decimal.TryParse(productoDto.Compra, NumberStyles.Any, new CultureInfo("es-AR"), out precioCompra))
                throw new Exception("El campo de 'Costo de compra' no es numerico");
            if (!int.TryParse(productoDto.Cantidad, NumberStyles.Any, new CultureInfo("es-AR"), out cantidadProductos))
                throw new Exception("El campo de 'Cantidad' no es numerico");
            if (productoDto.Proveedor == "Proveedor")
                productoDto.Proveedor = "";
            await ValidarDatosProducto(productoDto);
            decimal precioVenta = precioCompra + (precioCompra * margenGanancia / 100);
            if (precioCompra >= precioVenta)
                throw new Exception("El campo de 'Venta' es menor o igual al campo de 'Costo de compra'");
            decimal precioVentaUnitario = precioVenta / cantidadProductos;
            decimal precioCompraUnitario = precioCompra / cantidadProductos;
            Productos producto = new Productos
            {
                Nombre = productoDto.Nombre.ToUpper(),
                Categoria = productoDto.Categoria, //Es texto 
                Codigo = productoDto.Codigo,
                Venta = precioVentaUnitario.ToString(CultureInfo.InvariantCulture), 
                Compra = precioCompraUnitario.ToString(CultureInfo.InvariantCulture),
                Cantidad = cantidadProductos,
                Proveedor = productoDto.Proveedor,
                Ingreso = productoDto.Ingreso,
                Vencimiento = productoDto.Vencimiento,
                UnidadMedida = productoDto.UnidadMedida,
                
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
            if (string.IsNullOrWhiteSpace(producto.Nombre) || producto.Nombre.ToLower().Trim() == "Nombre*".ToLower().Trim())
                throw new Exception("El campo 'Nombre' esta vacio");
            if (string.IsNullOrWhiteSpace(producto.Categoria) || producto.Categoria.ToLower().Trim() == "Categoria*".ToLower().Trim())
                throw new Exception("El campo 'Categoria' esta vacio");
            if (string.IsNullOrWhiteSpace(producto.Compra) || producto.Compra.ToLower().Trim() == "Costo de compra*".ToLower().Trim())
                throw new Exception("El campo 'Compra' esta vacio");
            if (string.IsNullOrWhiteSpace(producto.Venta) || producto.Venta.ToLower().Trim() == "Precio de venta*".ToLower().Trim())
                throw new Exception("El campo 'Venta' esta vacio");
            if (producto.Nombre.Length > 100)
                throw new Exception("El nombre del producto no puede superar los 100 caracteres.");
        }
        public async Task<(bool estado, string mensaje, List<Productos> lista)> BusquedaProductoPorCodigoUltLote(string codigo)
        {
            var primerRespuesta = await _productos.Con_BusquedaPorCodigoUltLote(codigo);
            //codigo.Any(char.IsLetter) comprueba si almenos tiene una letra
            if (primerRespuesta.lista.Count == 0 && codigo.Any(char.IsLetter)) return await _productos.Con_ObtenerUltLotePorNombre(codigo, ""); 
            return primerRespuesta;
        }
        public async Task<(bool estado, string mensaje, List<Categorias> lista)> BusquedaCategorias(string busqueda) 
        {
            return await _productos.Con_BusquedaCategorias(busqueda);
        }
        public async Task<(bool estado, string mensaje, List<Productos> lista)> Con_ObtenerUltLotePorNombre(string busqueda, string categoria)
        {
            return await _productos.Con_ObtenerUltLotePorNombre(busqueda, categoria);
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
            if (string.IsNullOrWhiteSpace(prod.Nombre) || prod.Nombre.ToLower().Trim() == "Nombre*".ToLower().Trim())
                throw new Exception("El campo 'Nombre' esta vacio");
            if (string.IsNullOrWhiteSpace(prod.Venta.ToString()))
                throw new Exception("El campo 'Venta' esta vacio");
            if (string.IsNullOrWhiteSpace(prod.Compra.ToString()))
                throw new Exception("El campo 'Compra' esta vacio");
            if (string.IsNullOrWhiteSpace(prod.Cantidad.ToString()))
                throw new Exception("El campo 'Cantidad' esta vacio");
            if (prod.Nombre.Length > 100)
                throw new Exception("El nombre del producto no puede superar los 100 caracteres.");
            return;
        }
        private async Task ValidarDatosDeEditarCategoria(Categorias categoria)
        {
            if (categoria == null)
                throw new Exception("No hay datos del producto");
            if (string.IsNullOrWhiteSpace(categoria.Nombre) || categoria.Nombre.ToLower().Trim() == "NOMBRE DE LA CATEGORIA".ToLower().Trim())
                throw new Exception("El campo 'Nombre', del formulario de carga, esta vacio");
            if (string.IsNullOrWhiteSpace(categoria.Mostrar))
                throw new Exception("El campo 'Mostrar' no esta definido.");
            if (categoria.ID <= 0)
                throw new Exception("No hay ninguna categoria seleccionada");
            if (categoria.Nombre.Length > 80)
                throw new Exception("El nombre de la categoria no puede superar los 80 caracteres");
            return;
        }
        private async Task ValidarDatosCategoria(Categorias categoria)
        {
            if (categoria == null)
                throw new Exception("No hay datos del producto");
            if(string.IsNullOrWhiteSpace(categoria.Nombre) || categoria.Nombre.ToLower().Trim() == "NOMBRE DE LA CATEGORIA".ToLower().Trim())
                throw new Exception("El campo 'Nombre', del formulario de carga, esta vacio");
            if(categoria.Nombre.Length > 80)
                throw new Exception("El nombre de la categoria no puede superar los 80 caracteres");
            return;
        }
        private async Task ValidarDatosProducto(ProductosDto prod)
        {
            if (prod == null)
                throw new Exception ("No hay datos del producto");
            if (string.IsNullOrWhiteSpace(prod.Margen) || prod.Margen.ToLower().Trim() == "Margen de ganancia en %*".ToLower().Trim())
                throw new Exception("El campo 'Margen' esta vacio");
            if (string.IsNullOrWhiteSpace(prod.UnidadMedida) || prod.UnidadMedida.ToLower().Trim() == "Unidad de medida*".ToLower().Trim())
                throw new Exception("El campo 'Unidad de medida' esta vacio");
            if (string.IsNullOrWhiteSpace(prod.Nombre) || prod.Nombre.ToLower().Trim() == "Nombre*".ToLower().Trim())
                throw new Exception("El campo 'Nombre' esta vacio");
            if (string.IsNullOrWhiteSpace(prod.Categoria) || prod.Categoria.ToLower().Trim() == "Categoria*".ToLower().Trim())
                throw new Exception("El campo 'Categoria' esta vacio");
            if (string.IsNullOrWhiteSpace(prod.Codigo) || prod.Codigo.ToLower().Trim() == "Código*".ToLower().Trim())
                throw new Exception("El campo 'Codigo' esta vacio");
            if (string.IsNullOrWhiteSpace(prod.Compra.ToString()))
                throw new Exception("El campo 'Compra' esta vacio");
            if (string.IsNullOrWhiteSpace(prod.Cantidad.ToString()))
                throw new Exception("El campo 'Cantidad' esta vacio");
            if(prod.Ingreso >= prod.Vencimiento)
                throw new Exception("La fecha de vencimiento es inferior o igual a la de ingreso");
            if (prod.Nombre.Length > 100)
                throw new Exception("El nombre del producto no puede superar los 100 caracteres.");
            return;
        }
    }
}
