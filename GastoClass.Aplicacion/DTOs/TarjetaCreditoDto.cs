namespace GastoClass.Aplicacion.DTOs;
/// <summary>
/// Se encarga de la entrada / salida de la aplicacion
/// No recibe ViewModels, Entidades de forma directa desde la capa de Presentacion
/// </summary>
public class TarjetaCreditoDto
{
    public Guid Id { get; set; }
    public string? Tipo { get; set; }
    public string? Nombre { get; set; }
    public string? UltimosCuatroDigitos { get; set; }
    public int MesVencimiento { get; set; }
    public int AnioVencimiento { get; set; }
}
