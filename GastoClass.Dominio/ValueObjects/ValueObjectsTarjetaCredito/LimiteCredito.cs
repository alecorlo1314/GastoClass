using GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

namespace GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;

public readonly record struct LimiteCredito
{
    public decimal? Valor { get; }

    public LimiteCredito(decimal limiteCredito)
    {
        if (string.IsNullOrWhiteSpace(limiteCredito.ToString()))
            throw new ExcepcionLimiteCreditoInvalido(nameof(limiteCredito), "Limite Credito es requerido");
        if(limiteCredito <= 0) throw new ExcepcionLimiteCreditoInvalido(nameof(limiteCredito), "No puede ser menor a 0");
        Valor = limiteCredito;
    }
}
