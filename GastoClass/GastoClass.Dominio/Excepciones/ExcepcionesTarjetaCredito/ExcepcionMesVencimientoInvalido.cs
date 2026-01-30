namespace GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

public class ExcepcionMesVencimientoInvalido : ExcepcionDominio
{
    public ExcepcionMesVencimientoInvalido(string campo, string? mensaje) : base(campo, mensaje)
    {
    }
}
