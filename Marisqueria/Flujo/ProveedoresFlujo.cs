using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelo;
namespace Flujo
{
    public class ProveedoresFlujo : IProveedoresFlujo
    {
        private readonly IProveedoresDA _proveedoresDA;

        public ProveedoresFlujo(IProveedoresDA proveedoresDA)
        {
            _proveedoresDA = proveedoresDA;
        }

        public async Task<IEnumerable<ProveedorResponse>> Obtener()
        {
            return await _proveedoresDA.Obtener();
        }

        public async Task<ProveedorResponse?> Obtener(int IdProveedor)
        {
            return await _proveedoresDA.Obtener(IdProveedor);
        }

        public async Task<int> Agregar(ProveedorRequest proveedor)
        {
            return await _proveedoresDA.Agregar(proveedor);
        }

        public async Task<int> Editar(int IdProveedor, ProveedorRequest proveedor)
        {
            return await _proveedoresDA.Editar(IdProveedor, proveedor);
        }

        public async Task<int> Eliminar(int IdProveedor)
        {
            return await _proveedoresDA.Eliminar(IdProveedor);
        }

        public async Task<IEnumerable<ProductoProveedorResponse>> ObtenerProductos(int IdProveedor)
        {
            return await _proveedoresDA.ObtenerProductos(IdProveedor);
        }

        public async Task<int> AgregarProducto(int IdProveedor, ProductoProveedorRequest productoProveedor)
        {
            return await _proveedoresDA.AgregarProducto(IdProveedor, productoProveedor);
        }

        public async Task<int> EditarProducto(int IdProveedor, ProductoProveedorRequest productoProveedor)
        {
            return await _proveedoresDA.EditarProducto(IdProveedor, productoProveedor);
        }

        public async Task<int> EliminarProducto(int IdProveedor, int IdProducto)
        {
            return await _proveedoresDA.EliminarProducto(IdProveedor, IdProducto);
        }
    }
}
