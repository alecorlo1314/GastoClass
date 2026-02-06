using GastoClass.Aplicacion.Common;
using GastoClass.Aplicacion.Dashboard.DTOs;
using MediatR;

namespace GastoClass.Aplicacion.Dashboard.Consultas.UltimosCincoGastos;

public record ObtenerUltimosCincoGastosConsulta
    : IRequest<ResultadoConsulta<List<UltimoCincoGastosDto>>>

{
}
