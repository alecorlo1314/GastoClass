using GastoClass.Dominio.Excepciones.ExcepcionesGasto;

namespace GastoClass.Dominio.ValueObjects.ValueObjectsGasto;

public readonly record struct Categoria
{
    public string Valor { get; }

    public Categoria(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ExcepcionCategoriaInvalida();
        }
        Valor = valor;
    }
    public override string ToString() => Valor;
}
