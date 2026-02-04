namespace GastoClass.GastoClass.Dominio.ValueObjects.ValueObjectsGasto;

public readonly record struct Comercio
{
    public string Valor { get; }

    public Comercio(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ExcepcionDominio(nameof(Valor), "El comercio es requerido");

        if (valor.Length > 100)
            throw new ExcepcionDominio(nameof(Valor), "Máximo 100 caracteres");

        Valor = valor.Trim();
    }

    public override string ToString() => Valor;
}
