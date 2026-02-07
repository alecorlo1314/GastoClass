using GastoClass.Dominio.Excepciones;

namespace GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;

/// <summary>
/// Representa la fecha de vencimiento de la tarjeta
/// Valida que la fecha de vencimiento sea posterior a la fecha actual
/// </summary>
public readonly record struct MesVencimiento
{
    public int Mes { get; }

    public MesVencimiento(int mes)
    {
        if (mes < 1 || mes > 12)
            throw new ExcepcionDominio(nameof(Mes), "El mes de vencimiento debe estar entre 1 y 12");

        Mes = mes;
    }
}

