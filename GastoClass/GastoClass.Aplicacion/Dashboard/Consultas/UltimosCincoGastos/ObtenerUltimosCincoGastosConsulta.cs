using GastoClass.GastoClass.Aplicacion.Common;
using GastoClass.GastoClass.Aplicacion.Dashboard.DTOs;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Dashboard.Consultas.UltimosCincoGastos;

public record ObtenerUltimosTresGastosConsulta
    : IRequest<ResultadoConsulta<List<UltimoCincoGastosDto>>>

{
}
