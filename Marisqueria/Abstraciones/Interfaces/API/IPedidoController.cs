using Abstracciones.Modelos.ModeloPedido;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Interfaces.API
{
    public interface IPedidoController
    {
        Task<IActionResult> Obtener();

        Task<IActionResult> Obtener(Guid IdPedido);

        Task<IActionResult> ObtenerPorUsuario(Guid IdUsuario);

        Task<IActionResult> Agregar(PedidoRequest pedido);

        Task<IActionResult> Editar(Guid IdPedido, PedidoRequest pedido);

        Task<IActionResult> Eliminar(Guid IdPedido);

        Task<IActionResult> ObtenerPedidoActivo(Guid idUsuario);

        Task<IActionResult> Confirmar(Guid idPedido);

        Task<IActionResult> Cancelar(Guid idPedido);

        Task<IActionResult> Preparar(Guid idPedido);

        Task<IActionResult> Entregar(Guid idPedido);

    }
}
