namespace Abstracciones.Modelo
{
    public class PedidoBase
    {
        public decimal Total { get; set; }

        public string Estado { get; set; } = string.Empty;
    }

    public class PedidoRequest
    {
        public Guid IdUsuario { get; set; }
    }

    public class PedidoResponse : PedidoBase
    {
        public Guid IdPedido { get; set; }

        public string NombreUsuario { get; set; } = string.Empty;

        public DateTime FechaPedido { get; set; }
    }
}
