using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos.ModeloResenas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResenasController : ControllerBase, IResenasController
    {
        private readonly IResenasFlujo _resenasFlujo;
        private readonly ILogger<ResenasController> _logger;

        public ResenasController(IResenasFlujo resenasFlujo, ILogger<ResenasController> logger)
        {
            _resenasFlujo = resenasFlujo;
            _logger = logger;
        }

        [HttpGet("Producto/{idProducto}")]
        public async Task<IActionResult> ObtenerPorProducto(int idProducto)
        {
            var resultado = await _resenasFlujo.ObtenerPorProducto(idProducto);

            if (!resultado.Any())
            {
                return NoContent();
            }

            return Ok(resultado);
        }

        [HttpGet("{idResena}")]
        public async Task<IActionResult> Obtener(int idResena)
        {
            var resultado = await _resenasFlujo.Obtener(idResena);

            if (resultado == null)
            {
                return NotFound("La reseña no existe.");
            }

            return Ok(resultado);
        }


        [HttpPost]
        public async Task<IActionResult> Agregar(ResenaRequest resena)
        {
            var resultado = await _resenasFlujo.Agregar(resena);

            return CreatedAtAction(nameof(Obtener), new { idResena = resultado }, null);
        }


        [HttpPut("{idResena}")]
        public async Task<IActionResult> Editar(int idResena, ResenaRequest resena)
        {
            if (!await ExisteResena(idResena))
            {
                return NotFound("La reseña no existe.");
            }

            var resultado = await _resenasFlujo.Editar(idResena, resena);

            return Ok(resultado);
        }

        [HttpDelete("{idResena}")]
        public async Task<IActionResult> Eliminar(int idResena)
        {
            if (!await ExisteResena(idResena))
            {
                return NotFound("La reseña no existe.");
            }

            await _resenasFlujo.Eliminar(idResena);

            return NoContent();
        }

        private async Task<bool> ExisteResena(int idResena)
        {
            var resultado = await _resenasFlujo.Obtener(idResena);

            return resultado != null;
        }
    }
}