using GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

namespace GastoClass.Dominio.ValueObjects;

public record UltimosCuatroDigitosTarjeta
{
    public int Valor { get; }

    public UltimosCuatroDigitosTarjeta(int valor)
    {
        if (string.IsNullOrWhiteSpace(valor.ToString())
            || valor.ToString().Length != 4
            || !int.TryParse(valor.ToString(), out _))
        {
            throw new ExcepcionNumeroTarjetaInvalida();
        }
        this.Valor = valor;
    }
}
