namespace GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

public class ExcepcionBancoLongitudInvalida : ExcepcionDominio
{
    public ExcepcionBancoLongitudInvalida() : base("Debe tener mas de 3 caracteres") { }
}
