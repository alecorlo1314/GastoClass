namespace GastoClass.Dominio.Excepciones.ExcepcionesGasto;

public class ExcepcionNombreImagenRequerido : ExcepcionDominio
{
    public ExcepcionNombreImagenRequerido(string campo, string? mensaje) : base(campo, mensaje)
    {
    }
}
