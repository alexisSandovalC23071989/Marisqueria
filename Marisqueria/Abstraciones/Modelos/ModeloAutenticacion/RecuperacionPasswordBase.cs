namespace Abstracciones.Modelos.ModeloAutenticacion
{

    public class RecuperacionPasswordBase
    {
        public Guid IdUsuario { get; set; }
        public string Token { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public bool Utilizado { get; set; }
    }

}