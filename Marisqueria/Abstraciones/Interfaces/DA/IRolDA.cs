using Abstracciones.Modelos.ModeloAutenticacion;

namespace Abstracciones.Interfaces.DA
{
    public interface IRolDA
    {
        Task<IEnumerable<RolResponse>> Obtener();
        Task<RolResponse> Obtener(Guid id);
        Task<Guid> Agregar(RolRequest rol);
        Task<Guid> Editar(Guid id, RolRequest rol);
        Task<Guid> Eliminar(Guid id);
    }
}