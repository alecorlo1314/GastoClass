using GastoClass.Dominio.Excepciones;

namespace GastoClass.Dominio.Entidades;

public readonly record struct TipoMoneda
{
    public string Valor { get; }

    public TipoMoneda(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ExcepcionDominio(nameof(Valor), "El tipo de moneda es requerido");

        Valor = valor.Trim();
    }
}

