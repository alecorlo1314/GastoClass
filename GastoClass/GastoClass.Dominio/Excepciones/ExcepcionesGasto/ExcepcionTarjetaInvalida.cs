using GastoClass.Dominio.Excepciones;

namespace GastoClass.GastoClass.Dominio.Excepciones.ExcepcionesGasto;

public class ExcepcionTarjetaInvalida : ExcepcionDominio
{
    public ExcepcionTarjetaInvalida(string campo, string? mensaje) : base(campo, mensaje) { }
}
