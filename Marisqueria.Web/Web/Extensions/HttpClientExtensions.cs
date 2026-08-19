using System.Net.Http.Headers;
using System.Security.Claims;

namespace Web.Extensions
{
    public static class HttpClientExtensions
    {
        public static void AgregarAutorizacion(
            this HttpClient cliente,
            ClaimsPrincipal usuario)
        {
            var token = usuario.FindFirst("Token")?.Value;

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new InvalidOperationException(
                    "No se encontró el token del usuario autenticado.");
            }

            cliente.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
