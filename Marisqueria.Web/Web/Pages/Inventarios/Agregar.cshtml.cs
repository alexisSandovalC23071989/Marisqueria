using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Json;

namespace Web.Pages.Inventarios
{
    public class AgregarModel : PageModel
    {
        private readonly IConfiguracion _configuracion;
        [BindProperty] public InventarioRequest Inventario { get; set; } = new();
        public List<SelectListItem> Productos { get; private set; } = new();
        public List<SelectListItem> Proveedores { get; private set; } = new();

        public AgregarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public async Task OnGetAsync() => await CargarCatalogosAsync();

        public async Task<IActionResult> OnPostAsync()
        {
            if (Inventario.FechaVencimiento.Date < Inventario.FechaIngreso.Date)
                ModelState.AddModelError("Inventario.FechaVencimiento", "La fecha de vencimiento debe ser igual o posterior a la fecha de ingreso.");

            if (!ModelState.IsValid)
            {
                await CargarCatalogosAsync();
                return Page();
            }

            using var cliente = new HttpClient();
            cliente.AgregarAutorizacion(User);
            var respuesta = await cliente.PostAsJsonAsync(_configuracion.ObtenerMetodo("ApiEndPoints", "AgregarInventario"), Inventario);
            if (!respuesta.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, await respuesta.Content.ReadAsStringAsync());
                await CargarCatalogosAsync();
                return Page();
            }
            return RedirectToPage("./Index");
        }

        private async Task CargarCatalogosAsync()
        {
            using var cliente = new HttpClient();
            var productos = await cliente.GetFromJsonAsync<List<ProductoResponse>>(_configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProductos")) ?? new();
            var proveedores = await cliente.GetFromJsonAsync<List<ProveedorResponse>>(_configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProveedores")) ?? new();
            Productos = productos.Select(x => new SelectListItem(x.Nombre, x.IdProducto.ToString())).ToList();
            Proveedores = proveedores.Select(x => new SelectListItem(x.Empresa, x.IdProveedor.ToString())).ToList();
        }
    }
}
