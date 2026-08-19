using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos.ModeloPromociones;
using Dapper;
using System.Data;

namespace DA
{
    public class PromocionesDA : IPromocionesDA
    {
        private readonly DBContext _dbContext;
        public PromocionesDA(DBContext dbContext) => _dbContext = dbContext;

        public async Task<IEnumerable<PromocionResponse>> Obtener(string? estado = null)
        {
            using var conexion = _dbContext.CrearConexion();
            var promociones = (await conexion.QueryAsync<PromocionResponse>(
                "ObtenerPromociones", new { Estado = estado }, commandType: CommandType.StoredProcedure)).ToList();
            await CargarRelaciones(conexion, promociones);
            return promociones;
        }

        public async Task<IEnumerable<PromocionResponse>> ObtenerActivas()
        {
            using var conexion = _dbContext.CrearConexion();
            var promociones = (await conexion.QueryAsync<PromocionResponse>(
                "ObtenerPromocionesActivas", commandType: CommandType.StoredProcedure)).ToList();
            await CargarRelaciones(conexion, promociones);
            return promociones;
        }

        public async Task<PromocionResponse?> Obtener(int IdPromocion)
        {
            using var conexion = _dbContext.CrearConexion();
            var promocion = await conexion.QueryFirstOrDefaultAsync<PromocionResponse>(
                "ObtenerPromocion", new { IdPromocion }, commandType: CommandType.StoredProcedure);
            if (promocion != null)
                await CargarRelaciones(conexion, new List<PromocionResponse> { promocion });
            return promocion;
        }

        public async Task<PrecioPromocionalResponse?> ObtenerPrecioProducto(int IdProducto)
        {
            using var conexion = _dbContext.CrearConexion();
            return await conexion.QueryFirstOrDefaultAsync<PrecioPromocionalResponse>(
                "ObtenerPrecioPromocionalProducto", new { IdProducto }, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<AlertaPromocionResponse>> ObtenerAlertas(int diasAnticipacion = 3, bool soloPendientes = true)
        {
            using var conexion = _dbContext.CrearConexion();
            return await conexion.QueryAsync<AlertaPromocionResponse>(
                "ObtenerAlertasPromociones",
                new { DiasAnticipacion = diasAnticipacion, SoloPendientes = soloPendientes },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> MarcarAlertaRevisada(int IdAlerta)
        {
            using var conexion = _dbContext.CrearConexion();
            return await conexion.ExecuteScalarAsync<int>(
                "MarcarAlertaPromocionRevisada", new { IdAlerta }, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> Agregar(PromocionRequest promocion)
        {
            using var conexion = _dbContext.CrearConexion();
            conexion.Open();
            using var transaccion = conexion.BeginTransaction();
            try
            {
                var id = await conexion.ExecuteScalarAsync<int>("AgregarPromocion", new
                {
                    promocion.Nombre,
                    promocion.Descripcion,
                    promocion.TipoDescuento,
                    promocion.ValorDescuento,
                    promocion.FechaInicio,
                    promocion.FechaFin
                }, transaccion, commandType: CommandType.StoredProcedure);

                foreach (var idProducto in promocion.IdsProductos.Distinct())
                    await conexion.ExecuteAsync("AgregarPromocionProducto", new { IdPromocion = id, IdProducto = idProducto }, transaccion, commandType: CommandType.StoredProcedure);

                foreach (var idCategoria in promocion.IdsCategorias.Distinct())
                    await conexion.ExecuteAsync("AgregarPromocionCategoria", new { IdPromocion = id, IdCategoria = idCategoria }, transaccion, commandType: CommandType.StoredProcedure);

                transaccion.Commit();
                return id;
            }
            catch
            {
                transaccion.Rollback();
                throw;
            }
        }

        public async Task<int> Editar(int IdPromocion, PromocionRequest promocion)
        {
            using var conexion = _dbContext.CrearConexion();
            conexion.Open();
            using var transaccion = conexion.BeginTransaction();
            try
            {
                await conexion.ExecuteAsync("EditarPromocion", new
                {
                    IdPromocion,
                    promocion.Nombre,
                    promocion.Descripcion,
                    promocion.TipoDescuento,
                    promocion.ValorDescuento,
                    promocion.FechaInicio,
                    promocion.FechaFin
                }, transaccion, commandType: CommandType.StoredProcedure);

                await conexion.ExecuteAsync("EliminarProductosPromocion", new { IdPromocion }, transaccion, commandType: CommandType.StoredProcedure);
                await conexion.ExecuteAsync("EliminarCategoriasPromocion", new { IdPromocion }, transaccion, commandType: CommandType.StoredProcedure);
                // Si la vigencia cambió, la alerta anterior deja de ser válida y podrá generarse nuevamente.
                await conexion.ExecuteAsync("ReiniciarAlertaPromocion", new { IdPromocion }, transaccion, commandType: CommandType.StoredProcedure);

                foreach (var idProducto in promocion.IdsProductos.Distinct())
                    await conexion.ExecuteAsync("AgregarPromocionProducto", new { IdPromocion, IdProducto = idProducto }, transaccion, commandType: CommandType.StoredProcedure);

                foreach (var idCategoria in promocion.IdsCategorias.Distinct())
                    await conexion.ExecuteAsync("AgregarPromocionCategoria", new { IdPromocion, IdCategoria = idCategoria }, transaccion, commandType: CommandType.StoredProcedure);

                transaccion.Commit();
                return IdPromocion;
            }
            catch
            {
                transaccion.Rollback();
                throw;
            }
        }

        public async Task<int> Desactivar(int IdPromocion)
        {
            using var conexion = _dbContext.CrearConexion();
            return await conexion.ExecuteScalarAsync<int>("DesactivarPromocion", new { IdPromocion }, commandType: CommandType.StoredProcedure);
        }

        private static async Task CargarRelaciones(IDbConnection conexion, List<PromocionResponse> promociones)
        {
            foreach (var promocion in promociones)
            {
                var productos = await conexion.QueryAsync<PromocionProductoResponse>(
                    "ObtenerProductosPromocion", new { promocion.IdPromocion }, commandType: CommandType.StoredProcedure);
                promocion.Productos = productos.ToList();

                var categorias = await conexion.QueryAsync<PromocionCategoriaResponse>(
                    "ObtenerCategoriasPromocion", new { promocion.IdPromocion }, commandType: CommandType.StoredProcedure);
                promocion.Categorias = categorias.ToList();
            }
        }
    }
}
