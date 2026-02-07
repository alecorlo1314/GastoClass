using GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

namespace GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;

public readonly record struct DiaCorte
{
    public int Dia { get; }

    public DiaCorte(int dia)
    {
        if (string.IsNullOrWhiteSpace(dia.ToString()))
            throw new ExcepcionDiaCorteInvalido(nameof(dia), "Fecha es requerida");
        if (dia < 1) 
            throw new ExcepcionDiaCorteInvalido(nameof(dia), "No puede ser menor a 1");
        if(dia > 31) 
            throw new ExcepcionDiaCorteInvalido(nameof(dia), "No puede ser mayor a 31");
        Dia = dia;
    }
}
