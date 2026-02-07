using GastoClass.Aplicacion.Common;
using MediatR;

namespace GastoClass.Aplicacion.Gasto.Commands.AgregarGasto;

public record AgregarGastoCommand
    : IRequest<ResultadosValidacion>
{
    public decimal? MontoCommand { get; init; }
    public DateTime? FechaCommand { get; init; }
    public int? TarjetaIdCommand { get; init; }
    public string? DescripcionCommand { get; init; }
    public string? CategoriaCommand { get; init; }
    public string? ComercioCommand { get; init; }
    public string? EstadoCommand { get; init; }
    public string? NombreImagenCommand { get; init; }
}
