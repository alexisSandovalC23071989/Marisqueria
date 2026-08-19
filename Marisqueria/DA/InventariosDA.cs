using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos.ModeloInventario;
using Dapper;
using System.Data;

namespace DA
{
    public class InventariosDA : IInventariosDA
    {
        private readonly DBContext _dbContext;

        public InventariosDA(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<InventarioResponse>> Obtener()
        {
            using var conexion = _dbContext.CrearConexion();
            return await conexion.QueryAsync<InventarioResponse>(
                "ObtenerInventarios",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<InventarioResponse?> Obtener(int IdInventario)
        {
            using var conexion = _dbContext.CrearConexion();
            return await conexion.QueryFirstOrDefaultAsync<InventarioResponse>(
                "ObtenerInventario",
                new { IdInventario },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> Agregar(InventarioRequest inventario)
        {
            using var conexion = _dbContext.CrearConexion();
            return await conexion.ExecuteScalarAsync<int>(
                "AgregarInventario",
                inventario,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> Editar(int IdInventario, InventarioRequest inventario)
        {
            using var conexion = _dbContext.CrearConexion();
            var parametros = new
            {
                IdInventario,
                inventario.IdProducto,
                inventario.IdProveedor,
                inventario.Lote,
                inventario.CantidadDisponible,
                inventario.FechaIngreso,
                inventario.FechaVencimiento,
                inventario.Ubicacion
            };

            return await conexion.ExecuteScalarAsync<int>(
                "EditarInventario",
                parametros,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> Eliminar(int IdInventario)
        {
            using var conexion = _dbContext.CrearConexion();
            return await conexion.ExecuteScalarAsync<int>(
                "EliminarInventario",
                new { IdInventario },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<StockProductoResponse> ObtenerStockProducto(int idProducto)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.QueryFirstAsync<StockProductoResponse>("ObtenerStockProducto",
                new
                {
                    IdProducto = idProducto
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> DescontarStock(int idProducto, int cantidad)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.ExecuteScalarAsync<int>("DescontarStockProducto", new
                {
                    IdProducto = idProducto,
                    Cantidad = cantidad
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> DevolverStock(int idProducto, int cantidad)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.ExecuteScalarAsync<int>("DevolverStockProducto", new
                {
                    IdProducto = idProducto,
                    Cantidad = cantidad
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}
