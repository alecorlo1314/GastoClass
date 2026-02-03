namespace GastoClass.Dominio.Entidades;

public readonly record struct TipoMoneda
{
    public string? Tipo { get; }

    public TipoMoneda(string? TipoMoneda)
    {
        Tipo = TipoMoneda; 
    }
}
 