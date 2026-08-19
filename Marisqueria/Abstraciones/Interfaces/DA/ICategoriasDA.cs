using Abstracciones.Modelos.ModeloProductos;

namespace Abstracciones.Interfaces.DA
{
    public interface ICategoriasDA
    {
        Task<IEnumerable<CategoriaResponse>> Obtener();
        Task<CategoriaResponse?> Obtener(int IdCategoria);
        Task<int> Agregar(CategoriaRequest categoria);
        Task<int> Editar(int IdCategoria, CategoriaRequest categoria);
        Task<int> Eliminar(int IdCategoria);
    }
}
