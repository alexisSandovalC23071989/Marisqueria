using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos.ModeloProductos
{
    public class CategoriaBase
    {
        [Required(ErrorMessage = "El nombre es requerido.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;
    }

    public class CategoriaRequest : CategoriaBase
    {
    }

    public class CategoriaResponse : CategoriaBase
    {
        public int IdCategoria { get; set; }
    }
}
