namespace GastoClass.Dominio.Excepciones;

/// <summary>
/// Esta clase se encarga de manejar la excepción de número de tarjeta inválido
/// Con los siguientes criterios:
/// - El número de tarjeta es nullo o vacío
/// - El numero de no tiene 4 dígitos
/// - El numero de tarjeta no es numérico
/// </summary>
public class ExcepcionNumeroTarjetaInvalida : ExcepcionDominio
{
    public ExcepcionNumeroTarjetaInvalida() 
        : base("Los últimos 4 dígitos de la tarjeta no son válidos") { }
}
