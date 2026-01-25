using GastoClass.Dominio.Excepciones;

namespace GastoClass.Dominio.ValueObjects;

/// <summary>
/// Representa el nombre de la tarjeta
/// Valida que el nombre de la tarjeta no este vacio
/// </summary>
public record NombreTarjeta
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
