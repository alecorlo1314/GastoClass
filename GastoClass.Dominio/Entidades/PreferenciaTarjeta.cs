namespace GastoClass.Dominio.Entidades;

public class PreferenciaTarjeta
{
    public int Id { get; }
    public string? ColorHex1 { get; }
    public string? ColorHex2 { get; } 
    public string? ColorBorde { get;}
    public string? ColorTexto { get; }
    public string? IconoTipoTarjeta { get;}
    public string? IconoChip { get;} 

    public PreferenciaTarjeta(
        int id, string? colorHex1, string? colorHex2, 
        string? colorBorde, string? colorTexto, string? 
        iconoTipoTarjeta, string? iconoChip)
    {
        Id = id;
        ColorHex1 = colorHex1;
        ColorHex2 = colorHex2;
        ColorBorde = colorBorde;
        ColorTexto = colorTexto;
        IconoTipoTarjeta = iconoTipoTarjeta;
        IconoChip = iconoChip;
    }
}
