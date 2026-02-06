using GastoClass.Dominio.Excepciones;

namespace GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;

public readonly record struct TipoTarjeta
{
    public string Valor { get; }

    public TipoTarjeta(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ExcepcionDominio(nameof(Valor), "El tipo de tarjeta es requerido");

        Valor = valor.Trim();
    }
}

