namespace GastoClass.Dominio.Excepciones.ExcepcionesGasto;

public class ExcepcionFechaInvalida : ExcepcionDominio
{
    public ExcepcionFechaInvalida(string campo, string? mensaje) : base(campo, mensaje)
    {
    }
}
