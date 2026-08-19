using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos.ModeloPromociones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromocionesController : ControllerBase, IPromocionesController
    {
        private readonly IPromocionesFlujo _promocionesFlujo;
        public PromocionesController(IPromocionesFlujo promocionesFlujo) => _promocionesFlujo = promocionesFlujo;

        [HttpGet]
        public async Task<IActionResult> Obtener([FromQuery] string? estado = null)
        {
            var resultado = await _promocionesFlujo.Obtener(estado);
            return resultado.Any() ? Ok(resultado) : NoContent();
        }

        [HttpGet("activas")]
        public async Task<IActionResult> ObtenerActivas()
        {
            var resultado = await _promocionesFlujo.ObtenerActivas();
            return resultado.Any() ? Ok(resultado) : NoContent();
        }

        [HttpGet("producto/{IdProducto}/precio")]
        public async Task<IActionResult> ObtenerPrecioProducto(int IdProducto)
        {
            var resultado = await _promocionesFlujo.ObtenerPrecioProducto(IdProducto);
            return resultado == null ? NoContent() : Ok(resultado);
        }

        [HttpGet("alertas")]
        public async Task<IActionResult> ObtenerAlertas([FromQuery] int diasAnticipacion = 3, [FromQuery] bool soloPendientes = true)
        {
            try
            {
                var resultado = await _promocionesFlujo.ObtenerAlertas(diasAnticipacion, soloPendientes);
                return resultado.Any() ? Ok(resultado) : NoContent();
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }

        [Authorize(Roles = "Administrador")]
        [HttpPatch("alertas/{IdAlerta}/revisar")]
        public async Task<IActionResult> MarcarAlertaRevisada(int IdAlerta)
        {
            var filas = await _promocionesFlujo.MarcarAlertaRevisada(IdAlerta);
            return filas == 0 ? NotFound("La alerta no existe.") : NoContent();
        }

        [HttpGet("{IdPromocion}")]
        public async Task<IActionResult> Obtener(int IdPromocion)
        {
            var resultado = await _promocionesFlujo.Obtener(IdPromocion);
            return resultado == null ? NotFound("La promoción no existe.") : Ok(resultado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Agregar(PromocionRequest promocion)
        {
            try
            {
                var id = await _promocionesFlujo.Agregar(promocion);
                return CreatedAtAction(nameof(Obtener), new { IdPromocion = id }, null);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("{IdPromocion}")]
        public async Task<IActionResult> Editar(int IdPromocion, PromocionRequest promocion)
        {
            if (await _promocionesFlujo.Obtener(IdPromocion) == null) return NotFound("La promoción no existe.");
            try
            {
                await _promocionesFlujo.Editar(IdPromocion, promocion);
                return Ok(IdPromocion);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }

        [Authorize(Roles = "Administrador")]
        [HttpPatch("{IdPromocion}/desactivar")]
        public async Task<IActionResult> Desactivar(int IdPromocion)
        {
            if (await _promocionesFlujo.Obtener(IdPromocion) == null) return NotFound("La promoción no existe.");
            await _promocionesFlujo.Desactivar(IdPromocion);
            return NoContent();
        }
    }
}
