using Abstracciones.Interfaces.Servicios;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Flujo.Servicios
{
    public class CorreoServicio : ICorreoServicio
    {
        private readonly IConfiguration _configuration;

        public CorreoServicio(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnviarRecuperacionPassword(
            string correoDestino,
            string nombreUsuario,
            string token)
        {
            var correoOrigen =
                _configuration["Correo:CorreoOrigen"];

            var passwordAplicacion =
                _configuration["Correo:PasswordAplicacion"];

            var host =
                _configuration["Correo:Host"];

            var puertoTexto =
                _configuration["Correo:Puerto"];

            if (string.IsNullOrWhiteSpace(correoOrigen))
            {
                throw new Exception(
                    "No se configuró Correo:CorreoOrigen.");
            }

            if (string.IsNullOrWhiteSpace(passwordAplicacion))
            {
                throw new Exception(
                    "No se configuró Correo:PasswordAplicacion.");
            }

            if (string.IsNullOrWhiteSpace(host))
            {
                throw new Exception(
                    "No se configuró Correo:Host.");
            }

            if (!int.TryParse(puertoTexto, out var puerto))
            {
                throw new Exception(
                    "El puerto SMTP no es válido.");
            }

            var enlace =
                $"https://localhost:7247/Autenticacion/CambiarPassword?token={token}";

            var asunto = "Recuperación de contraseña";

            var cuerpo = $"""
                Hola {nombreUsuario}.

                Recibimos una solicitud para cambiar tu contraseña.

                Ingresa al siguiente enlace:

                {enlace}

                Este enlace expira en 30 minutos.
                """;

            using var mensaje = new MailMessage
            {
                From = new MailAddress(
                    correoOrigen,
                    "Marisquería"),
                Subject = asunto,
                Body = cuerpo,
                IsBodyHtml = false
            };

            mensaje.To.Add(correoDestino);

            using var clienteSmtp =
                new SmtpClient(host, puerto)
                {
                    Credentials = new NetworkCredential(
                        correoOrigen,
                        passwordAplicacion),
                    EnableSsl = true
                };

            await clienteSmtp.SendMailAsync(mensaje);
        }
    }
}