namespace GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

public class ExcepcionDiaPagoInvalido : ExcepcionDominio
{
    public ExcepcionDiaPagoInvalido(string campo, string? mensaje) : base(campo, mensaje)
    {
    }
}
