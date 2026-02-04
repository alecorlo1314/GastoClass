using GastoClass.GastoClass.Aplicacion.Dashboard.DTOs;
using GastoClass.GastoClass.Dominio.Interfaces;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Dashboard.Consultas.UltimosCincoGastos;

public class ObtenerUltimosTresGastosHandler(IRepositorioGasto repositorioGasto)
    : IRequestHandler<ObtenerUltimosTresGastosConsulta, List<UltimoCincoGastosDto>>
{
    public async Task<List<UltimoCincoGastosDto>> Handle(ObtenerUltimosTresGastosConsulta request, CancellationToken cancellationToken)
    {
        var resultado = await repositorioGasto.ObtenerTodosAsync();
        //realizar la seleccion de los cinco en orden descendente
        return resultado!.OrderByDescending(g => g.Fecha.Valor!)
            .Take(5)
            .Select(g => new UltimoCincoGastosDto
            {
                Icono = g.NombreImagen!.Value.Valor,
                Descripcion = g.Descripcion.Valor,
                Fecha = g.Fecha.Valor!,
                Monto = g.Monto.Valor
            }).ToList();
    }
}
