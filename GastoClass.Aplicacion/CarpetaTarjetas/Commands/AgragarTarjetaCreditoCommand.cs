using GastoClass.Aplicacion.Common;
using GastoClass.Aplicacion.DTOs;
using MediatR;

namespace GastoClass.Aplicacion.CarpetaGastos.Commands;

public class AgragarTarjetaCreditoCommand : 
    IRequest<ResultadosValidacion>
{
    public int? Id { get; init; }
    public string? TipoTarjeta {  get; init; }
    public string? Nombre { get; init; }
    public int? UltimosCuatroDigitos { get; init; }
    public int? MesVencimiento { get; init; }
    public int? AnioVencimiento { get; init; }
    public decimal? LimiteCredito{  get; init; }
    public decimal? Balance { get; init; }
    public decimal? CreditoDisponible { get; init; }
    public string? Moneda { get; init; }
    public int? DiaCorte { get; init; }
    public int? DiaPago { get; init; }
    public string? NombreBanco { get; init; }
    public int idPreferencia { get; init; }
    public PreferenciasTarjetaCreditoDto? PreferenciaTarjeta { get; init; }
}
