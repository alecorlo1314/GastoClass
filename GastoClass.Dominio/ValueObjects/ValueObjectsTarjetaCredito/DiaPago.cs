using GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

namespace GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;

public readonly record struct DiaPago
{
    public int Dia { get; }

    public DiaPago(int diaPago)
    {
        Dia = diaPago;
        if (string.IsNullOrWhiteSpace(diaPago.ToString()))
            throw new ExcepcionDiaPagoInvalido(nameof(diaPago), "Dia es requerido");
        if (diaPago < 1)
            throw new ExcepcionDiaPagoInvalido(nameof(diaPago), "No puede ser menor a 1");
        if (diaPago > 31)
            throw new ExcepcionDiaPagoInvalido(nameof(diaPago), "No puede ser mayor a 31");
        Dia = diaPago;
    }
}
