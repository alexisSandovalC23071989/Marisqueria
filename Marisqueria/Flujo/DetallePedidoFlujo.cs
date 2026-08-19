using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos.ModeloPedido;

namespace Flujo
{
    public class DetallePedidoFlujo : IDetallePedidoFlujo
    {
        private readonly IDetallePedidoDA _detallePedidoDA;
        private readonly IPedidoDA _pedidoDA;
        private readonly IProductosDA _productosDA;
        private readonly IPromocionesDA _promocionesDA;
        private readonly IInventariosDA _inventariosDA;

        public DetallePedidoFlujo(IDetallePedidoDA detallePedidoDA, IPedidoDA pedidoDA, IProductosDA productosDA, IPromocionesDA promocionesDA, IInventariosDA inventariosDA)
        {
            _detallePedidoDA = detallePedidoDA;
            _pedidoDA = pedidoDA;
            _productosDA = productosDA;
            _promocionesDA = promocionesDA;
            _inventariosDA = inventariosDA;
        }

        public async Task<Guid> Agregar(DetallePedidoRequest detalle)
        {
            var producto = await _productosDA.Obtener(detalle.IdProducto);
            if (producto == null)
                throw new Exception("El producto seleccionado no existe.");

            var stock = await _inventariosDA.ObtenerStockProducto(detalle.IdProducto);
            if (stock.StockDisponible < detalle.Cantidad)
                throw new ArgumentException($"Stock insuficiente. Disponible: {stock.StockDisponible}.");

            await AplicarPromocionVigente(detalle, producto.PrecioVentaActual);
            detalle.Subtotal = detalle.PrecioUnitario * detalle.Cantidad;

            var resultado = await _detallePedidoDA.Agregar(detalle);
            await _inventariosDA.DescontarStock(detalle.IdProducto, detalle.Cantidad);
            await _pedidoDA.RecalcularTotal(detalle.IdPedido);
            return resultado;
        }

        public async Task<Guid> Editar(Guid id, DetallePedidoRequest detalle)
        {
            var detalleActual = await _detallePedidoDA.Obtener(id);
            if (detalleActual == null)
                throw new Exception("El detalle del pedido no existe.");

            int diferencia = detalle.Cantidad - detalleActual.Cantidad;
            if (diferencia > 0)
            {
                var stock = await _inventariosDA.ObtenerStockProducto(detalleActual.IdProducto);
                if (stock.StockDisponible < diferencia)
                    throw new ArgumentException($"Stock insuficiente. Disponible: {stock.StockDisponible}.");
                await _inventariosDA.DescontarStock(detalleActual.IdProducto, diferencia);
            }
            else if (diferencia < 0)
            {
                await _inventariosDA.DevolverStock(detalleActual.IdProducto, Math.Abs(diferencia));
            }

            detalle.IdPedido = detalleActual.IdPedido;
            detalle.IdProducto = detalleActual.IdProducto;

            var producto = await _productosDA.Obtener(detalle.IdProducto);
            if (producto == null)
                throw new Exception("El producto seleccionado no existe.");

            // Se valida otra vez la promoción al modificar el pedido. Así una promoción
            // vencida/desactivada deja de aplicarse y una nueva promoción vigente sí se aplica.
            await AplicarPromocionVigente(detalle, producto.PrecioVentaActual);
            detalle.Subtotal = detalle.PrecioUnitario * detalle.Cantidad;

            var resultado = await _detallePedidoDA.Editar(id, detalle);
            await _pedidoDA.RecalcularTotal(detalle.IdPedido);
            return resultado;
        }

        public async Task<Guid> Eliminar(Guid id)
        {
            var detalle = await _detallePedidoDA.Obtener(id);
            var resultado = await _detallePedidoDA.Eliminar(id);
            await _inventariosDA.DevolverStock(detalle.IdProducto, detalle.Cantidad);
            await _pedidoDA.RecalcularTotal(detalle.IdPedido);
            return resultado;
        }

        public async Task<DetallePedidoResponse> Obtener(Guid id)
        {
            var detalle = await _detallePedidoDA.Obtener(id);
            if (detalle == null) return detalle;

            var pedido = await _pedidoDA.Obtener(detalle.IdPedido);
            if (pedido != null && pedido.Estado == "Pendiente")
            {
                await RefrescarPrecioPromocional(detalle);
                detalle = await _detallePedidoDA.Obtener(id);
            }
            return detalle;
        }

        public async Task<IEnumerable<DetallePedidoResponse>> ObtenerPorPedido(Guid idPedido)
        {
            var detalles = (await _detallePedidoDA.ObtenerPorPedido(idPedido)).ToList();
            var pedido = await _pedidoDA.Obtener(idPedido);

            // HU-30: cada consulta de un pedido pendiente vuelve a validar la vigencia.
            // Los pedidos ya confirmados/preparados/entregados conservan el precio histórico.
            if (pedido != null && pedido.Estado == "Pendiente")
            {
                var cambio = false;
                foreach (var detalle in detalles)
                    cambio |= await RefrescarPrecioPromocional(detalle);

                if (cambio)
                {
                    await _pedidoDA.RecalcularTotal(idPedido);
                    detalles = (await _detallePedidoDA.ObtenerPorPedido(idPedido)).ToList();
                }
            }

            return detalles;
        }

        private async Task AplicarPromocionVigente(DetallePedidoRequest detalle, decimal precioOriginal)
        {
            detalle.PrecioOriginal = precioOriginal;
            detalle.PrecioUnitario = precioOriginal;
            detalle.TipoDescuento = string.Empty;
            detalle.ValorDescuento = 0;
            detalle.IdPromocion = null;

            var promocion = await _promocionesDA.ObtenerPrecioProducto(detalle.IdProducto);
            if (promocion == null) return;

            detalle.PrecioOriginal = promocion.PrecioOriginal;
            detalle.PrecioUnitario = promocion.PrecioPromocional;
            detalle.TipoDescuento = promocion.TipoDescuento;
            detalle.ValorDescuento = promocion.ValorDescuento;
            detalle.IdPromocion = promocion.IdPromocion;
        }

        private async Task<bool> RefrescarPrecioPromocional(DetallePedidoResponse actual)
        {
            var producto = await _productosDA.Obtener(actual.IdProducto);
            if (producto == null) return false;

            var actualizado = new DetallePedidoRequest
            {
                IdPedido = actual.IdPedido,
                IdProducto = actual.IdProducto,
                Cantidad = actual.Cantidad
            };
            await AplicarPromocionVigente(actualizado, producto.PrecioVentaActual);
            actualizado.Subtotal = actualizado.PrecioUnitario * actualizado.Cantidad;

            var cambio = actual.IdPromocion != actualizado.IdPromocion
                || actual.PrecioUnitario != actualizado.PrecioUnitario
                || actual.PrecioOriginal != actualizado.PrecioOriginal;

            if (cambio)
                await _detallePedidoDA.Editar(actual.IdDetalle, actualizado);

            return cambio;
        }
    }
}
