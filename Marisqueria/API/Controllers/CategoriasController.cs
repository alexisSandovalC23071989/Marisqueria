using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos.ModeloProductos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase, ICategoriasController
    {
        private readonly ICategoriasFlujo _categoriasFlujo;

        public CategoriasController(ICategoriasFlujo categoriasFlujo)
        {
            _categoriasFlujo = categoriasFlujo;
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _categoriasFlujo.Obtener();
            return resultado.Any() ? Ok(resultado) : NoContent();
        }

        [HttpGet("{IdCategoria}")]
        public async Task<IActionResult> Obtener(int IdCategoria)
        {
            var resultado = await _categoriasFlujo.Obtener(IdCategoria);
            return resultado == null ? NotFound("La categoría no existe.") : Ok(resultado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Agregar(CategoriaRequest categoria)
        {
            var resultado = await _categoriasFlujo.Agregar(categoria);
            return CreatedAtAction(nameof(Obtener), new { IdCategoria = resultado }, null);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("{IdCategoria}")]
        public async Task<IActionResult> Editar(int IdCategoria, CategoriaRequest categoria)
        {
            if (await _categoriasFlujo.Obtener(IdCategoria) == null)
            {
                return NotFound("La categoría no existe.");
            }

            return Ok(await _categoriasFlujo.Editar(IdCategoria, categoria));
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("{IdCategoria}")]
        public async Task<IActionResult> Eliminar(int IdCategoria)
        {
            if (await _categoriasFlujo.Obtener(IdCategoria) == null)
            {
                return NotFound("La categoría no existe.");
            }

            var resultado = await _categoriasFlujo.Eliminar(IdCategoria);
            if (resultado == 0)
            {
                return Conflict("No se puede eliminar la categoría porque tiene productos asociados.");
            }

            return NoContent();
        }
    }
}
