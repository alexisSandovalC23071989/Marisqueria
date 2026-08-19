using System;
using Abstracciones.Modelos.ModeloResenas;

namespace Abstracciones.Interfaces.DA
{
    public interface IResenasDA
    {
        Task<IEnumerable<ResenaResponse>> ObtenerPorProducto(int idProducto);

        Task<ResenaResponse?> Obtener(int idResena);

        Task<int> Agregar(ResenaRequest resena);

        Task<int> Editar(int idResena, ResenaRequest resena);

        Task<int> Eliminar(int idResena);
    }
}