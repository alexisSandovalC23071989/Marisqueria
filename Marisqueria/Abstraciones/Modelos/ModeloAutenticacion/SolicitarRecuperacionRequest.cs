using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos.ModeloAutenticacion
{
    public class SolicitarRecuperacionRequest
    {
        [Required(ErrorMessage = "El correo es requerido.")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        public string Correo { get; set; } = string.Empty;
    }
}