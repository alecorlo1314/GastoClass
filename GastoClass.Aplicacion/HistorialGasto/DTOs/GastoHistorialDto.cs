namespace GastoClass.Aplicacion.HistorialGasto.DTOs;

public class GastoHistorialDto
{
    public string? Descripcion { get; init; }
    public int Id { get; init; }
    public string? Categoria { get; init; }
    public int TarjetaId { get; init; }
    public DateTime Fecha { get; init; }
    public decimal Monto { get; init; }
}
