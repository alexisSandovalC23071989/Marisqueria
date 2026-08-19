namespace Abstracciones.Modelos.ModeloAutenticacion
{
    //Voy a usarla internamente para enviar a DA con la contraseña encriptada
    public class UsuarioDARequest : UsuarioBase
    {
        public string ContrasenaHash { get; set; } = string.Empty;
    }
}