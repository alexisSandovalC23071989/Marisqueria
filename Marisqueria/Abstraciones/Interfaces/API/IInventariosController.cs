using Abstracciones.Modelos.ModeloInventario;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IInventariosController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> Obtener(int IdInventario);
        Task<IActionResult> Agregar(InventarioRequest inventario);
        Task<IActionResult> Editar(int IdInventario, InventarioRequest inventario);
        Task<IActionResult> Eliminar(int IdInventario);
    }
}
