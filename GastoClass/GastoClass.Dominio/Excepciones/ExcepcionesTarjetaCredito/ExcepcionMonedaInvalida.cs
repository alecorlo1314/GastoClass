namespace GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

public class ExcepcionMonedaInvalida : ExcepcionDominio
{
    public ExcepcionMonedaInvalida(string campo, string? mensaje) : base(campo, mensaje)
    {
    }
}
