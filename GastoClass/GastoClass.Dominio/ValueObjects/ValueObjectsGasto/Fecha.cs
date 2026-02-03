using GastoClass.Dominio.Excepciones.ExcepcionesGasto;
namespace GastoClass.GastoClass.Dominio.ValueObjects.ValueObjectsGasto;

public readonly record struct Fecha
{
    public DateTime Valor { get; }

    public Fecha(DateTime fecha)
    {
        if (fecha.Year < 2024)
            throw new ExcepcionFechaInvalida(nameof(Valor),
                "La fecha no puede ser menor al año 2024");

        if (fecha > DateTime.Now)
            throw new ExcepcionFechaInvalida(nameof(Valor),
                "La fecha no puede ser mayor a la fecha actual");

        Valor = fecha;
    }
}
