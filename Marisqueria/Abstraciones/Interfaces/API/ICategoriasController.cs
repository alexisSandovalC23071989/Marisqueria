using Abstracciones.Modelos.ModeloProductos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface ICategoriasController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> Obtener(int IdCategoria);
        Task<IActionResult> Agregar(CategoriaRequest categoria);
        Task<IActionResult> Editar(int IdCategoria, CategoriaRequest categoria);
        Task<IActionResult> Eliminar(int IdCategoria);
    }
}
