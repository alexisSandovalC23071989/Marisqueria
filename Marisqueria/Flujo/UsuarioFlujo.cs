using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos.ModeloAutenticacion;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Abstracciones.Interfaces.Servicios;


namespace Flujo
{
    public class UsuarioFlujo : IUsuarioFlujo
    {
        private readonly IUsuarioDA _usuarioDA;
        private readonly IConfiguration _configuration;
        private readonly ICorreoServicio _correoServicio;

        public UsuarioFlujo(
            IUsuarioDA usuarioDA,
            IConfiguration configuration,
            ICorreoServicio correoServicio)
        {
            _usuarioDA = usuarioDA;
            _configuration = configuration;
            _correoServicio = correoServicio;
        }

        private string GenerarToken(LoginUsuarioResponse usuario)
        {
            var claims = new[]
            {
             new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
             new Claim(ClaimTypes.Name, usuario.Nombre),
             new Claim(ClaimTypes.Email, usuario.Correo),
              new Claim(ClaimTypes.Role, usuario.Rol)
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            var credenciales = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])
                ),
                signingCredentials: credenciales
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<LoginResponse?> Login(LoginRequest login)
        {
            var usuario = await _usuarioDA.ObtenerPorCorreo(login.Correo);

            if (usuario == null)
            {
                return null;
            }

            var contrasenaCorrecta = BCrypt.Net.BCrypt.Verify(
                login.Contrasena,
                usuario.ContrasenaHash
            );

            if (!contrasenaCorrecta)
            {
                return null;
            }

            var token = GenerarToken(usuario);

            return new LoginResponse
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                IdRol = usuario.IdRol,
                Rol = usuario.Rol,
                Token = token
            };
        }
        public async Task<IEnumerable<UsuarioResponse>> Obtener()
        {
            return await _usuarioDA.Obtener();
        }

        public async Task<UsuarioResponse> Obtener(Guid id)
        {
            return await _usuarioDA.Obtener(id);
        }

        public async Task<Guid> Agregar(UsuarioRequest usuario)
        {
            var usuarioDA = new UsuarioDARequest
            {
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                Telefono = usuario.Telefono,
                ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasena)
            };

            return await _usuarioDA.Agregar(usuarioDA);
        }

        public Task<Guid> Editar(
            Guid Id,
            UsuarioEditarRequest usuario)
        {
            return _usuarioDA.Editar(Id, usuario);
        }
        public async Task<Guid> Eliminar(Guid id)
        {
            return await _usuarioDA.Eliminar(id);
        }

        public async Task<Guid> SolicitarRecuperacion(SolicitarRecuperacionRequest solicitud)

        {
            var usuario = await _usuarioDA.ObtenerPorCorreo(solicitud.Correo);

            if (usuario == null)
            {
                throw new Exception(
                    "No existe un usuario registrado con ese correo.");
            }

            var recuperacion = new RecuperacionPasswordRequest
            {
                IdUsuario = usuario.Id,
                Token = Guid.NewGuid().ToString(),
                FechaCreacion = DateTime.UtcNow,
                FechaExpiracion = DateTime.UtcNow.AddMinutes(30),
                Utilizado = false
            };

            var idRecuperacion =
                await _usuarioDA.AgregarRecuperacionPassword(recuperacion);

            await _correoServicio.EnviarRecuperacionPassword(
                usuario.Correo,
                usuario.Nombre,
                recuperacion.Token);

            return idRecuperacion;
        }

        public async Task CambiarPassword(CambiarPasswordRequest solicitud)
    
        {
            var recuperacion =
                await _usuarioDA.ObtenerRecuperacionPorToken(
                    solicitud.Token);

            if (recuperacion == null)
            {
                throw new Exception(
                    "El token de recuperación no existe.");
            }

            if (recuperacion.Utilizado)
            {
                throw new Exception(
                    "El token de recuperación ya fue utilizado.");
            }

            if (recuperacion.FechaExpiracion < DateTime.UtcNow)
            {
                throw new Exception(
                    "El token de recuperación está vencido.");
            }

            var nuevaContrasenaHash =
                BCrypt.Net.BCrypt.HashPassword(
                    solicitud.NuevaContrasena);

            await _usuarioDA.CambiarPassword(
                recuperacion.IdUsuario,
                nuevaContrasenaHash);

            await _usuarioDA.MarcarRecuperacionUtilizada(
                recuperacion.Id);
        }
    }
}