using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos.ModeloPedido;
using Dapper;
using System.Data;


namespace DA
{
    public class PedidoDA : IPedidoDA
    {
        private readonly DBContext _dbContext;

        public PedidoDA(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Agregar(PedidoRequest pedido)
        {
            using var conexion = _dbContext.CrearConexion();

            var parametros = new
            {
                IdPedido = Guid.NewGuid(),
                IdUsuario = pedido.IdUsuario,
                FechaPedido = DateTime.Now,
                Estado = "Pendiente",
                Total = 0
            };

            return await conexion.ExecuteScalarAsync<Guid>(
                "AgregarPedido",
                parametros,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Guid> Editar(Guid idPedido, PedidoRequest pedido)
        {
            using var conexion = _dbContext.CrearConexion();

            var parametros = new
            {
                IdPedido = idPedido,
                IdUsuario = pedido.IdUsuario,
                FechaPedido = DateTime.Now,
                //Estado = pedido.Estado,
                //Total = pedido.Total
            };

            return await conexion.ExecuteScalarAsync<Guid>(
                "EditarPedido",
                parametros,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Guid> Eliminar(Guid idPedido)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.ExecuteScalarAsync<Guid>(
                "EliminarPedido",
                new
                {
                    IdPedido = idPedido
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<PedidoResponse>> Obtener()
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.QueryAsync<PedidoResponse>(
                "ObtenerPedidos",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<PedidoResponse> Obtener(Guid id)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.QueryFirstOrDefaultAsync<PedidoResponse>(
                "ObtenerPedido",
                new
                {
                    IdPedido = id
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<PedidoResponse>> ObtenerPorUsuario(Guid idUsuario)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.QueryAsync<PedidoResponse>(
                "ObtenerPedidosCliente",
                new
                {
                    IdUsuario = idUsuario
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<PedidoResponse> ObtenerPedidoActivo(Guid idUsuario)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.QueryFirstOrDefaultAsync<PedidoResponse>(
                "ObtenerPedidoActivo",
                new
                {
                    IdUsuario = idUsuario
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Guid> Confirmar(Guid idPedido)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.ExecuteScalarAsync<Guid>(
                "ConfirmarPedido",
                new
                {
                    IdPedido = idPedido
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Guid> Cancelar(Guid idPedido)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.ExecuteScalarAsync<Guid>(
                "CancelarPedido",
                new
                {
                    IdPedido = idPedido
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Guid> Preparar(Guid idPedido)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.ExecuteScalarAsync<Guid>(
                "MarcarPedidoPreparado",
                new
                {
                    IdPedido = idPedido
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Guid> Entregar(Guid idPedido)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.ExecuteScalarAsync<Guid>(
                "EntregarPedido",
                new
                {
                    IdPedido = idPedido
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Guid> RecalcularTotal(Guid idPedido)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.ExecuteScalarAsync<Guid>(
                "RecalcularTotalPedido",
                new
                {
                    IdPedido = idPedido
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}
