using GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

namespace GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;

public readonly record struct DiaCorte
{
    public int Dia { get; }

    public DiaCorte(int diaCorte)
    {
        if (diaCorte < 1 || diaCorte > 31 || string.IsNullOrWhiteSpace(diaCorte.ToString()) || string.IsNullOrEmpty(diaCorte.ToString()))
        {
            throw new ExcepcionDiaCorteInvalido();
        }
        Dia = diaCorte;
    }
}
