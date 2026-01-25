using GastoClass.Dominio.Excepciones;

namespace GastoClass.Dominio.ValueObjects;

public record LimiteCredito
{
    public int? Valor { get; }

    public LimiteCredito(int limiteCredito)
    {
        if (string.IsNullOrWhiteSpace(limiteCredito.ToString()) || limiteCredito <= 0)
        {
            throw new ExcepcionLimiteCreditoInvalido();
        }
        Valor = limiteCredito;
    }
}
