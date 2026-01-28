using MediatR;

namespace GastoClass.Aplicacion.Dashboard.Consultas.ObtenerResumenMensual;

public record ObtenerResumenMensualConsulta(int Mes, int Anio) 
    : IRequest<ResumenMesDto>
{
}
