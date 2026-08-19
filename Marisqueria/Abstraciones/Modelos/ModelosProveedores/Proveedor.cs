using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelo
{
    public class ProveedorBase
    {
        [Required(ErrorMessage = "La empresa es requerida.")]
        [StringLength(150, ErrorMessage = "La empresa no puede exceder los 150 caracteres y no puede ser menor a 2.", MinimumLength = 2)]
        public string Empresa { get; set; } = string.Empty;

        [Required(ErrorMessage = "La cédula jurídica es requerida.")]
        [RegularExpression(@"^\d{1}-\d{3}-\d{6}$", ErrorMessage = "La cédula jurídica debe tener el formato #-###-######.")]
        [Display(Name = "Cédula jurídica")]
        public string CedulaJuridica { get; set; } = string.Empty;

        [Required(ErrorMessage = "El contacto es requerido.")]
        [StringLength(100, ErrorMessage = "El contacto no puede exceder los 100 caracteres.")]
        [Display(Name = "Nombre de contacto")]
        public string Contacto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es requerido.")]
        [Phone(ErrorMessage = "El teléfono no tiene un formato válido.")]
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres.")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es requerido.")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        [StringLength(150, ErrorMessage = "El correo no puede exceder los 150 caracteres.")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es requerida.")]
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
}