using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos.ModeloProductos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductoRequest = Abstracciones.Modelos.ModeloProductos.ProductoRequest;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase, IProductosController
    {
        private readonly IProductosFlujo _productosFlujo;
        private readonly ILogger<ProductosController> _logger;

        public ProductosController(IProductosFlujo productosFlujo, ILogger<ProductosController> logger)
        {
            _productosFlujo = productosFlujo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _productosFlujo.Obtener();
            if (!resultado.Any())
            {
                return NoContent();
            }

            return Ok(resultado);
        }

        [HttpGet("{IdProducto}")]
        public async Task<IActionResult> Obtener(int IdProducto)
        {
            var resultado = await _productosFlujo.Obtener(IdProducto);
            if (resultado == null)
            {
                return NotFound("El producto no existe.");
            }

            return Ok(resultado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Agregar(ProductoRequest producto)
        {
            var resultado = await _productosFlujo.Agregar(producto);
            return CreatedAtAction(nameof(Obtener), new { IdProducto = resultado }, null);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("{IdProducto}")]
        public async Task<IActionResult> Editar(int IdProducto, ProductoRequest producto)
        {
            if (!await VerificarExistenciaProducto(IdProducto))
            {
                return NotFound("El producto no existe.");
            }

            var resultado = await _productosFlujo.Editar(IdProducto, producto);
            return Ok(resultado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("{IdProducto}")]
        public async Task<IActionResult> Eliminar(int IdProducto)
        {
            if (!await VerificarExistenciaProducto(IdProducto))
            {
                return NotFound("El producto no existe.");
            }

            await _productosFlujo.Eliminar(IdProducto);
            return NoContent();
        }

        private async Task<bool> VerificarExistenciaProducto(int IdProducto)
        {
            var resultadoProductoExiste = await _productosFlujo.Obtener(IdProducto);
            return resultadoProductoExiste != null;
        }
    }
}

