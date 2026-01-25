using GastoClass.Dominio.Excepciones;

namespace GastoClass.Dominio.ValueObjects;

public record NombreBanco
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
