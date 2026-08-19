using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelo
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

    // Se mantiene para las páginas de Productos que ya consumen este tipo.
    public class Categoria : CategoriaResponse
    {
    }
}
