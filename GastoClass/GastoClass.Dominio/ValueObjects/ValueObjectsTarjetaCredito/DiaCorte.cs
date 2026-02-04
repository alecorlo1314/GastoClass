namespace GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;

public readonly record struct DiaCorte
{
    public int Dia { get; }

    public DiaCorte(int dia)
    {
        if (dia < 1 || dia > 31)
            throw new ExcepcionDominio(nameof(Dia), "El día de corte debe estar entre 1 y 31");

        Dia = dia;
    }
}

