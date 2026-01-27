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
        if (mesVencimiento < 1 || mesVencimiento > 12 ||
            string.IsNullOrWhiteSpace(mesVencimiento.ToString()) || 
            string.IsNullOrEmpty(mesVencimiento.ToString()))
        {
            throw new ExcepcionMesVencimientoInvalido();
        }
        Mes = mesVencimiento;
    }
}
