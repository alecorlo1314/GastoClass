using GastoClass.Dominio.Interfaces;
using MediatR;

namespace GastoClass.Aplicacion.Dashboard.Consultas.ObtenerResumenMensual;

public class ObtenerResumenMensualHandler(IRepositorioGasto repositorioGasto) :
    IRequestHandler<ObtenerResumenMensualConsulta, ResumenMesDto>
{
    public async Task<ResumenMesDto> Handle(ObtenerResumenMensualConsulta request, CancellationToken cancellationToken)
    {
        return new ResumenMesDto
        {
            TotalGastado = await repositorioGasto.TotalMesAsync(request.Mes, request.Anio),
            CantidadTransacciones = await repositorioGasto.CantidadMesAsync(request.Mes, request.Anio)
        };
    }
}
