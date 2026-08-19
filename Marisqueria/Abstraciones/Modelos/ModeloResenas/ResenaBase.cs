using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos.ModeloResenas
{
    public class ResenaBase
    {
        [Required(ErrorMessage = "La calificación es requerida.")]
        [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5.")]
        public int Calificacion { get; set; }

        [Required(ErrorMessage = "El comentario es requerido.")]
        public string Comentario { get; set; } = string.Empty;
    }
}