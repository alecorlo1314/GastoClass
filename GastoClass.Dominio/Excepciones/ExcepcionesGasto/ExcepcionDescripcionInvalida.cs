namespace GastoClass.Dominio.Excepciones.ExcepcionesGasto;

public class ExcepcionDescripcionInvalida : ExcepcionDominio
{
    public ExcepcionDescripcionInvalida(string campo, string? mensaje) : base(campo, mensaje)
    {
    }
}
