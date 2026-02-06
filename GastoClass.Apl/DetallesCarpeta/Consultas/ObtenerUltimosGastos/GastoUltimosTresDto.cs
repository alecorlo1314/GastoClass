namespace GastoClass.Aplicacion.DetallesCarpeta.Consultas.ObtenerUltimosGastos;

public class GastoUltimosTresDto
{
    public string? IconoCategoriaGasto { get; set; }
    public string? DescripcionGasto { get; set; }
    public DateTime FechaGasto { get; set; }
    public string? CategoriaGasto { get; set; }
    public int UltimosCuatroDigitosTarjeta { get; set; }
    public string? EstadoGasto { get; set; }
    public decimal MontoGasto { get; set; }
}
