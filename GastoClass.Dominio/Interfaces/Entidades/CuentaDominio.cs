namespace GastoClass.Dominio.Entidades;

public class CuentaDominio
{
    public Guid Id { get; set; }
    public string NombreCuenta { get; set; }//Ej: Cuenta Salario, Multimoney, Cuenta Ahorros
    public string NombreEntidad { get; set; }//Ej: BAC, BCR, Davivienda, Fintech
    public string TipoCuenta { get; set; }//Cuenta bancaria,tarjeta de debito, tarjeta virtual
    public string TipoMoneda { get; set; }// CRC o USD

    public decimal SaldoInicial { get; set; }
    public decimal SaldoActual { get; private set; }
    public bool EsCuentaPrincipal { get; set; }

    //public List<MovimientoDebito> Movimientos { get; set; } = new();

    public void RegistrarIngreso(decimal monto)
    {
        SaldoActual += monto;
    }

    public void RegistrarGasto(decimal monto)
    {
        if (SaldoActual < monto)
            throw new InvalidOperationException("Saldo insuficiente");

        SaldoActual -= monto;
    }
}
