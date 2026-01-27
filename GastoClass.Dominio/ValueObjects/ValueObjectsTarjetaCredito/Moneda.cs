using GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

namespace GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;

public readonly record struct Moneda
{
    public string Valor { get; }

    public Moneda(string tipoMoneda)
    {
        if (string.IsNullOrWhiteSpace(tipoMoneda))
            throw new ExcepcionMonedaInvalida(nameof(tipoMoneda), "El tipo de moneda no puede estar vacío.");

        Valor = tipoMoneda;
    }
}
