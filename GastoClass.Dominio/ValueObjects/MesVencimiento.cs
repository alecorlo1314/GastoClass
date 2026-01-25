using GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

namespace GastoClass.Dominio.ValueObjects;

/// <summary>
/// Representa la fecha de vencimiento de la tarjeta
/// Valida que la fecha de vencimiento sea posterior a la fecha actual
/// </summary>
public record MesVencimiento
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
