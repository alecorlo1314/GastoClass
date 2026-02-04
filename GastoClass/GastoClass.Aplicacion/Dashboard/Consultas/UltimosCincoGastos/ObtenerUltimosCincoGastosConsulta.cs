using GastoClass.GastoClass.Aplicacion.Common;
using GastoClass.GastoClass.Aplicacion.Dashboard.DTOs;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Dashboard.Consultas.UltimosCincoGastos;

public record ObtenerUltimosCincoGastosConsulta
    : IRequest<ResultadoConsulta<List<UltimoCincoGastosDto>>>

{
}
