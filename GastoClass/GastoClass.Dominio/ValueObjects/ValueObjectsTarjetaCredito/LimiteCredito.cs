namespace GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;

public readonly record struct LimiteCredito
{
    public decimal Valor { get; }

    public LimiteCredito(decimal valor)
    {
        if (valor <= 0)
            throw new ExcepcionDominio(nameof(Valor), "El límite de crédito debe ser mayor a cero");

        Valor = valor;
    }
}

