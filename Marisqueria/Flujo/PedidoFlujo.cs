using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos.ModeloPedido;

namespace Flujo
{
    public class PedidoFlujo : IPedidoFlujo
    {
        private readonly IPedidoDA _pedidoDA;
        private readonly IDetallePedidoDA _detallePedidoDA;
        private readonly IInventariosDA _inventariosDA;

        public PedidoFlujo(IPedidoDA pedidoDA, IDetallePedidoDA detallePedidoDA, IInventariosDA inventariosDA)
        {
            _pedidoDA = pedidoDA;
            _detallePedidoDA = detallePedidoDA;
            _inventariosDA = inventariosDA;
        }

        public Task<Guid> Confirmar(Guid idPedido)
        {
            return _pedidoDA.Confirmar(idPedido);
        }

        public async Task<Guid> Cancelar(Guid idPedido)
        {
            var detalles = await _detallePedidoDA.ObtenerPorPedido(idPedido);

            foreach (var detalle in detalles)
            {
                await _inventariosDA.DevolverStock(detalle.IdProducto, detalle.Cantidad);
            }

            return await _pedidoDA.Cancelar(idPedido);
        }

        public Task<Guid> Preparar(Guid idPedido)
        {
            return _pedidoDA.Preparar(idPedido);
        }

        public Task<Guid> Entregar(Guid idPedido)
        {
            return _pedidoDA.Entregar(idPedido);
        }

        public Task<Guid> RecalcularTotal(Guid idPedido)
        {
            return _pedidoDA.RecalcularTotal(idPedido);
        }

        public Task<PedidoResponse> ObtenerPedidoActivo(Guid idUsuario)
        {
            return _pedidoDA.ObtenerPedidoActivo(idUsuario);
        }

        public Task<Guid> Agregar(PedidoRequest pedido)
        {
            return _pedidoDA.Agregar(pedido);
        }

        public Task<Guid> Editar(Guid id, PedidoRequest pedido)
        {
            return _pedidoDA.Editar(id, pedido);
        }

        public async Task<Guid> Eliminar(Guid id)
        {
            var detalles = await _detallePedidoDA.ObtenerPorPedido(id);

            foreach (var detalle in detalles)
            {
                await _inventariosDA.DevolverStock(detalle.IdProducto, detalle.Cantidad);
            }

            return await _pedidoDA.Eliminar(id);
        }

        public Task<IEnumerable<PedidoResponse>> Obtener()
        {
            return _pedidoDA.Obtener();
        }

        public Task<PedidoResponse> Obtener(Guid id)
        {
            return _pedidoDA.Obtener(id);
        }

        public Task<IEnumerable<PedidoResponse>> ObtenerPorUsuario(Guid idUsuario)
        {
            return _pedidoDA.ObtenerPorUsuario(idUsuario);
        }
    }
}