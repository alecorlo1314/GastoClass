using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Dashboard.ResumenMes;

public record ObtenerResumenMesConsulta(int Mes, int Anio)
    : IRequest<ResumenMesDto> { }
