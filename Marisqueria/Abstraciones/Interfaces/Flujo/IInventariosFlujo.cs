using Abstracciones.Modelos.ModeloInventario;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IInventariosFlujo
    {
        Task<IEnumerable<InventarioResponse>> Obtener();
        Task<InventarioResponse?> Obtener(int IdInventario);
        Task<int> Agregar(InventarioRequest inventario);
        Task<int> Editar(int IdInventario, InventarioRequest inventario);
        Task<int> Eliminar(int IdInventario);
        Task<StockProductoResponse> ObtenerStockProducto(int idProducto);
        Task<int> DescontarStock(int idProducto, int cantidad);
        Task<int> DevolverStock(int idProducto, int cantidad);
    }
}
