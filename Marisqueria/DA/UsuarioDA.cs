using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos.ModeloAutenticacion;
using Dapper;
using System.Data;
namespace DA
{
    public class UsuarioDA : IUsuarioDA
    {
        private readonly DBContext _dbContext;

        public UsuarioDA(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<UsuarioResponse>> Obtener()
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.QueryAsync<UsuarioResponse>(
                "ObtenerUsuarios",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<UsuarioResponse> Obtener(Guid id)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.QueryFirstOrDefaultAsync<UsuarioResponse>(
                "ObtenerUsuario",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Guid> Agregar(UsuarioDARequest usuario)
        {
            using var conexion = _dbContext.CrearConexion();

            var parametros = new
            {
                usuario.Nombre,
                usuario.Correo,
                usuario.ContrasenaHash,
                usuario.Telefono
            };

            return await conexion.ExecuteScalarAsync<Guid>(
                "AgregarUsuario",
                parametros,
                commandType: CommandType.StoredProcedure);
        }

          public async Task<Guid> Editar(Guid id, UsuarioEditarRequest usuario)

        {
            using var conexion = _dbContext.CrearConexion();

            var parametros = new
            {
                Id = id,
                usuario.IdRol,
                usuario.Nombre,
                usuario.Correo,
                usuario.Telefono
            };

            await conexion.ExecuteAsync(
                "EditarUsuario",
                parametros,
                commandType: CommandType.StoredProcedure);

            return id;
        }
        public async Task<Guid> Eliminar(Guid id)
        {
            using var conexion = _dbContext.CrearConexion();

            await conexion.ExecuteAsync(
                "EliminarUsuario",
                new { Id = id },
                commandType: CommandType.StoredProcedure);

            return id;
        }
        public async Task<LoginUsuarioResponse?> ObtenerPorCorreo(string correo)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.QueryFirstOrDefaultAsync<LoginUsuarioResponse>(
                "ObtenerUsuarioPorCorreo",
                new { Correo = correo },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Guid> AgregarRecuperacionPassword(RecuperacionPasswordRequest recuperacion)

        {
            using var conexion = _dbContext.CrearConexion();

            var parametros = new
            {
                recuperacion.IdUsuario,
                recuperacion.Token,
                recuperacion.FechaCreacion,
                recuperacion.FechaExpiracion,
                recuperacion.Utilizado
            };

            return await conexion.ExecuteScalarAsync<Guid>(
                "AgregarRecuperacionPassword",
                parametros,
                commandType: CommandType.StoredProcedure);
        }
        public async Task<RecuperacionPasswordResponse?> ObtenerRecuperacionPorToken(string token)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.QueryFirstOrDefaultAsync<RecuperacionPasswordResponse>(
                "ObtenerRecuperacionPorToken",
                new
                {
                    Token = token
                },
                commandType: CommandType.StoredProcedure);
        }
        public async Task CambiarPassword(Guid idUsuario, string contrasenaHash)

        {
            using var conexion = _dbContext.CrearConexion();

            await conexion.ExecuteAsync(
                "CambiarPassword",
                new
                {
                    IdUsuario = idUsuario,
                    ContrasenaHash = contrasenaHash
                },
                commandType: CommandType.StoredProcedure);
        }
        public async Task MarcarRecuperacionUtilizada(Guid id)
        {
            using var conexion = _dbContext.CrearConexion();

            await conexion.ExecuteAsync(
                "MarcarRecuperacionUtilizada",
                new
                {
                    Id = id
                },
                commandType: CommandType.StoredProcedure);
        }


    }
}