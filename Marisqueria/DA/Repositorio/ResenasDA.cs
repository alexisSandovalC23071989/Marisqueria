using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos.ModeloResenas;
using Dapper;
using System.Data;

namespace DA
{
    public class ResenasDA : IResenasDA
    {
        private readonly DBContext _dbContext;

        public ResenasDA(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ResenaResponse>> ObtenerPorProducto(int idProducto)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.QueryAsync<ResenaResponse>(
                "ObtenerResenasPorProducto",
                new { id_producto = idProducto },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<ResenaResponse?> Obtener(int idResena)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.QueryFirstOrDefaultAsync<ResenaResponse>(
                "ObtenerResena",
                new { id_resena = idResena },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> Agregar(ResenaRequest resena)
        {
            using var conexion = _dbContext.CrearConexion();

            var baseDatos = await conexion.QuerySingleAsync<string>(
                "SELECT DB_NAME()");

            var usuarioExiste = await conexion.QuerySingleAsync<int>(
                "SELECT COUNT(*) FROM USUARIO WHERE Id = @id_usuario",
                new { id_usuario = resena.IdUsuario });

          

            return await conexion.ExecuteScalarAsync<int>(
                "AgregarResena",
                new
                {
                    id_usuario = resena.IdUsuario,
                    id_producto = resena.IdProducto,
                    calificacion = resena.Calificacion,
                    comentario = resena.Comentario
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> Editar(int idResena, ResenaRequest resena)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.ExecuteScalarAsync<int>(
                "EditarResena",
                new
                {
                    id_resena = idResena,
                    id_usuario = resena.IdUsuario,
                    id_producto = resena.IdProducto,
                    calificacion = resena.Calificacion,
                    comentario = resena.Comentario
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> Eliminar(int idResena)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.ExecuteScalarAsync<int>(
                "EliminarResena",
                new
                {
                    id_resena = idResena
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}