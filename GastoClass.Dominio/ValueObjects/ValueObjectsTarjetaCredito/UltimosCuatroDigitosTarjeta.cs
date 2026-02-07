using GastoClass.Dominio.Excepciones;

namespace GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;

public readonly record struct UltimosCuatroDigitosTarjeta
{
    public int Valor { get; }

    public UltimosCuatroDigitosTarjeta(int valor)
    {
        if (valor < 1000 || valor > 9999)
            throw new ExcepcionDominio(nameof(Valor), "Debe contener exactamente 4 dígitos");

        Valor = valor;
    }
}

