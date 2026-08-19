namespace Abstracciones.Modelos.ModeloAutenticacion
{
    public class LoginResponse
    {
        public Guid Id { get; set; }    
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public Guid IdRol { get; set; }
        public string Rol { get; set; } = string.Empty;   
        public string Token { get; set; } = string.Empty;

    }
}
