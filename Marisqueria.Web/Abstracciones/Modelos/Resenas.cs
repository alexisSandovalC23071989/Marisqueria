using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Web.Modelos.ModeloResenas
{
    public class ResenaBase
    {
        [Required(ErrorMessage = "La calificación es requerida.")]
        [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5.")]
        [DisplayName("Calificación")]
        public int Calificacion { get; set; }

        [Required(ErrorMessage = "El comentario es requerido.")]
        [StringLength(500, ErrorMessage = "El comentario no puede exceder los 500 caracteres.")]
        [DisplayName("Comentario")]
        public string Comentario { get; set; } = string.Empty;
    }

    public class ResenaRequest : ResenaBase
    {
        [Required(ErrorMessage = "El usuario es requerido.")]
        [DisplayName("Usuario")]
        public Guid IdUsuario { get; set; }

        [Required(ErrorMessage = "El producto es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un producto.")]
        [DisplayName("Producto")]
        public int IdProducto { get; set; }
    }

    public class ResenaResponse : ResenaBase
    {
        public int IdResena { get; set; }

        public Guid IdUsuario { get; set; }

        public int IdProducto { get; set; }

        public DateTime Fecha { get; set; }
    }
}
