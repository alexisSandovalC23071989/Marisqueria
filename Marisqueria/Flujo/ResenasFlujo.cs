using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos.ModeloResenas;

namespace Flujo
{
    public class ResenasFlujo : IResenasFlujo
    {
        private readonly IResenasDA _resenasDA;

        public ResenasFlujo(IResenasDA resenasDA)
        {
            _resenasDA = resenasDA;
        }

        public Task<IEnumerable<ResenaResponse>> ObtenerPorProducto(int idProducto)
        {
            return _resenasDA.ObtenerPorProducto(idProducto);
        }

        public Task<ResenaResponse?> Obtener(int idResena)
        {
            return _resenasDA.Obtener(idResena);
        }

        public Task<int> Agregar(ResenaRequest resena)
        {
            return _resenasDA.Agregar(resena);
        }

        public Task<int> Editar(int idResena, ResenaRequest resena)
        {
            return _resenasDA.Editar(idResena, resena);
        }

        public Task<int> Eliminar(int idResena)
        {
            return _resenasDA.Eliminar(idResena);
        }
    }
}