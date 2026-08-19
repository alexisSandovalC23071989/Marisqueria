using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos.ModeloPedido;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase, IPedidoController
    {
        private readonly IPedidoFlujo _pedidoFlujo;
        private readonly ILogger<PedidoController> _logger;

        public PedidoController(IPedidoFlujo pedidoFlujo,
                                ILogger<PedidoController> logger)
        {
            _pedidoFlujo = pedidoFlujo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _pedidoFlujo.Obtener();

            if (!resultado.Any())
                return NoContent();

            return Ok(resultado);
        }

        [HttpGet("{IdPedido}")]
        public async Task<IActionResult> Obtener(Guid IdPedido)
        {
            var resultado = await _pedidoFlujo.Obtener(IdPedido);

            if (resultado == null)
                return NotFound("El pedido no existe.");

            return Ok(resultado);
        }

        [HttpGet("usuario/{IdUsuario}")]
        public async Task<IActionResult> ObtenerPorUsuario(Guid IdUsuario)
        {
            var resultado = await _pedidoFlujo.ObtenerPorUsuario(IdUsuario);

            if (!resultado.Any())
                return NoContent();

            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(PedidoRequest pedido)
        {
            var resultado = await _pedidoFlujo.Agregar(pedido);

            return CreatedAtAction(nameof(Obtener),
                new { IdPedido = resultado },
                resultado);
        }

        [HttpPut("{IdPedido}")]
        public async Task<IActionResult> Editar(Guid IdPedido, PedidoRequest pedido)
        {
            if (!await VerificarExistenciaPedido(IdPedido))
                return NotFound("El pedido no existe.");

            var resultado = await _pedidoFlujo.Editar(IdPedido, pedido);

            return Ok(resultado);
        }

        [HttpDelete("{IdPedido}")]
        public async Task<IActionResult> Eliminar(Guid IdPedido)
        {
            var pedido = await _pedidoFlujo.Obtener(IdPedido);

            if (pedido == null)
            {
                return NotFound("El pedido no existe.");
            }

            if (pedido.Estado != "Pendiente")
            {
                return BadRequest(
                    "Solo se pueden eliminar pedidos en estado Pendiente.");
            }

            await _pedidoFlujo.Eliminar(IdPedido);

            return NoContent();
        }

        private async Task<bool> VerificarExistenciaPedido(Guid IdPedido)
        {
            var resultado = await _pedidoFlujo.Obtener(IdPedido);
            return resultado != null;
        }

        [HttpPut("{IdPedido}/confirmar")]
        public async Task<IActionResult> Confirmar(Guid IdPedido)
        {
            if (!await VerificarExistenciaPedido(IdPedido))
                return NotFound("El pedido no existe.");

            await _pedidoFlujo.Confirmar(IdPedido);

            return Ok();
        }

        [HttpPut("{IdPedido}/cancelar")]
        public async Task<IActionResult> Cancelar(Guid IdPedido)
        {
            if (!await VerificarExistenciaPedido(IdPedido))
                return NotFound("El pedido no existe.");

            await _pedidoFlujo.Cancelar(IdPedido);

            return Ok();
        }

        [HttpPut("{IdPedido}/preparar")]
        public async Task<IActionResult> Preparar(Guid IdPedido)
        {
            if (!await VerificarExistenciaPedido(IdPedido))
                return NotFound("El pedido no existe.");

            await _pedidoFlujo.Preparar(IdPedido);

            return Ok();
        }

        [HttpPut("{IdPedido}/entregar")]
        public async Task<IActionResult> Entregar(Guid IdPedido)
        {
            if (!await VerificarExistenciaPedido(IdPedido))
                return NotFound("El pedido no existe.");

            await _pedidoFlujo.Entregar(IdPedido);

            return Ok();
        }

        [HttpGet("activo/{IdUsuario}")]
        public async Task<IActionResult> ObtenerPedidoActivo(Guid IdUsuario)
        {
            var resultado = await _pedidoFlujo.ObtenerPedidoActivo(IdUsuario);

            if (resultado == null)
                return NoContent();

            return Ok(resultado);
        }

    }
}