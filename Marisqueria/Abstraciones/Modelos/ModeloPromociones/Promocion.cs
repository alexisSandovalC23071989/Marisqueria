using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos.ModeloPromociones
{
    public class PromocionRequest
    {
        [Required(ErrorMessage = "El nombre es requerido.")]
        [StringLength(120, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 120 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de descuento es requerido.")]
        [RegularExpression("PORCENTAJE|MONTO", ErrorMessage = "El tipo de descuento debe ser PORCENTAJE o MONTO.")]
        public string TipoDescuento { get; set; } = "PORCENTAJE";

        [Range(0.01, double.MaxValue, ErrorMessage = "El descuento debe ser mayor a cero.")]
        public decimal ValorDescuento { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es requerida.")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es requerida.")]
        public DateTime FechaFin { get; set; }

        public List<int> IdsProductos { get; set; } = new();
        public List<int> IdsCategorias { get; set; } = new();
    }

    public class PromocionResponse
    {
        public int IdPromocion { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string TipoDescuento { get; set; } = string.Empty;
        public decimal ValorDescuento { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Activa { get; set; }
        public DateTime? FechaDesactivacion { get; set; }
        public string Estado { get; set; } = string.Empty;
        public List<PromocionProductoResponse> Productos { get; set; } = new();
        public List<PromocionCategoriaResponse> Categorias { get; set; } = new();
    }

    public class PromocionProductoResponse
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal PrecioOriginal { get; set; }
        public decimal PrecioPromocional { get; set; }
        public bool AsignacionDirecta { get; set; }
    }

    public class PromocionCategoriaResponse
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class PrecioPromocionalResponse
    {
        public int IdProducto { get; set; }
        public int IdPromocion { get; set; }
        public string NombrePromocion { get; set; } = string.Empty;
        public string TipoDescuento { get; set; } = string.Empty;
        public decimal ValorDescuento { get; set; }
        public decimal PrecioOriginal { get; set; }
        public decimal PrecioPromocional { get; set; }
        public decimal Ahorro { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }

    public class AlertaPromocionResponse
    {
        public int IdAlerta { get; set; }
        public int IdPromocion { get; set; }
        public string NombrePromocion { get; set; } = string.Empty;
        public DateTime FechaFin { get; set; }
        public int DiasRestantes { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public bool Revisada { get; set; }
        public DateTime? FechaRevision { get; set; }
    }
}
