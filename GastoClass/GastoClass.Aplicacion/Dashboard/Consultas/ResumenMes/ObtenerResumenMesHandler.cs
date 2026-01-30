using GastoClass.GastoClass.Aplicacion.Dashboard.DTOs;
using GastoClass.GastoClass.Dominio.Interfaces;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Dashboard.Consultas.ResumenMes;

public class ObtenerResumenMesHandler(
    IRepositorioGasto repositorioGasto)
    : IRequestHandler<ObtenerResumenMesConsulta, ResumenMesDto>
{
    public async Task<ResumenMesDto> Handle(ObtenerResumenMesConsulta request, CancellationToken cancellationToken)
    {
        return new ResumenMesDto
        {
            TotalGastado = await repositorioGasto.TotalMesAsync(request.Mes, request.Anio),
            CantidadTransacciones = await repositorioGasto.CantidadMesAsync(request.Mes, request.Anio)
        };
    }
}
