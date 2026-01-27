using GastoClass.Dominio.Excepciones.ExcepcionesGasto;

namespace GastoClass.Dominio.ValueObjects.ValueObjectsGasto;

public readonly record struct Comercio
{
    public string? Valor { get; }

    public Comercio(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ExcepcionComercioRequerido();
        }
        Valor = valor;
    }

}
