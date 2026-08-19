using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelo
{
    public class ProductoProveedorBase
    {
        [Required(ErrorMessage = "El precio de compra es requerido.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio de compra debe ser mayor a 0.")]
        public decimal PrecioCompra { get; set; }

        [Required(ErrorMessage = "La unidad de medida es requerida.")]
        [StringLength(50, ErrorMessage = "La unidad de medida no puede exceder los 50 caracteres.")]
        public string UnidadMedida { get; set; } = string.Empty;
    }

    public class ProductoProveedorRequest : ProductoProveedorBase
    {
        [Required(ErrorMessage = "El producto es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El producto es requerido.")]
        public int IdProducto { get; set; }
    }

    public class ProductoProveedorResponse : ProductoProveedorBase
    {
        public int IdProducto { get; set; }
        public int IdProveedor { get; set; }
      
        public string Producto { get; set; } = string.Empty;
    }
}
