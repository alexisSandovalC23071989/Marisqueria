using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedoresController : ControllerBase, IProveedoresController
    {
        private readonly IProveedoresFlujo _proveedoresFlujo;
        private readonly ILogger<ProveedoresController> _logger;

        public ProveedoresController(IProveedoresFlujo proveedoresFlujo, ILogger<ProveedoresController> logger)
        {
            _proveedoresFlujo = proveedoresFlujo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _proveedoresFlujo.Obtener();
            if (!resultado.Any())
            {
                return NoContent();
            }

            return Ok(resultado);
        }

        [HttpGet("{IdProveedor}")]
        public async Task<IActionResult> Obtener(int IdProveedor)
        {
            var resultado = await _proveedoresFlujo.Obtener(IdProveedor);
            if (resultado == null)
            {
                return NotFound("El proveedor no existe.");
            }

            return Ok(resultado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Agregar(ProveedorRequest proveedor)
        {
            var resultado = await _proveedoresFlujo.Agregar(proveedor);
            return CreatedAtAction(nameof(Obtener), new { IdProveedor = resultado }, null);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("{IdProveedor}")]
        public async Task<IActionResult> Editar(int IdProveedor, ProveedorRequest proveedor)
        {
            if (!await VerificarExistenciaProveedor(IdProveedor))
            {
                return NotFound("El proveedor no existe.");
            }

            var resultado = await _proveedoresFlujo.Editar(IdProveedor, proveedor);
            return Ok(resultado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("{IdProveedor}")]
        public async Task<IActionResult> Eliminar(int IdProveedor)
        {
            if (!await VerificarExistenciaProveedor(IdProveedor))
            {
                return NotFound("El proveedor no existe.");
            }

            await _proveedoresFlujo.Eliminar(IdProveedor);
            return NoContent();
        }

        [HttpGet("{IdProveedor}/productos")]
        public async Task<IActionResult> ObtenerProductos(int IdProveedor)
        {
            if (!await VerificarExistenciaProveedor(IdProveedor))
            {
                return NotFound("El proveedor no existe.");
            }

            var resultado = await _proveedoresFlujo.ObtenerProductos(IdProveedor);
            if (!resultado.Any())
            {
                return NoContent();
            }

            return Ok(resultado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost("{IdProveedor}/productos")]
        public async Task<IActionResult> AgregarProducto(int IdProveedor, ProductoProveedorRequest productoProveedor)
        {
            if (!await VerificarExistenciaProveedor(IdProveedor))
            {
                return NotFound("El proveedor no existe.");
            }

            var resultado = await _proveedoresFlujo.AgregarProducto(IdProveedor, productoProveedor);
            return CreatedAtAction(nameof(ObtenerProductos), new { IdProveedor }, null);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("{IdProveedor}/productos/{IdProducto}")]
        public async Task<IActionResult> EditarProducto(int IdProveedor, int IdProducto, ProductoProveedorRequest productoProveedor)
        {
            if (!await VerificarExistenciaProveedor(IdProveedor))
            {
                return NotFound("El proveedor no existe.");
            }

            productoProveedor.IdProducto = IdProducto;
            var resultado = await _proveedoresFlujo.EditarProducto(IdProveedor, productoProveedor);
            return Ok(resultado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("{IdProveedor}/productos/{IdProducto}")]
        public async Task<IActionResult> EliminarProducto(int IdProveedor, int IdProducto)
        {
            if (!await VerificarExistenciaProveedor(IdProveedor))
            {
                return NotFound("El proveedor no existe.");
            }

            await _proveedoresFlujo.EliminarProducto(IdProveedor, IdProducto);
            return NoContent();
        }

        private async Task<bool> VerificarExistenciaProveedor(int IdProveedor)
        {
            var resultadoProveedorExiste = await _proveedoresFlujo.Obtener(IdProveedor);
            return resultadoProveedorExiste != null;
        }
    }
}
