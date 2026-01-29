namespace GastoClass.Dominio.Excepciones;

/// <summary>
/// Esta clase se encarga de manejar las excepciones del dominio
/// </summary>
public class ExcepcionDominio : Exception
{
    public string Campo { get; }
    public ExcepcionDominio(string campo,string? mensaje) 
        : base(mensaje) 
    {
        Campo = campo;
    }
}
