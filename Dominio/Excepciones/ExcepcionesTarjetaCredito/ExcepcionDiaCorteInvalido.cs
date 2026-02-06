namespace GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

public class ExcepcionDiaCorteInvalido : ExcepcionDominio
{
    public ExcepcionDiaCorteInvalido(string campo, string? mensaje) : base(campo, mensaje)
    {
    }
}
