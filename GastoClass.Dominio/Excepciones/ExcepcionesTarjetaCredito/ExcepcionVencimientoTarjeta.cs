namespace GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

/// <summary>
/// Esta clase se encarga de manejar la excepción de vencimiento de tarjeta
/// </summary>
public class ExcepcionVencimientoTarjeta : ExcepcionDominio
{
    public ExcepcionVencimientoTarjeta() 
        : base("La tarjeta ha vencido") { }
}
