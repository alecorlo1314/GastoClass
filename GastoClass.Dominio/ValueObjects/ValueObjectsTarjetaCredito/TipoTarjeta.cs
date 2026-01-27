using GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

namespace GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;

public readonly record struct TipoTarjeta
{
    public string? Valor { get; }
    public TipoTarjeta(string tipoTarjeta)
    {
        if(string.IsNullOrWhiteSpace(tipoTarjeta) || string.IsNullOrEmpty(tipoTarjeta))
        {
            throw new ExcepcionTipoTarjetaInvalido();
        }
        Valor = tipoTarjeta;
    }
}
