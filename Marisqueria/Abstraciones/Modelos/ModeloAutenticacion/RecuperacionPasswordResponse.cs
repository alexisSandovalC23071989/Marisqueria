namespace Abstracciones.Modelos.ModeloAutenticacion
{
    public class RecuperacionPasswordResponse
    {
        public Guid Id { get; set; }

        public Guid IdUsuario { get; set; }

        public string Token { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaExpiracion { get; set; }

        public bool Utilizado { get; set; }
    }
}