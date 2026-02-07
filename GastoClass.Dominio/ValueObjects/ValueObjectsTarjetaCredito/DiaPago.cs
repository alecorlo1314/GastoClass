using GastoClass.Dominio.Excepciones;

namespace GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;

public readonly record struct DiaPago
{
    public int Dia { get; }

    public DiaPago(int dia)
    {
        if (dia < 1 || dia > 31)
            throw new ExcepcionDominio(nameof(Dia), "El día de pago debe estar entre 1 y 31");

        Dia = dia;
    }
}

