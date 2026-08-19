using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos.ModeloAutenticacion
{
    public class UsuarioRequest : UsuarioBase
    {
        [Required(ErrorMessage = "La contraseña es requerida.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Contrasena { get; set; } = string.Empty;
    }
}
