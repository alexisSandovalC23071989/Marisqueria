using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos.ModeloPromociones;

namespace Flujo
{
    public class PromocionesFlujo : IPromocionesFlujo
    {
        private readonly IPromocionesDA _promocionesDA;

        public PromocionesFlujo(IPromocionesDA promocionesDA)
        {
            _promocionesDA = promocionesDA;
        }

        public Task<IEnumerable<PromocionResponse>> Obtener(string? estado = null) => _promocionesDA.Obtener(estado);
        public Task<IEnumerable<PromocionResponse>> ObtenerActivas() => _promocionesDA.ObtenerActivas();
        public Task<PromocionResponse?> Obtener(int IdPromocion) => _promocionesDA.Obtener(IdPromocion);
        public Task<PrecioPromocionalResponse?> ObtenerPrecioProducto(int IdProducto) => _promocionesDA.ObtenerPrecioProducto(IdProducto);

        public Task<IEnumerable<AlertaPromocionResponse>> ObtenerAlertas(int diasAnticipacion = 3, bool soloPendientes = true)
        {
            if (diasAnticipacion < 1 || diasAnticipacion > 30)
                throw new ArgumentException("Los días de anticipación deben estar entre 1 y 30.");
            return _promocionesDA.ObtenerAlertas(diasAnticipacion, soloPendientes);
        }

        public Task<int> MarcarAlertaRevisada(int IdAlerta) => _promocionesDA.MarcarAlertaRevisada(IdAlerta);

        public Task<int> Agregar(PromocionRequest promocion)
        {
            Validar(promocion);
            return _promocionesDA.Agregar(promocion);
        }

        public Task<int> Editar(int IdPromocion, PromocionRequest promocion)
        {
            Validar(promocion);
            return _promocionesDA.Editar(IdPromocion, promocion);
        }

        public Task<int> Desactivar(int IdPromocion) => _promocionesDA.Desactivar(IdPromocion);

        private static void Validar(PromocionRequest promocion)
        {
            if (promocion.FechaFin < promocion.FechaInicio)
                throw new ArgumentException("La fecha de fin debe ser igual o posterior a la fecha de inicio.");

            if (promocion.TipoDescuento == "PORCENTAJE" && promocion.ValorDescuento > 100)
                throw new ArgumentException("El porcentaje de descuento no puede ser mayor a 100.");

            if (!promocion.IdsProductos.Any() && !promocion.IdsCategorias.Any())
                throw new ArgumentException("Debe seleccionar al menos un producto o una categoría.");
        }
    }
}
