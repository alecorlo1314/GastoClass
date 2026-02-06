namespace GastoClass.Aplicacion.DTOs;

public class CreateGastoDto
{
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; }
    public int TarjetaId { get; set; }
    public string? Descripcion { get; set; }
    public string? Categoria { get; set; }
    public string? Comercio { get; set; }
    public string? Estado { get; set; }
    public string? NombreImagen { get; set; }
}
