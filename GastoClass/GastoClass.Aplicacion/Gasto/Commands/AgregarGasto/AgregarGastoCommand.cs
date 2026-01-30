using GastoClass.Aplicacion.Common;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Gasto.Commands.AgregarGasto;

public class AgregarGastoCommand
    : IRequest<ResultadosValidacion>
{
    public decimal Monto { get; init; }
    public DateTime Fecha { get; init; }
    public int TarjetaId { get; init; }
    public string? Descripcion { get; init; }
    public string? Categoria { get; init; }
    public string? Comercio { get; init; }
    public string? Estado { get; init; }
    public string? NombreImagen { get; init; }
}
