using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos.ModeloInventario;

namespace Flujo
{
    public class InventariosFlujo : IInventariosFlujo
    {
        private readonly IInventariosDA _inventariosDA;
        private readonly IProductosDA _productosDA;
        private readonly IProveedoresDA _proveedoresDA;

        public InventariosFlujo(
            IInventariosDA inventariosDA,
            IProductosDA productosDA,
            IProveedoresDA proveedoresDA)
        {
            _inventariosDA = inventariosDA;
            _productosDA = productosDA;
            _proveedoresDA = proveedoresDA;
        }

        public Task<IEnumerable<InventarioResponse>> Obtener() => _inventariosDA.Obtener();
        public Task<InventarioResponse?> Obtener(int IdInventario) => _inventariosDA.Obtener(IdInventario);

        public async Task<int> Agregar(InventarioRequest inventario)
        {
            await Validar(inventario);
            return await _inventariosDA.Agregar(inventario);
        }

        public async Task<int> Editar(int IdInventario, InventarioRequest inventario)
        {
            await Validar(inventario);
            return await _inventariosDA.Editar(IdInventario, inventario);
        }

        public Task<int> Eliminar(int IdInventario) => _inventariosDA.Eliminar(IdInventario);

        private async Task Validar(InventarioRequest inventario)
        {
            if (inventario.FechaVencimiento.Date < inventario.FechaIngreso.Date)
            {
                throw new ArgumentException("La fecha de vencimiento debe ser igual o posterior a la fecha de ingreso.");
            }

            if (await _productosDA.Obtener(inventario.IdProducto) == null)
            {
                throw new ArgumentException("El producto seleccionado no existe.");
            }

            if (await _proveedoresDA.Obtener(inventario.IdProveedor) == null)
            {
                throw new ArgumentException("El proveedor seleccionado no existe.");
            }
        }

        public Task<StockProductoResponse> ObtenerStockProducto(int idProducto)
        {
            return _inventariosDA.ObtenerStockProducto(idProducto);
        }

        public Task<int> DescontarStock(int idProducto, int cantidad)
        {
            return _inventariosDA.DescontarStock(idProducto, cantidad);
        }

        public Task<int> DevolverStock(int idProducto, int cantidad)
        {
            return _inventariosDA.DevolverStock(idProducto, cantidad);
        }
    }
}
