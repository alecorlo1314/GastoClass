namespace GastoClass.GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

public sealed class SaldoInsuficienteException : ExcepcionDominio
{
    public decimal SaldoActual { get; }
    public decimal MontoSolicitado { get; }

    public SaldoInsuficienteException(decimal saldoActual, decimal montoSolicitado)
        : base("SaldoInsuficiente","El saldo disponible no es suficiente para realizar el gasto")
    {
        SaldoActual = saldoActual;
        MontoSolicitado = montoSolicitado;
    }
}
