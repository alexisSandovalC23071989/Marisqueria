using Abstracciones.Modelos.ModeloPedido;

namespace Abstracciones.Interfaces.DA
{
    public interface IPedidoDA
    {
        Task<Guid> Agregar(PedidoRequest pedido);

        Task<Guid> Editar(Guid id, PedidoRequest pedido);

        Task<Guid> Eliminar(Guid id);

        Task<IEnumerable<PedidoResponse>> Obtener();

        Task<PedidoResponse> Obtener(Guid id);

        Task<IEnumerable<PedidoResponse>> ObtenerPorUsuario(Guid idUsuario);

        Task<PedidoResponse> ObtenerPedidoActivo(Guid idUsuario);

        Task<Guid> Confirmar(Guid idPedido);

        Task<Guid> Cancelar(Guid idPedido);

        Task<Guid> Preparar(Guid idPedido);

        Task<Guid> Entregar(Guid idPedido);

        Task<Guid> RecalcularTotal(Guid idPedido);
    }
}
