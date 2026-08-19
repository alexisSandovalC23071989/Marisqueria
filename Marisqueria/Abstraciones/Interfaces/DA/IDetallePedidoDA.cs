using Abstracciones.Modelos.ModeloPedido;

namespace Abstracciones.Interfaces.DA
{
    public interface IDetallePedidoDA
    {
        Task<Guid> Agregar(DetallePedidoRequest detalle);

        Task<Guid> Editar(Guid id, DetallePedidoRequest detalle);

        Task<Guid> Eliminar(Guid id);

        Task<IEnumerable<DetallePedidoResponse>> ObtenerPorPedido(Guid idPedido);

        Task<DetallePedidoResponse> Obtener(Guid id);
    }
}
