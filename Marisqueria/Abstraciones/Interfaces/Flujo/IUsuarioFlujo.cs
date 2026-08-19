using Abstracciones.Modelos.ModeloAutenticacion;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IUsuarioFlujo
    {
        Task<IEnumerable<UsuarioResponse>> Obtener();

        Task<UsuarioResponse?> Obtener(Guid Id);

        Task<Guid> Agregar(UsuarioRequest usuario);

        Task<Guid> Editar(Guid Id, UsuarioEditarRequest usuario);

        Task<Guid> Eliminar(Guid Id);

        Task<LoginResponse?> Login(LoginRequest login);

        Task<Guid> SolicitarRecuperacion(SolicitarRecuperacionRequest solicitud);

        Task CambiarPassword(CambiarPasswordRequest solicitud);

    }
}