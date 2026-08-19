using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos.ModeloProductos;
using Dapper;
using System.Data;

namespace DA
{
    public class CategoriasDA : ICategoriasDA
    {
        private readonly DBContext _dbContext;

        public CategoriasDA(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<CategoriaResponse>> Obtener()
        {
            using var conexion = _dbContext.CrearConexion();
            return await conexion.QueryAsync<CategoriaResponse>(
                "ObtenerCategorias",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<CategoriaResponse?> Obtener(int IdCategoria)
        {
            using var conexion = _dbContext.CrearConexion();
            return await conexion.QueryFirstOrDefaultAsync<CategoriaResponse>(
                "ObtenerCategoria",
                new { IdCategoria },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> Agregar(CategoriaRequest categoria)
        {
            using var conexion = _dbContext.CrearConexion();
            return await conexion.ExecuteScalarAsync<int>(
                "AgregarCategoria",
                categoria,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> Editar(int IdCategoria, CategoriaRequest categoria)
        {
            using var conexion = _dbContext.CrearConexion();
            return await conexion.ExecuteScalarAsync<int>(
                "EditarCategoria",
                new { IdCategoria, categoria.Nombre },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> Eliminar(int IdCategoria)
        {
            using var conexion = _dbContext.CrearConexion();
            return await conexion.ExecuteScalarAsync<int>(
                "EliminarCategoria",
                new { IdCategoria },
                commandType: CommandType.StoredProcedure);
        }
    }
}
