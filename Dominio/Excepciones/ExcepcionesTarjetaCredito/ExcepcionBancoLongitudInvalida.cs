namespace GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

public class ExcepcionBancoLongitudInvalida : ExcepcionDominio
{
    public ExcepcionBancoLongitudInvalida(string campo, string? mensaje) : base(campo, mensaje)
    {
    }
}
