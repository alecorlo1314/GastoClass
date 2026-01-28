using SQLite;
namespace Infraestructura.Persistencia.Entidades;

public class PreferenciasTarjetaEntidad
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string? ColorHex1 { get; set; } //Contiene el color hexadecimal del LinearGradient
    public string? ColorHex2 { get; set; } //Contiene el color hexadecimal del LinearGradient
    public string? ColorBorde { get; set; } // Contiene el color hexadecimal del borde
    public string? ColorTexto { get; set; } // Contiene el color hexadecimal del texto
    public string? IconoTipoTarjeta { get; set; } //Contiene visa, mastercard o amex
    public string? IconoChip { get; set; } // Contiene el color icono del chip
}
