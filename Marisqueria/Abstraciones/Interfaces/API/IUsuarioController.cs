using Abstracciones.Modelos.ModeloAutenticacion;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IUsuarioController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> Obtener(Guid Id);
        Task<IActionResult> Agregar(UsuarioRequest usuario);
        Task<IActionResult> Editar(Guid Id, UsuarioEditarRequest usuario);
        Task<IActionResult> Eliminar(Guid Id);
        Task<IActionResult> SolicitarRecuperacion(SolicitarRecuperacionRequest solicitud);
        Task<IActionResult> CambiarPassword(CambiarPasswordRequest solicitud);

    }
}