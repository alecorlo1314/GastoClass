using GastoClass.Dominio.Excepciones.ExcepcionesGasto;
using GastoClass.GastoClass.Dominio.Excepciones.ExcepcionesGasto;

public readonly record struct Comercio
{
    public string Valor { get; }

    public Comercio(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ExcepcionComercioRequerido(nameof(Valor), "El comercio es requerido");

        if (valor.Length > 100)
            throw new ExcepcionComercioMaximoCaracteres(nameof(Valor),
                "El comercio no puede tener más de 100 caracteres");

        Valor = valor.Trim();
    }

    public override string ToString() => Valor;
}
