using GastoClass.Dominio.Excepciones;

namespace GastoClass.GastoClass.Dominio.Excepciones.ExcepcionesGasto;

public class ExcepcionComercioMaximoCaracteres : ExcepcionDominio
{
    public ExcepcionComercioMaximoCaracteres(string campo, string? mensaje) : base(campo, mensaje) { }
}
