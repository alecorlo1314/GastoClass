using GastoClass.Dominio.Interfaces;
using GastoClass.Aplicacion.Dashboard.DTOs;
using MediatR;

namespace GastoClass.Aplicacion.Dashboard.Consultas.ResumenMes;

public class ObtenerResumenMesHandler(
    IRepositorioGasto repositorioGasto)
    : IRequestHandler<ObtenerResumenMesConsulta, ResumenMesDto>
{
    public async Task<ResumenMesDto> Handle(
        ObtenerResumenMesConsulta request, CancellationToken cancellationToken)
    {
        var resultado = new ResumenMesDto
        {
            TotalGastado = await repositorioGasto.TotalMesAsync(request.Mes, request.Anio),
            CantidadTransacciones = await repositorioGasto.CantidadMesAsync(request.Mes, request.Anio)
        };
        return resultado;
    }
}
