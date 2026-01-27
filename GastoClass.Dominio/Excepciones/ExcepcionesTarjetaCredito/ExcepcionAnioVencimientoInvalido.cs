namespace GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

public class ExcepcionAnioVencimientoInvalido : ExcepcionDominio
{
    public ExcepcionAnioVencimientoInvalido(string campo, string? mensaje) : base(campo, mensaje)
    {
    }
}
