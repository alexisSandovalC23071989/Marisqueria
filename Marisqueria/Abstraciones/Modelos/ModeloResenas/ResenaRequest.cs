namespace Abstracciones.Modelos.ModeloResenas
{
    public class ResenaRequest : ResenaBase
    {
        public Guid IdUsuario { get; set; }
        public int IdProducto { get; set; }
    }
}