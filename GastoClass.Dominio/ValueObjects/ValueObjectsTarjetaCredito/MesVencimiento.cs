using GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

namespace GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;

/// <summary>
/// Representa la fecha de vencimiento de la tarjeta
/// Valida que la fecha de vencimiento sea posterior a la fecha actual
/// </summary>
public readonly record struct MesVencimiento
{
    public int Mes { get;}

    public MesVencimiento(int mesVencimiento)
    {
        if (string.IsNullOrWhiteSpace(mesVencimiento.ToString()))
            throw new ExcepcionMesVencimientoInvalido(nameof(mesVencimiento), "Mes es requerido");
        if(mesVencimiento < 1) 
            throw new ExcepcionMesVencimientoInvalido(nameof(mesVencimiento), "No puede ser menor a 1");
        if(mesVencimiento > 12) 
            throw new ExcepcionMesVencimientoInvalido(nameof(mesVencimiento), "No puede ser mayor a 12");
        Mes = mesVencimiento;
    }
}
