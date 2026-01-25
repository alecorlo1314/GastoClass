
using GastoClass.Dominio.Excepciones;

namespace GastoClass.Dominio.ValueObjects;

public record UltimosCuatroDigitosTarjeta
{
    public string Valor { get; }

    public UltimosCuatroDigitosTarjeta(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor)
            || valor.Length != 4 
            || !int.TryParse(valor.ToString(), out _))
        {
            throw new ExcepcionNumeroTarjetaInvalida();
        }
        this.Valor = valor;
    }
}
