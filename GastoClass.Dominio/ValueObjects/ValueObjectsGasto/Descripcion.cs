using GastoClass.Dominio.Excepciones.ExcepcionesGasto;

namespace GastoClass.Dominio.ValueObjects.ValueObjectsGasto;

public readonly record struct Descripcion
{
    public string Valor { get; }

    public Descripcion(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ExcepcionDescripcionInvalida();
        }
        Valor = valor.Trim();
        if (valor.Length > 200)
        {
            throw new ExcepcionDescripcionInvalida();
        }
        Valor = valor;
    }
    public override string ToString() => Valor;
}
