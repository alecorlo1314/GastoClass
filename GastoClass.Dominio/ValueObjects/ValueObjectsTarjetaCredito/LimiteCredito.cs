using GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

namespace GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;

public readonly record struct LimiteCredito
{
    public decimal? Valor { get; }

    public LimiteCredito(decimal limiteCredito)
    {
        if (string.IsNullOrWhiteSpace(limiteCredito.ToString()) || limiteCredito <= 0)
        {
            throw new ExcepcionLimiteCreditoInvalido();
        }
        Valor = limiteCredito;
    }
}
