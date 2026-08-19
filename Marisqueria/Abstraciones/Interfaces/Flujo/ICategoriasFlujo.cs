using Abstracciones.Modelos.ModeloProductos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface ICategoriasFlujo
    {
        Task<IEnumerable<CategoriaResponse>> Obtener();
        Task<CategoriaResponse?> Obtener(int IdCategoria);
        Task<int> Agregar(CategoriaRequest categoria);
        Task<int> Editar(int IdCategoria, CategoriaRequest categoria);
        Task<int> Eliminar(int IdCategoria);
    }
}
