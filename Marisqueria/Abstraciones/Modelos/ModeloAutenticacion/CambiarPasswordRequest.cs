using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos.ModeloAutenticacion
{
    public class CambiarPasswordRequest
    {
        [Required(ErrorMessage = "El token es requerido.")]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "La nueva contraseña es requerida.")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        public string NuevaContrasena { get; set; } = string.Empty;
    }
}