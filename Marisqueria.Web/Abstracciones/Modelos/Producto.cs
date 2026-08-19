using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelo
{
    public class ProductoBase
    {
        [Required(ErrorMessage = "El nombre es requerido.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres y no puede ser menor a 2.", MinimumLength = 2)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripcion es requerida.")]
        [DisplayName("Descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio de venta actual es requerido.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio de venta actual debe ser mayor a 0.")]
        [DisplayName("Precio de venta actual")]
        public decimal PrecioVentaActual { get; set; }

        [Required(ErrorMessage = "La imagen es requerida.")]
        public string Imagen { get; set; } = string.Empty;
    }

    public class ProductoRequest : ProductoBase
    {
        [Required(ErrorMessage = "La categoria es requerida.")]
        [Range(1, int.MaxValue, ErrorMessage = "La categoria es requerida.")]
        [DisplayName("Categoria")]
        public int IdCategoria { get; set; }
    }

    public class ProductoResponse : ProductoBase
    {
        public int IdProducto { get; set; }
        public int IdCategoria { get; set; }
        public string Categoria { get; set; } = string.Empty;
    }
}

