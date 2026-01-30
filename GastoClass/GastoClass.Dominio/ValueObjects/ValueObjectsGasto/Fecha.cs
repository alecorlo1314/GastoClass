using GastoClass.Dominio.Excepciones.ExcepcionesGasto;

namespace GastoClass.GastoClass.Dominio.ValueObjects.ValueObjectsGasto;

public readonly record struct Fecha
{
    public DateTime? Valor { get;}
    public Fecha(DateTime? fecha)
    {
        if(fecha!.Value.Year < 2024)
        {
            throw new ExcepcionFechaInvalida(nameof(fecha),"No puede ser menor al 2024");
        }
        if (fecha > DateTime.Now)
        {
            throw new ExcepcionFechaInvalida(nameof(fecha), "No puede ser mayor a la fecha actual");
        }
        if (fecha == null)
        {
            throw new ExcepcionFechaInvalida(nameof(fecha), "No puede ser nulo");
        }
        if (fecha < DateTime.MinValue)
        {
            throw new ExcepcionFechaInvalida(nameof(fecha), "No puede ser menor a la fecha minima");
        }
        Valor = fecha;
    }
}
