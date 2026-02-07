namespace GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

public sealed class GastoDuplicadoException : ExcepcionDominio, IExcepcionPopup
{
    public int GastoId { get; }

    public GastoDuplicadoException(int gastoId)
        : base("GastoDuplicado","El gasto ya fue registrado anteriormente")
    {
        GastoId = gastoId;
    }
}
