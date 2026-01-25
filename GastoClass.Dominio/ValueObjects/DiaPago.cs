using GastoClass.Dominio.Excepciones;

namespace GastoClass.Dominio.ValueObjects;

public record DiaPago
{
    public int Dia { get; }

    public DiaPago(int diaPago)
    {
        if (diaPago < 1 || diaPago > 31 || string.IsNullOrWhiteSpace(diaPago.ToString()) || string.IsNullOrEmpty(diaPago.ToString()))
        {
            throw new ExcepcionDiaPagoInvalido();
        }
        Dia = diaPago;
    }
}
