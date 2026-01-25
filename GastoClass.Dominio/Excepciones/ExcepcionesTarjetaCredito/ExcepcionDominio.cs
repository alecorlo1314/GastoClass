namespace GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

/// <summary>
/// Esta clase se encarga de manejar las excepciones del dominio
/// </summary>
public class ExcepcionDominio : Exception
{
    public ExcepcionDominio(string mensaje) : base(mensaje) { }
}
