using GastoClass.GastoClass.Aplicacion.Dashboard.DTOs;
using GastoClass.GastoClass.Dominio.Interfaces;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Dashboard.Consultas.GastosPorCategoria;

public class ObtenerGastoPorCategoriaHandler( IRepositorioGasto repositorioGasto) :
    IRequestHandler<ObtenerGastosPorCategoriaConsulta, List<GastoPorCategoriaDto>?>
{
    public async Task<List<GastoPorCategoriaDto>?> Handle(ObtenerGastosPorCategoriaConsulta request, CancellationToken cancellationToken)
    {
        var listaGastoPorCategoria = await repositorioGasto.GastoPorCategoriaMes(request.mes, request.anio);
        //mapear a dt
        List<GastoPorCategoriaDto> gastoPorCategorioDtos = new();
        return listaGastoPorCategoria!.Select(x => new GastoPorCategoriaDto
        {
            Categoria = x.Categoria.Valor,
            TotalGastado = x.Monto.Valor
        }).ToList();
    }
}
