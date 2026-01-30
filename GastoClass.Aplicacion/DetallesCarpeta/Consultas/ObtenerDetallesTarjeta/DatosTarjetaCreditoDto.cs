using MediatR;

namespace GastoClass.Aplicacion.DetallesCarpeta.Consultas.ObtenerDetallesTarjeta;

public class DatosTarjetaCreditoDto
    : IRequest
{
    public decimal BalanceDetalles { get; init; }
    public string? IconoChipDetalles { get; init; }
    public string? NombreBancoDetalles { get; init; }
    public int? MesVencimientoDetalles { get; init; }
    public int? AnioVencimientoDetalles { get; init; }
    public int UltimosCuatroDigitos { get; init; }
    public string? IconoTipoTarjetaDetalles { get; init; }
    public string? ColorHex1Detalles { get; init; }
    public string? ColorHex2Detalles { get; init; }
    public string? ColorBordeDetalles { get; init; }
    public string? ColorTextoDetalles { get; init; }
    }
