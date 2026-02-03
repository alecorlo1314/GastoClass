using GastoClass.GastoClass.Aplicacion.Dashboard.DTOs;
using GastoClass.GastoClass.Dominio.Interfaces;
using Infraestructura.Persistencia.Entidades;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Dashboard.Consultas.GastosPorCategoria;

public class ObtenerGastoPorCategoriaHandler( IRepositorioGasto repositorioGasto) :
    IRequestHandler<ObtenerGastosPorCategoriaConsulta, List<GastoPorCategoriaDto>?>
{
    public async Task<List<GastoPorCategoriaDto>?> Handle(ObtenerGastosPorCategoriaConsulta request, CancellationToken cancellationToken)
    {
        //Obtenemos todos los gastos
        var listaGastos = await repositorioGasto.ObtenerTodosAsync();
        //Convertimos el mes y año en un rango de fechas
        var inicioMes = new DateTime(request.anio, request.mes, 1);
        var finMes = inicioMes.AddMonths(1);
        //Consulta para obtener los gastos del mes y ano por categoria especificados
        var consulta = listaGastos!
            .Where(g => g.Fecha.Valor >= inicioMes && g.Fecha.Valor < finMes)
            .GroupBy(g => g.Categoria)
            .Select(g => new GastoPorCategoriaDto
            {
                //Se pasa la categoria
                Categoria = g.Key.Valor,
                //Se suma el monto por categoria
                TotalGastado = g.Sum(x => x.Monto.Valor)
                //Se ordena de mayor a menor
            }).OrderByDescending(g => g.TotalGastado);

        return consulta.ToList();
    }
}
