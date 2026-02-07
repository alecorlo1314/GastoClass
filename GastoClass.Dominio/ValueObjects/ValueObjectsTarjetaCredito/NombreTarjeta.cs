using GastoClass.Dominio.Excepciones;

namespace GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;

/// <summary>
/// Representa el nombre de la tarjeta
/// Valida que el nombre de la tarjeta no este vacio
/// </summary>
public readonly record struct NombreTarjeta
{
    public string Valor { get; }

    public NombreTarjeta(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ExcepcionDominio(nameof(Valor), "El nombre de la tarjeta es requerido");

        Valor = valor.Trim();
    }
}

