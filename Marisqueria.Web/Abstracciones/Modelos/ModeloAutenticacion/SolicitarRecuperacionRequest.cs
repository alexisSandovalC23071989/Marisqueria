using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Web.Modelos.ModeloAutenticacion
{
    public class SolicitarRecuperacionRequest
    {
        [Required(ErrorMessage = "El correo es requerido.")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo válido.")]
        public string Correo { get; set; } = string.Empty;
    }
}