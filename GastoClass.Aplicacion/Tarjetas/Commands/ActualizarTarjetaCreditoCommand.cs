using GastoClass.Aplicacion.Common;
using MediatR;

namespace GastoClass.Aplicacion.Gastos.Commands;

public class ActualizarTarjetaCreditoCommand :
    IRequest<ResultadosValidacion>
{
    public int IdTarjeta { get; init; }
    public string? NombreTarjeta { get; init; }
    public string? TipoTarjeta { get; init; }
    public string? TipoMoneda { get; init; }
    public string? NombreBanco { get; init; }
}
