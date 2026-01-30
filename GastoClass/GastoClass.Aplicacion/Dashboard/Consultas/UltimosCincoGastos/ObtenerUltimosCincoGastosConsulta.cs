using GastoClass.GastoClass.Aplicacion.Dashboard.DTOs;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Dashboard.Consultas.UltimosCincoGastos;

public record ObtenerUltimosTresGastosConsulta
    : IRequest<List<UltimoCincoGastosDto>>
{
}
