using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos.ModeloInventario
{
    public class InventarioBase
    {
        [Required(ErrorMessage = "El lote es requerido.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El lote no puede exceder los 100 caracteres.")]
        public string Lote { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "La cantidad disponible no puede ser negativa.")]
        public decimal CantidadDisponible { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaIngreso { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        public DateTime FechaVencimiento { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "La ubicación es requerida.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "La ubicación debe tener entre 2 y 150 caracteres.")]
        public string Ubicacion { get; set; } = string.Empty;
    }

    public class InventarioRequest : InventarioBase
    {
        [Range(1, int.MaxValue, ErrorMessage = "El producto es requerido.")]
        public int IdProducto { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El proveedor es requerido.")]
        public int IdProveedor { get; set; }
    }

    public class InventarioResponse : InventarioRequest
    {
        public int IdInventario { get; set; }
        public string Producto { get; set; } = string.Empty;
        public string Proveedor { get; set; } = string.Empty;
    }

    public class StockProductoResponse
    {
        public int IdProducto { get; set; }

        public decimal StockDisponible { get; set; }
    }
}
