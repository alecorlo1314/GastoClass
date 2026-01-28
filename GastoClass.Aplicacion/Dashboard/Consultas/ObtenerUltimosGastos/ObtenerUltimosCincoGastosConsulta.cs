using MediatR;

namespace GastoClass.Aplicacion.Dashboard.Consultas.ObtenerUltimosGastos;

public record ObtenerUltimosCincoGastosConsulta
    : IRequest<List<GastoUltimosCincoDto>>
{
}
