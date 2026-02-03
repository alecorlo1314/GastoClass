namespace GastoClass.GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

public sealed class TarjetaInactivaException : ExcepcionDominio
{
    public int TarjetaId { get; }

    public TarjetaInactivaException(int tarjetaId)
        : base("TarjetaInactiva", "La tarjeta seleccionada no se encuentra activa")
    {
        TarjetaId = tarjetaId;
    }
}

