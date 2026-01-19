using SQLite;

namespace GastoClass.Dominio.Model;

public class PreferenciaTarjeta
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string? ColorHex1 { get; set; } //Contiene el color hexadecimal del LinearGradient
    public string? ColorHex2 { get; set; } //Contiene el color hexadecimal del LinearGradient
    public string? ColorBorde { get; set; } // Contiene el color hexadecimal del borde
}
