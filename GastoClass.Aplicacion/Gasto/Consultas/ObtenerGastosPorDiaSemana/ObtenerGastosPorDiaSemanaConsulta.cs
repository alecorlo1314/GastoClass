using GastoClass.Aplicacion.Gasto.DTOs;
using MediatR;

namespace GastoClass.Aplicacion.Gasto.Consultas.ObtenerGastosPorDiaSemana;

public record ObtenerGastosPorDiaSemanaConsulta(int IdTarjeta)
    : IRequest<List<GastoPorDiaSemanaDto>?>
{
}
