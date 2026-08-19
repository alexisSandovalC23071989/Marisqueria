using Abstracciones.Modelos.ModeloPromociones;

namespace Abstracciones.Interfaces.DA
{
    public interface IPromocionesDA
    {
        Task<IEnumerable<PromocionResponse>> Obtener(string? estado = null);
        Task<IEnumerable<PromocionResponse>> ObtenerActivas();
        Task<PromocionResponse?> Obtener(int IdPromocion);
        Task<PrecioPromocionalResponse?> ObtenerPrecioProducto(int IdProducto);
        Task<IEnumerable<AlertaPromocionResponse>> ObtenerAlertas(int diasAnticipacion = 3, bool soloPendientes = true);
        Task<int> MarcarAlertaRevisada(int IdAlerta);
        Task<int> Agregar(PromocionRequest promocion);
        Task<int> Editar(int IdPromocion, PromocionRequest promocion);
        Task<int> Desactivar(int IdPromocion);
    }
}
