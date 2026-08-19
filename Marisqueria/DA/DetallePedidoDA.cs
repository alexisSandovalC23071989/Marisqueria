using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos.ModeloPedido;
using Dapper;
using System.Data;

namespace DA
{
    public class DetallePedidoDA : IDetallePedidoDA
    {
        private readonly DBContext _dbContext;

        public DetallePedidoDA(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Agregar(DetallePedidoRequest detalle)
        {
            using var conexion = _dbContext.CrearConexion();

            var parametros = new
            {
                IdDetalle = Guid.NewGuid(),
                IdPedido = detalle.IdPedido,
                IdProducto = detalle.IdProducto,
                Cantidad = detalle.Cantidad,
                PrecioUnitario = detalle.PrecioUnitario,
                Subtotal = detalle.Subtotal,
                PrecioOriginal = detalle.PrecioOriginal,
                TipoDescuento = detalle.TipoDescuento,
                ValorDescuento = detalle.ValorDescuento,
                IdPromocion = detalle.IdPromocion
            };

            return await conexion.ExecuteScalarAsync<Guid>("AgregarDetallePedido", parametros, commandType: CommandType.StoredProcedure);
        }

        public async Task<Guid> Editar(Guid id, DetallePedidoRequest detalle)
        {
            using var conexion = _dbContext.CrearConexion();

            var parametros = new
            {
                IdDetalle = id,
                Cantidad = detalle.Cantidad,
                PrecioUnitario = detalle.PrecioUnitario,
                Subtotal = detalle.Subtotal,
                PrecioOriginal = detalle.PrecioOriginal,
                TipoDescuento = detalle.TipoDescuento,
                ValorDescuento = detalle.ValorDescuento,
                IdPromocion = detalle.IdPromocion
            };

            return await conexion.ExecuteScalarAsync<Guid>("ActualizarDetallePedido", parametros, commandType: CommandType.StoredProcedure);
        }

        public async Task<Guid> Eliminar(Guid id)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.ExecuteScalarAsync<Guid>(
                "EliminarDetallePedido",
                new
                {
                    IdDetalle = id
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<DetallePedidoResponse> Obtener(Guid id)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.QueryFirstOrDefaultAsync<DetallePedidoResponse>("ObtenerDetallePedidoPorId",
                new
                {
                    IdDetalle = id
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<DetallePedidoResponse>> ObtenerPorPedido(Guid idPedido)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.QueryAsync<DetallePedidoResponse>(
                "ObtenerDetallePedido",
                new
                {
                    IdPedido = idPedido
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}