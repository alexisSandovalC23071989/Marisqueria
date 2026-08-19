using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Web.Modelos.ModeloAutenticacion
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "El correo es requerido.")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo válido.")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida.")]
        public string Contrasena { get; set; } = string.Empty;
    }
}