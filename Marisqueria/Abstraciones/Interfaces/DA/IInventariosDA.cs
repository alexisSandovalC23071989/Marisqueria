using Abstracciones.Modelos.ModeloInventario;

namespace Abstracciones.Interfaces.DA
{
    public interface IInventariosDA
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
