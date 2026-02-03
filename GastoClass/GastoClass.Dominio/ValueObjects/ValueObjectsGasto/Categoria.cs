using GastoClass.Dominio.Excepciones.ExcepcionesGasto;
namespace GastoClass.GastoClass.Dominio.ValueObjects.ValueObjectsGasto;

public readonly record struct Categoria
{
    public string Valor { get; }

    public Categoria(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ExcepcionDominio(nameof(Valor), "La categoría es requerida");

        Valor = valor.Trim();
    }

    public override string ToString() => Valor;
}
