using Abstracciones.Interfaces.Reglas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Resenas
{
    public class EliminarModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

    [BindProperty]
        public int IdResena { get; set; }

        [BindProperty]
        public int IdProducto { get; set; }

        public string ErrorResena { get; set; } = string.Empty;


        public EliminarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }


        // ==========================================
        // GET - MOSTRAR CONFIRMACIÓN
        // ==========================================

        public IActionResult OnGet(
            int idResena,
            int idProducto)
        {
            IdResena = idResena;
            IdProducto = idProducto;

            return Page();
        }


        // ==========================================
        // POST - ELIMINAR RESEÑA
        // ==========================================

        public async Task<IActionResult> OnPostAsync()
        {
            using var cliente = new HttpClient();

            var endpoint = string.Format(
                _configuracion.ObtenerMetodo(
                    "ApiEndPoints",
                    "EliminarResena"),
                IdResena);

            var respuesta = await cliente.DeleteAsync(endpoint);

            if (!respuesta.IsSuccessStatusCode)
            {
                var error =
                    await respuesta.Content.ReadAsStringAsync();

                ErrorResena =
                    $"No se pudo eliminar la reseña. {error}";

                return Page();
            }

            return RedirectToPage(
                "/Productos/Detalle",
                new
                {
                    id = IdProducto
                });
        }
    }


}
