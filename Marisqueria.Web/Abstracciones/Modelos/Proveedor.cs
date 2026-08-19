using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelo
{
    public class ProveedorBase
    {
        [Required(ErrorMessage = "La empresa es requerida.")]
        [StringLength(150, ErrorMessage = "La empresa no puede exceder los 150 caracteres y no puede ser menor a 2.", MinimumLength = 2)]
        public string Empresa { get; set; } = string.Empty;

        [Required(ErrorMessage = "La cedula juridica es requerida.")]
        [RegularExpression(@"^\d{1}-\d{3}-\d{6}$", ErrorMessage = "La cedula juridica debe tener el formato #-###-######.")]
        public string CedulaJuridica { get; set; } = string.Empty;

        [Required(ErrorMessage = "El contacto es requerido.")]
        [StringLength(100, ErrorMessage = "El contacto no puede exceder los 100 caracteres.")]
        public string Contacto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El telefono es requerido.")]
        [Phone(ErrorMessage = "El telefono no tiene un formato valido.")]
        [StringLength(20, ErrorMessage = "El telefono no puede exceder los 20 caracteres.")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es requerido.")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato valido.")]
        [StringLength(150, ErrorMessage = "El correo no puede exceder los 150 caracteres.")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La direccion es requerida.")]
        public string Direccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estado es requerido.")]
        [RegularExpression("^(Activo|Inactivo)$", ErrorMessage = "El estado debe ser 'Activo' o 'Inactivo'.")]
        public string Estado { get; set; } = string.Empty;
    }

    public class ProveedorRequest : ProveedorBase
    {
    }

    public class ProveedorResponse : ProveedorBase
    {
        public int IdProveedor { get; set; }
    }

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
