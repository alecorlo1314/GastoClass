namespace GastoClass.Dominio.Excepciones;

/// <summary>
/// Esta clase se encarga de manejar la excepción de fecha inválida de expiración
/// Con los siguientes criterios:
/// - El mes es menor a 1 o mayor a 12
/// - El mes es nullo
/// - El año es nullo
/// </summary>
public class ExcepcionFechaInvalidaExpiracion : ExcepcionDominio
{
    public ExcepcionFechaInvalidaExpiracion() 
        : base("La fecha de expiración es inválida") { }
}
