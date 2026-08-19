using Abstracciones.Modelos.ModeloPromociones;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IPromocionesController
    {
        Task<IActionResult> Obtener(string? estado = null);
        Task<IActionResult> ObtenerActivas();
        Task<IActionResult> Obtener(int IdPromocion);
        Task<IActionResult> ObtenerPrecioProducto(int IdProducto);
        Task<IActionResult> ObtenerAlertas(int diasAnticipacion = 3, bool soloPendientes = true);
        Task<IActionResult> MarcarAlertaRevisada(int IdAlerta);
        Task<IActionResult> Agregar(PromocionRequest promocion);
        Task<IActionResult> Editar(int IdPromocion, PromocionRequest promocion);
        Task<IActionResult> Desactivar(int IdPromocion);
    }
}
