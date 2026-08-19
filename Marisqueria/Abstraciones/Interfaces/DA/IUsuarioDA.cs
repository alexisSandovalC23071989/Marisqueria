using Abstracciones.Modelos.ModeloAutenticacion;

namespace Abstracciones.Interfaces.DA
{
    public interface IUsuarioDA
    {
        Task<IEnumerable<UsuarioResponse>> Obtener();

        Task<UsuarioResponse?> Obtener(Guid id);

        Task<LoginUsuarioResponse?> ObtenerPorCorreo(string correo);

        Task<Guid> Agregar(UsuarioDARequest usuario);

        Task<Guid> Editar(Guid Id,UsuarioEditarRequest usuario);

        Task<Guid> Eliminar(Guid id);

        Task<Guid> AgregarRecuperacionPassword(RecuperacionPasswordRequest recuperacion);

        Task<RecuperacionPasswordResponse?> ObtenerRecuperacionPorToken(string token);

        Task CambiarPassword(Guid idUsuario, string contrasenaHash);

        Task MarcarRecuperacionUtilizada(Guid id);

    }
}