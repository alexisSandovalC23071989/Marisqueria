using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos.ModeloProductos;

namespace Flujo
{
    public class CategoriasFlujo : ICategoriasFlujo
    {
        private readonly ICategoriasDA _categoriasDA;

        public CategoriasFlujo(ICategoriasDA categoriasDA)
        {
            _categoriasDA = categoriasDA;
        }

        public Task<IEnumerable<CategoriaResponse>> Obtener() => _categoriasDA.Obtener();
        public Task<CategoriaResponse?> Obtener(int IdCategoria) => _categoriasDA.Obtener(IdCategoria);
        public Task<int> Agregar(CategoriaRequest categoria) => _categoriasDA.Agregar(categoria);
        public Task<int> Editar(int IdCategoria, CategoriaRequest categoria) => _categoriasDA.Editar(IdCategoria, categoria);
        public Task<int> Eliminar(int IdCategoria) => _categoriasDA.Eliminar(IdCategoria);
    }
}
