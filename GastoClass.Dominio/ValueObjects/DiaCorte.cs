using GastoClass.Dominio.Excepciones;

namespace GastoClass.Dominio.ValueObjects;

public record DiaCorte
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
