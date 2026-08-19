using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos.ModeloPedido;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallePedidoController : ControllerBase, IDetallePedidoController
    {
        private readonly IDetallePedidoFlujo _detallePedidoFlujo;
        private readonly ILogger<DetallePedidoController> _logger;

        public DetallePedidoController(
            IDetallePedidoFlujo detallePedidoFlujo,
            ILogger<DetallePedidoController> logger)
        {
            _detallePedidoFlujo = detallePedidoFlujo;
            _logger = logger;
        }

        [HttpGet("{IdPedido}")]
        public async Task<IActionResult> ObtenerPorPedido(Guid IdPedido)
        {
            var resultado = await _detallePedidoFlujo.ObtenerPorPedido(IdPedido);

            if (!resultado.Any())
                return NoContent();

            return Ok(resultado);
        }

        [HttpGet("detalle/{IdDetalle}")]
        public async Task<IActionResult> ObtenerDetalle(Guid IdDetalle)
        {
            var resultado = await _detallePedidoFlujo.Obtener(IdDetalle);

            if (resultado == null)
                return NotFound("El detalle no existe.");

            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(DetallePedidoRequest detalle)
        {
            var resultado = await _detallePedidoFlujo.Agregar(detalle);

            return CreatedAtAction(
                nameof(ObtenerDetalle),
                new { IdDetalle = resultado },
                null);
        }

        [HttpPut("{IdDetalle}")]
        public async Task<IActionResult> Editar(Guid IdDetalle, DetallePedidoRequest detalle)
        {
            if (!await VerificarExistenciaDetalle(IdDetalle))
                return NotFound("El detalle no existe.");

            var resultado = await _detallePedidoFlujo.Editar(IdDetalle, detalle);

            return Ok(resultado);
        }

        [HttpDelete("{IdDetalle}")]
        public async Task<IActionResult> Eliminar(Guid IdDetalle)
        {
            if (!await VerificarExistenciaDetalle(IdDetalle))
                return NotFound("El detalle no existe.");

            await _detallePedidoFlujo.Eliminar(IdDetalle);

            return NoContent();
        }

        private async Task<bool> VerificarExistenciaDetalle(Guid IdDetalle)
        {
            var resultado = await _detallePedidoFlujo.Obtener(IdDetalle);
            return resultado != null;
        }

    }
}