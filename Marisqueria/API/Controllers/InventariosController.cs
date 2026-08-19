using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos.ModeloInventario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventariosController : ControllerBase, IInventariosController
    {
        private readonly IInventariosFlujo _inventariosFlujo;

        public InventariosController(IInventariosFlujo inventariosFlujo)
        {
            _inventariosFlujo = inventariosFlujo;
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _inventariosFlujo.Obtener();
            return resultado.Any() ? Ok(resultado) : NoContent();
        }

        [HttpGet("{IdInventario}")]
        public async Task<IActionResult> Obtener(int IdInventario)
        {
            var resultado = await _inventariosFlujo.Obtener(IdInventario);
            return resultado == null ? NotFound("El registro de inventario no existe.") : Ok(resultado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Agregar(InventarioRequest inventario)
        {
            try
            {
                var resultado = await _inventariosFlujo.Agregar(inventario);
                return CreatedAtAction(nameof(Obtener), new { IdInventario = resultado }, null);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("{IdInventario}")]
        public async Task<IActionResult> Editar(int IdInventario, InventarioRequest inventario)
        {
            if (await _inventariosFlujo.Obtener(IdInventario) == null)
            {
                return NotFound("El registro de inventario no existe.");
            }

            try
            {
                return Ok(await _inventariosFlujo.Editar(IdInventario, inventario));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("{IdInventario}")]
        public async Task<IActionResult> Eliminar(int IdInventario)
        {
            if (await _inventariosFlujo.Obtener(IdInventario) == null)
            {
                return NotFound("El registro de inventario no existe.");
            }

            await _inventariosFlujo.Eliminar(IdInventario);
            return NoContent();
        }

        [HttpGet("producto/{IdProducto}/stock")]
        public async Task<IActionResult> ObtenerStockProducto(int IdProducto)
        {
            var resultado = await _inventariosFlujo.ObtenerStockProducto(IdProducto);

            return Ok(resultado);
        }
    }
}
