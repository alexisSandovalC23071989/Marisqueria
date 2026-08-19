using Abstracciones.Modelos.ModeloPedido;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IDetallePedidoFlujo
    {
        Task<Guid> Agregar(DetallePedidoRequest detalle);

        Task<Guid> Editar(Guid id, DetallePedidoRequest detalle);

        Task<Guid> Eliminar(Guid id);

        Task<IEnumerable<DetallePedidoResponse>> ObtenerPorPedido(Guid idPedido);

        Task<DetallePedidoResponse> Obtener(Guid id);
    }
}
