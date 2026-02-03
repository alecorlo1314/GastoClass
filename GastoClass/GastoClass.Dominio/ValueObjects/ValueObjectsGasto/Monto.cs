using GastoClass.Dominio.Excepciones.ExcepcionesGasto;

namespace GastoClass.GastoClass.Dominio.ValueObjects.ValueObjectsGasto;

public readonly record struct Monto
{
    public decimal Valor { get; }

    public Monto(decimal valor)
    {
        if (valor <= 0)
            throw new ExcepcionDominio(nameof(Valor),
                "El monto debe ser mayor a cero");

        Valor = valor;
    }
}

