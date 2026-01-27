namespace GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

public class ExcepcionBancoNullInvalido : ExcepcionDominio
{
    public ExcepcionBancoNullInvalido(string campo, string? mensaje) : base(campo, mensaje)
    {
    }
}
