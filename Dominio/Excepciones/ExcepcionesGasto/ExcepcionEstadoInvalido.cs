namespace GastoClass.Dominio.Excepciones.ExcepcionesGasto;

public class ExcepcionEstadoInvalido : ExcepcionDominio
{
    public ExcepcionEstadoInvalido(string campo, string? mensaje) : base(campo, mensaje)
    {
    }
}
