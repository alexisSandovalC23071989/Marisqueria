namespace Abstracciones.Interfaces.Servicios
{
    public interface ICorreoServicio
    {
        Task EnviarRecuperacionPassword(string correoDestino, string nombreUsuario, string token);

    }
}