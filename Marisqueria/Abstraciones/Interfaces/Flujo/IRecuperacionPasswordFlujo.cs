using Abstracciones.Modelos.ModeloAutenticacion;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IRecuperacionPasswordFlujo
    {
        Task<Guid> Agregar(RecuperacionPasswordRequest recuperacionPassword);
        Task<RecuperacionPasswordResponse> Obtener(string token);
        Task<Guid> MarcarTokenUtilizado(string token);
    }
}