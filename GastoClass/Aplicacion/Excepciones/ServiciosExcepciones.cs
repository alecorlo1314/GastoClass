namespace GastoClass.Aplicacion.Excepciones;

public class ServiciosExcepciones : Exception
{
    public ServiciosExcepciones(string message) : base(message) { }
    public ServiciosExcepciones(string message, Exception innerException) : base(message, innerException) { }
}
