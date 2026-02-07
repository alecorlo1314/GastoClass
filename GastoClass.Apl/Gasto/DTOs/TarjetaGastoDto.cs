namespace GastoClass.GastoClass.Aplicacion.Gasto.DTOs;

public class TarjetaGastoDto
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public int UltimosCuatroDigitos { get; set; }
    public string? TipoTarjetaIcono { get; set; }
}
