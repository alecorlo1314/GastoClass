using GastoClass.Dominio.Interfaces;
using MediatR;

namespace GastoClass.Aplicacion.Dashboard.Consultas.ObtenerUltimosGastos;

public class ObtenerUltimosTresGastosHandler(IRepositorioGasto repositorioGasto)
    : IRequestHandler<ObtenerUltimosTresGastosConsulta, List<GastoUltimosTresDto>>
{
    public async Task<List<GastoUltimosTresDto>> Handle(ObtenerUltimosTresGastosConsulta request, CancellationToken cancellationToken)
    {
        var resultado = await repositorioGasto.ObtenerTodosAsync();
        //realizar la seleccion de los cinco en orden descendente
        return resultado!.OrderBy(g => g.Fecha)
            .Take(5)
            .Select(g => new GastoUltimosTresDto
            {
                Icono = g.NombreImagen!.Value.Valor,
                Descripcion = g.Descripcion.Valor,
                Fecha = g.Fecha.Valor!.Value,
                Monto = g.Monto.Valor
            }).ToList();
    }
}
