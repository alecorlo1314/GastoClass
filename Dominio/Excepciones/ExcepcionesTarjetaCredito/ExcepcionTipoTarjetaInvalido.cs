namespace GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

public class ExcepcionTipoTarjetaInvalido : ExcepcionDominio
{
    public ExcepcionTipoTarjetaInvalido(string campo, string? mensaje) : base(campo, mensaje)
    {
    }
}
