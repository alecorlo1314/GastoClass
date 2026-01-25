using GastoClass.Dominio.Excepciones;

namespace GastoClass.Dominio.ValueObjects;

/// <summary>
/// Representa la fecha de vencimiento de la tarjeta
/// Valida que la fecha de vencimiento sea posterior a la fecha actual
/// </summary>
public record FechaVencimiento
{
    public int Mes { get;}
    public int Anio { get;}

    public FechaVencimiento(int mes, int anio)
    {
        if (mes < 1 || mes > 12 
            || string.IsNullOrWhiteSpace(mes.ToString()) 
            || string.IsNullOrEmpty(anio.ToString()))
            throw new ExcepcionFechaInvalidaExpiracion();

        var vencimiento = new DateTime(mes, anio, DateTime.DaysInMonth(anio, mes));

        if(vencimiento < DateTime.UtcNow.Date)
            throw new ExcepcionFechaInvalidaExpiracion();

        Mes = mes;
        Anio = anio;
    }
}
