using GastoClass.Dominio.Excepciones;

namespace GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;

public readonly record struct NombreBanco
{
    public string Valor { get; }

    public NombreBanco(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ExcepcionDominio(nameof(Valor), "El nombre del banco es requerido");

        if (valor.Trim().Length < 2)
            throw new ExcepcionDominio(nameof(Valor), "El nombre del banco debe tener al menos 2 caracteres");

        Valor = valor.Trim();
    }
}

