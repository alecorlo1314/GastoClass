using GastoClass.Dominio.Excepciones.ExcepcionesGasto;

namespace GastoClass.Dominio.ValueObjects.ValueObjectsGasto;

public readonly record struct Fecha
{
    public DateTime? Valor { get;}
    public Fecha(DateTime? fecha)
    {
        if(fecha!.Value.Year < 2024)
        {
            throw new ExcepcionFechaInvalida();
        }
        Valor = fecha;
    }
}
