using GastoClass.Aplicacion.Dashboard.DTOs;
using MediatR;

namespace GastoClass.Aplicacion.Dashboard.Consultas.ResumenMes;

public record ObtenerResumenMesConsulta(int Mes, int Anio)
    : IRequest<ResumenMesDto> { }
