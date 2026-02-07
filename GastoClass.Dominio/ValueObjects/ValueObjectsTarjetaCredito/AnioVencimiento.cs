using GastoClass.Dominio.Excepciones;

namespace GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;

public readonly record struct AnioVencimiento
{
    public int Anio { get; }

    public AnioVencimiento(int anio)
    {
        if (anio < DateTime.Now.Year)
            throw new ExcepcionDominio(nameof(Anio), "El año de vencimiento no puede ser menor al actual");

        Anio = anio;
    }
}

