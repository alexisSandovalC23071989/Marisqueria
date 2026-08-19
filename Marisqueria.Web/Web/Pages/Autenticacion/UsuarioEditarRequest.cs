namespace Abstracciones.Web.Modelos.ModeloAutenticacion
{
    public class UsuarioEditarRequest
    {
        public string Nombre { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public Guid IdRol { get; set; }
    }
}