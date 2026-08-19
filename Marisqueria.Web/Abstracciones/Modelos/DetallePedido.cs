namespace Abstracciones.Modelos.ModeloPedido
{
    public class DetallePedidoBase
    {
        public int IdProducto { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioOriginal { get; set; }

        public string TipoDescuento { get; set; } = string.Empty;

        public decimal ValorDescuento { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal { get; set; }

        public int? IdPromocion { get; set; }
    }

    public class DetallePedidoRequest : DetallePedidoBase
    {
        public Guid IdPedido { get; set; }
    }

    public class DetallePedidoResponse : DetallePedidoBase
    {
        public Guid IdDetalle { get; set; }

        public Guid IdPedido { get; set; }

        public string NombreProducto { get; set; }

        public string? NombrePromocion { get; set; }
    }
}
