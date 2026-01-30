namespace GastoClass.Dominio.Excepciones.ExcepcionesGasto;

public class ExcepcionCategoriaInvalida : ExcepcionDominio
{
    public ExcepcionCategoriaInvalida(string campo, string? mensaje) 
        : base(campo, mensaje) { }
}
