using GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

namespace GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;

public readonly record struct NombreBanco
{
    public string Valor { get; }

    public NombreBanco(string nombreBanco)
    {
        if (string.IsNullOrWhiteSpace(nombreBanco) || string.IsNullOrEmpty(nombreBanco))
            throw new ExcepcionNumeroTarjetaInvalida(nameof(nombreBanco), "Banco es requerido");
        if(nombreBanco.Length < 2) 
            throw new ExcepcionBancoLongitudInvalida(nameof(nombreBanco), "No puede ser menor a 2");
        Valor = nombreBanco;
    }
}
