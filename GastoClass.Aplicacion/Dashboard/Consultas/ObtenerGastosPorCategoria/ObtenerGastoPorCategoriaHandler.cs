using GastoClass.Dominio.Interfaces;
using MediatR;

namespace GastoClass.Aplicacion.Dashboard.Consultas.ObtenerGastosPorCategoria;

public class ObtenerGastoPorCategoriaHandler(IRepositorioGasto repositorioGasto) :
    IRequestHandler<ObtenerGastosPorCategoriaConsulta, List<GastoPorCategorioDto>?>
{
    public async Task<List<GastoPorCategorioDto>?> Handle(ObtenerGastosPorCategoriaConsulta request, CancellationToken cancellationToken)
    {
        var listaGastoPorCategoria = await repositorioGasto.GastoPorCategoriaMes(request.mes, request.anio);
        //mapear a dt
        List<GastoPorCategorioDto> gastoPorCategorioDtos = new();
        return listaGastoPorCategoria!.Select(x => new GastoPorCategorioDto
        {
            Categoria = x.Categoria.Valor,
            TotalGastado = x.Monto.Valor
        }).ToList();
    }
}
