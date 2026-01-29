using MediatR;

namespace GastoClass.Aplicacion.Dashboard.Consultas.ObtenerUltimosGastos;

public record ObtenerUltimosTresGastosConsulta
    : IRequest<List<UltimoCincoGastosDto>>
{
}
