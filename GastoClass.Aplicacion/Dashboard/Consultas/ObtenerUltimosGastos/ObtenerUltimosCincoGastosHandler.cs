using GastoClass.Dominio.Interfaces;
using MediatR;

namespace GastoClass.Aplicacion.Dashboard.Consultas.ObtenerUltimosGastos;

public class ObtenerUltimosCincoGastosHandler(IRepositorioGasto repositorioGasto)
    : IRequestHandler<ObtenerUltimosCincoGastosConsulta, List<GastoUltimosCincoDto>>
{
    public async Task<List<GastoUltimosCincoDto>> Handle(ObtenerUltimosCincoGastosConsulta request, CancellationToken cancellationToken)
    {
        var resultado = await repositorioGasto.ObtenerTodosAsync();
        //realizar la seleccion de los cinco en orden descendente
        return resultado!.OrderBy(g => g.Fecha)
            .Take(5)
            .Select(g => new GastoUltimosCincoDto
            {
                Icono = g.NombreImagen!.Value.Valor,
                Descripcion = g.Descripcion.Valor,
                Fecha = g.Fecha.Valor!.Value,
                Monto = g.Monto.Valor
            }).ToList();
    }
}
