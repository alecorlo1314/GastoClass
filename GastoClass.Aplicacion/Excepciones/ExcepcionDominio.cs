namespace GastoClass.Aplicacion.Excepciones;

/// <summary>
/// Contiene las excepciones de validacion de capa de aplicacion
/// Para errores de entrada, no negocio
/// </summary>
public class ExcepcionDominio : Exception
{
    public ExcepcionDominio(string message) : base(message) { }
}
