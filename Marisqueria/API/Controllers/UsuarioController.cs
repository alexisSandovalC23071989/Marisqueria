using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Abstracciones.Modelos.ModeloAutenticacion;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Interfaces.API;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase, IUsuarioController
    {
        private readonly IUsuarioFlujo _usuarioFlujo;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(IUsuarioFlujo usuarioFlujo, ILogger<UsuarioController> logger)
        {
            _usuarioFlujo = usuarioFlujo;
            _logger = logger;
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest login)
        {   
            var resultado = await _usuarioFlujo.Login(login);
            //validacion de correo y contraseña 
            if (resultado == null)
            {
                return Unauthorized("Correo o contraseña incorrectos.");
            }

            return Ok(resultado);///200, 401 no autenticado
        }

        [AllowAnonymous]
        [HttpPost("AgregarUsuario")]
        public async Task<IActionResult> Agregar(UsuarioRequest usuario)
        {
            var resultado = await _usuarioFlujo.Agregar(usuario);
            return CreatedAtAction(nameof(Obtener), new { Id = resultado }, null);
        }
        //201 cotenido creado

        [Authorize(Roles = "Administrador")]
        [HttpPut("EditarUsuario/{Id}")]
        public async Task<IActionResult> Editar(
        Guid Id,
        UsuarioEditarRequest usuario)
        {
            var usuarioExiste = await _usuarioFlujo.Obtener(Id);

            if (usuarioExiste == null)
            {
                return NotFound("El usuario no existe.");
            }

            var resultado = await _usuarioFlujo.Editar(Id, usuario);

            return Ok(resultado); // 201 editado correctamente.
                                  //404 no existe contenido.
                                  //401 no autenticado.
        }
        [Authorize(Roles = "Administrador")]
        [HttpDelete("EliminarUsuario/{Id}")]
        public async Task<IActionResult> Eliminar(Guid Id) //204  eliminado correctamente
                                                          // 401 falta de autorizacion
        {
            await _usuarioFlujo.Eliminar(Id);
            return NoContent();
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("ObtenerUsuarios")]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _usuarioFlujo.Obtener();

            if (!resultado.Any())
            {
                return NoContent();
            }

            return Ok(resultado);// 200 ok se realizo correctamente.
                                 // 204 no contenido
                                 // 401 sin autenticacion
                                 //403, sin rol adm
        }
        [Authorize(Roles = "Administrador")]
        [HttpGet("ObtenerUsuario/{Id}")]
        public async Task<IActionResult> Obtener(Guid Id)
        {
            var resultado = await _usuarioFlujo.Obtener(Id);
            return Ok(resultado); //200 correctemente
                                  // 401 sin autenticacion
        }
        [AllowAnonymous] // Este metodo debe poder usarse sin el token para recuperar la contraseña
        [HttpPost("SolicitarRecuperacion")]
        public async Task<IActionResult> SolicitarRecuperacion(SolicitarRecuperacionRequest solicitud)

        {
            try
            {
                var resultado =
                    await _usuarioFlujo.SolicitarRecuperacion(solicitud);

                return Ok(new
                {
                    mensaje = "Se generó la solicitud de recuperación.",
                    idRecuperacion = resultado
                });
            }
            catch (Exception ex)
            {                           // 400 no existe el correo
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("CambiarPassword")]
        public async Task<IActionResult> CambiarPassword(CambiarPasswordRequest solicitud)

        {
            try
            {
                await _usuarioFlujo.CambiarPassword(solicitud);

                return Ok(new
                {
                    mensaje = "La contraseña fue actualizada correctamente."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}