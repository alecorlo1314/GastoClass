using GastoClass.GastoClass.Aplicacion.Common;
using GastoClass.GastoClass.Aplicacion.Dashboard.DTOs;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Dashboard.Consultas.ResumenMes;

public record ObtenerResumenMesConsulta(int Mes, int Anio)
    : IRequest<ResultadoConsulta<ResumenMesDto>> { }
