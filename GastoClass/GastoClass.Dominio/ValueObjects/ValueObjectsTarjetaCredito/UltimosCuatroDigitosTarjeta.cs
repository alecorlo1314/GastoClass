using GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

namespace GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;

public readonly record struct UltimosCuatroDigitosTarjeta
{
    public int Valor { get; }

    public UltimosCuatroDigitosTarjeta(int valor)
    {
        if (string.IsNullOrWhiteSpace(valor.ToString()))
            throw new ExcepcionNumeroTarjetaInvalida(nameof(valor), "El número de tarjeta no puede estar vacío.");
        if (valor.ToString().Length != 4)
            throw new ExcepcionNumeroTarjetaInvalida(nameof(valor), "El número de tarjeta debe ser de 4 digitos.");
        Valor = valor;
    }
}
