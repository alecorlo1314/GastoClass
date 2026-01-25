using GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

namespace GastoClass.Dominio.ValueObjects;
public record AnioVencimiento
{
    public int Anio { get; }

    public AnioVencimiento(int anioVencimiento)
    {
        if (anioVencimiento < 0 || 
            anioVencimiento > DateTime.Now.Year || 
            string.IsNullOrWhiteSpace(anioVencimiento.ToString()) || 
            string.IsNullOrEmpty(anioVencimiento.ToString()))
        {
            throw new ExcepcionAnioVencimientoInvalido();
        }
        Anio = anioVencimiento;
    }
}
