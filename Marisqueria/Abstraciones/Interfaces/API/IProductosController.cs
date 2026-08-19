using Abstracciones.Modelos.ModeloProductos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IProductosController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> Obtener(int IdProducto);
        Task<IActionResult> Agregar(ProductoRequest producto);
        Task<IActionResult> Editar(int IdProducto, ProductoRequest producto);
        Task<IActionResult> Eliminar(int IdProducto);
    }
}

