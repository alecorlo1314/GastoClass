using GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

namespace GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;

/// <summary>
/// Representa el nombre de la tarjeta
/// Valida que el nombre de la tarjeta no este vacio
/// </summary>
public readonly record struct NombreTarjeta
{
    public string? Valor { get; }

    public NombreTarjeta(string nombreTarjeta)
    {
        if (string.IsNullOrWhiteSpace(nombreTarjeta))
        {
            throw new ExcepcionNumeroTarjetaInvalida();
        }
        this.Valor = nombreTarjeta;
    }
}
