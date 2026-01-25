using GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

namespace GastoClass.Dominio.ValueObjects;

public record Moneda
{
    public string Valor { get; }

    public Moneda(string tipoMoneda)
    {
        if (string.IsNullOrWhiteSpace(tipoMoneda) || string.IsNullOrEmpty(tipoMoneda))
        {
            throw new ExcepcionMonedaInvalida();
        }

        Valor = tipoMoneda;
    }
}
