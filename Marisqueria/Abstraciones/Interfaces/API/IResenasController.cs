using Abstracciones.Modelos.ModeloResenas;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IResenasController
    {
        Task<IActionResult> ObtenerPorProducto(int idProducto);

        Task<IActionResult> Obtener(int idResena);

        Task<IActionResult> Agregar(ResenaRequest resena);

        Task<IActionResult> Editar(int idResena, ResenaRequest resena);

        Task<IActionResult> Eliminar(int idResena);
    }
}