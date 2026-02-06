using GastoClass.Aplicacion.Common;
using MediatR;

namespace GastoClass.Aplicacion.Tarjeta.Commands;

public class AgragarTarjetaCreditoCommand : 
    IRequest<ResultadosValidacion>
{
    public string? Tipo { get; set; }
    public string? Nombre { get; set; }
    public int UltimosCuatroDigitos { get; set; }
    public int MesVencimiento { get; set; }
    public int AnioVencimiento { get; set; }
    public string? TipoMoneda { get; set; }
    public decimal LimiteCredito { get; set; }
    public int DiaCorte { get; set; }
    public int DiaPago { get; set; }
    public string? NombreBanco { get; set; }

    // Preferencias visuales
    public string? ColorHex1 { get; init; }
    public string? ColorHex2 { get; init; }
    public string? ColorBorde { get; init; }
    public string? ColorTexto { get; init; }
    public string? IconoTipoTarjeta { get; init; }
    public string? IconoChip { get; init; }
}
