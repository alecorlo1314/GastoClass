using GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

namespace GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;

public readonly record struct NombreBanco
{
    public string Valor { get; }

    public NombreBanco(string nombreBanco)
    {
        if (string.IsNullOrWhiteSpace(nombreBanco) || string.IsNullOrEmpty(nombreBanco))
            throw new ExcepcionBancoNullInvalido();

        if(nombreBanco.Length < 2) 
            throw new ExcepcionBancoLongitudInvalida();
        Valor = nombreBanco;
    }
}
