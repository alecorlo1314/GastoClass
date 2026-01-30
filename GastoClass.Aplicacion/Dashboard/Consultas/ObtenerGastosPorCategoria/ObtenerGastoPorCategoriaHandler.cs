using GastoClass.Dominio.Interfaces;
using MediatR;

namespace GastoClass.Aplicacion.Dashboard.Consultas.ObtenerGastosPorCategoria;

public class ObtenerGastoPorCategoriaHandler(IRepositorioGasto repositorioGasto) :
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
