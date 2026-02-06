namespace GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

public sealed class LimiteCreditoExcedidoException : ExcepcionDominio, IExcepcionPopup
{
    public decimal Limite { get; }
    public decimal TotalActual { get; }

    public LimiteCreditoExcedidoException(decimal limite, decimal totalActual)
        : base("LiminiteCreditoExcedido", "Se ha excedido el límite de crédito de la tarjeta")
    {
        Limite = limite;
        TotalActual = totalActual;
    }
}
