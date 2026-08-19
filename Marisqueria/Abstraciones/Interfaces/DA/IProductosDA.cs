using Abstracciones.Modelos.ModeloProductos;

namespace Abstracciones.Interfaces.DA
{
    public interface IProductosDA
    {
        Task<IEnumerable<ProductoResponse>> Obtener();
        Task<ProductoResponse?> Obtener(int IdProducto);
        Task<int> Agregar(ProductoRequest producto);
        Task<int> Editar(int IdProducto, ProductoRequest producto);
        Task<int> Eliminar(int IdProducto);
    }
}
