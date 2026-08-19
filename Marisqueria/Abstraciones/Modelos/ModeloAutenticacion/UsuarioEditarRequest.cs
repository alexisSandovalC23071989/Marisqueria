using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos.ModeloAutenticacion
{
    public class UsuarioEditarRequest
    {
        [Required(ErrorMessage = "El nombre es requerido.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es requerido.")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es requerido.")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El rol es requerido.")]
        public Guid IdRol { get; set; }
    }
}