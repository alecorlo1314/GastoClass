namespace GastoClass.Dominio.Excepciones.ExcepcionesGasto;

public class ExcepcionComercioRequerido : ExcepcionDominio
{
    public ExcepcionComercioRequerido(string campo, string? mensaje) : base(campo, mensaje)
    {
    }
}
