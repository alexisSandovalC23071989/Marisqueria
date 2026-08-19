namespace Abstracciones.Modelos.ModeloResenas;

public class ResenaResponse
{
    public int IdResena { get; set; }
    public Guid IdUsuario { get; set; }
    public int IdProducto { get; set; }
    public int Calificacion { get; set; }
    public string Comentario { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
}
