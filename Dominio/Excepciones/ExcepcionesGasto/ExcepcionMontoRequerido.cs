namespace GastoClass.Dominio.Excepciones.ExcepcionesGasto;

public class ExcepcionMontoRequerido : ExcepcionDominio
{
    public ExcepcionMontoRequerido(string campo, string? mensaje) : base(campo, mensaje)
    {
    }
}
