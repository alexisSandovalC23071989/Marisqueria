namespace Abstracciones.Web.Modelos.ModeloAutenticacion
{
    public class UsuarioResponse
    {
        public Guid Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public Guid IdRol { get; set; }

        public string Rol { get; set; } = string.Empty;
    }
}