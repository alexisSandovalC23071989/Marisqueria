using Abstracciones.Modelos.ModeloPedido;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Interfaces.API
{
    public interface IDetallePedidoController
    {

        Task<IActionResult> ObtenerPorPedido(Guid IdPedido);

        Task<IActionResult> Agregar(DetallePedidoRequest detalle);

        Task<IActionResult> Editar(Guid IdDetalle, DetallePedidoRequest detalle);

        Task<IActionResult> Eliminar(Guid IdDetalle);
    }
}
