using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Proveedores
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public IndexModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public List<ProveedorResponse> proveedores { get; set; } = new List<ProveedorResponse>();

        public async Task<ActionResult> OnGet()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProveedores");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, endpoint);

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();

            if (respuesta.StatusCode == HttpStatusCode.NoContent)
            {
                proveedores = new List<ProveedorResponse>();
                return Page();
            }

            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            proveedores = JsonSerializer.Deserialize<List<ProveedorResponse>>(resultado, opciones) ?? new List<ProveedorResponse>();

            return Page();
        }

        public async Task<ActionResult> OnPostEliminarAsync(int id)
        {
            if (!User.IsInRole("Administrador")) return Forbid();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "EliminarProveedor");
            var cliente = new HttpClient();
            cliente.AgregarAutorizacion(User);

            var respuesta = await cliente.DeleteAsync(string.Format(endpoint, id));
            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Index");
        }
    }
}
