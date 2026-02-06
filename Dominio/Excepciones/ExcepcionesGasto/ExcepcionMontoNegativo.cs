namespace GastoClass.Dominio.Excepciones.ExcepcionesGasto;

public class ExcepcionMontoNegativo : ExcepcionDominio
{
    public ExcepcionMontoNegativo(string campo, string? mensaje) : base(campo, mensaje)
    {
    }
}
