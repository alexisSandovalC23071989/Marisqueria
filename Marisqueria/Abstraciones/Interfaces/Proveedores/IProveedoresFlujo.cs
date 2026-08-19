using Abstracciones.Modelo;
namespace Abstracciones.Interfaces.Flujo
{
    public interface IProveedoresFlujo
    {
        Task<IEnumerable<ProveedorResponse>> Obtener();
        Task<ProveedorResponse?> Obtener(int IdProveedor);
        Task<int> Agregar(ProveedorRequest proveedor);
        Task<int> Editar(int IdProveedor, ProveedorRequest proveedor);
        Task<int> Eliminar(int IdProveedor);

        Task<IEnumerable<ProductoProveedorResponse>> ObtenerProductos(int IdProveedor);
        Task<int> AgregarProducto(int IdProveedor, ProductoProveedorRequest productoProveedor);
        Task<int> EditarProducto(int IdProveedor, ProductoProveedorRequest productoProveedor);
        Task<int> EliminarProducto(int IdProveedor, int IdProducto);
    }
}
