using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Text.Json;

namespace Web.Pages.Proveedores
{
    public class EditarModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public EditarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        [BindProperty]
        public ProveedorRequest proveedor { get; set; } = new ProveedorRequest();

        public async Task<ActionResult> OnGet(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProveedor");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, id));

            var respuesta = await cliente.SendAsync(solicitud);
            if (respuesta.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            respuesta.EnsureSuccessStatusCode();

            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var proveedorExistente = JsonSerializer.Deserialize<ProveedorResponse>(resultado, opciones);

            if (proveedorExistente == null)
            {
                return NotFound();
            }

            proveedor = new ProveedorRequest
            {
                Empresa = proveedorExistente.Empresa,
                CedulaJuridica = proveedorExistente.CedulaJuridica,
                Contacto = proveedorExistente.Contacto,
                Telefono = proveedorExistente.Telefono,
                Correo = proveedorExistente.Correo,
                Direccion = proveedorExistente.Direccion,
                Estado = proveedorExistente.Estado
            };

            ViewData["IdProveedor"] = proveedorExistente.IdProveedor;

            return Page();
        }

        public async Task<ActionResult> OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "EditarProveedor");
            var cliente = new HttpClient();
            cliente.AgregarAutorizacion(User);

            var respuesta = await cliente.PutAsJsonAsync(string.Format(endpoint, id), proveedor);
            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Index");
        }
    }
}
