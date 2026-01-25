namespace GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;

/// <summary>
/// Esta clase se encarga de manejar la excepción de nombre de tarjeta inválido
/// Basandose en los siguientes criterios:
/// - El nombre de la tarjeta es nullo
/// - El nombre de la tarjeta es vacío
/// </summary>
public class ExceptionNombreTarjetaInvalida : ExcepcionDominio
{
    public ExceptionNombreTarjetaInvalida() 
        : base("El nombre de la tarjeta es inválido") { }
}
