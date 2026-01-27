namespace GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

public class ExcepcionLimiteCreditoInvalido : ExcepcionDominio
{
    public ExcepcionLimiteCreditoInvalido(string campo, string? mensaje) : base(campo, mensaje)
    {
    }
}
