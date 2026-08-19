using Abstracciones.Modelos.ModeloAutenticacion;

namespace Abstracciones.Interfaces.DA
{
    public interface IRecuperacionPasswordDA
    {
        Task<Guid> Agregar(RecuperacionPasswordRequest recuperacionPassword);
        Task<RecuperacionPasswordResponse> Obtener(string token);
        Task<Guid> MarcarTokenUtilizado(string token);
    }
}