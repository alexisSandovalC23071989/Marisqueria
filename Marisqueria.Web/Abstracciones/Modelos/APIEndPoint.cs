namespace Abstracciones.Modelo.Servicios
{
    public class APIEndPoint
    {
        public string? UrlBase { get; set; }
        public IEnumerable<Metodo>? Metodos { get; set; }
    }

    public class Metodo
    {
        public string Nombre { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
    }
}

