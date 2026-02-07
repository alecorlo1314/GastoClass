using GastoClass.GastoClass.Aplicacion.Gasto.DTOs;
using GastoClass.GastoClass.Dominio.Interfaces;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Gasto.Consultas.GastoPorDiaSemana;

public class ObtenerGastosPorTarjetaYRangoFechaHandler(IRepositorioGasto repositorioGasto)
    : IRequestHandler<ObtenerGastosPorTarjetaYRangoFechaConsulta, List<GastoPorDiaSemanaDto>>
{
    public async Task<List<GastoPorDiaSemanaDto>> Handle(ObtenerGastosPorTarjetaYRangoFechaConsulta request, CancellationToken cancellationToken)
    {
        // 1. Obtener gastos del repositorio
        var inicioMes = new DateTime(request.anio, request.mes, 1);
        var finMes = inicioMes.AddMonths(1);

        var gastos = await repositorioGasto.ObtenerTodosAsync();
        // 2. Agrupar y mapear a DTO
        var gastosAgrupados = gastos!
            .GroupBy(g => g.Fecha.Valor.DayOfWeek)
            .Select(grupo => new GastoPorDiaSemanaDto
            {
                DiaSemana = ObtenerNombreDiaSemana(grupo.Key),
                Total = grupo.Sum(g => g.Monto.Valor),
                NumeroDia = (int)grupo.Key,
                CantidadTransacciones = grupo.Count()
            })
            .ToList();

        // 3. Completar días faltantes con valor 0
        var todosLosDias = CompletarDiasFaltantes(gastosAgrupados);

        return todosLosDias;
    }
    private string ObtenerNombreDiaSemana(DayOfWeek dia)
    {
        return dia switch
        {
            DayOfWeek.Sunday => "Domingo",
            DayOfWeek.Monday => "Lunes",
            DayOfWeek.Tuesday => "Martes",
            DayOfWeek.Wednesday => "Miércoles",
            DayOfWeek.Thursday => "Jueves",
            DayOfWeek.Friday => "Viernes",
            DayOfWeek.Saturday => "Sábado",
            _ => dia.ToString()
        };
    }
    private List<GastoPorDiaSemanaDto> CompletarDiasFaltantes(
        List<GastoPorDiaSemanaDto> gastosExistentes)
    {
        var todosLosDias = new List<GastoPorDiaSemanaDto>();

        for (int i = 0; i < 7; i++)
        {
            var diaExistente = gastosExistentes.FirstOrDefault(g => g.NumeroDia == i);

            if (diaExistente != null)
            {
                todosLosDias.Add(diaExistente);
            }
            else
            {
                todosLosDias.Add(new GastoPorDiaSemanaDto
                {
                    DiaSemana = ObtenerNombreDiaSemana((DayOfWeek)i),
                    Total = 0,
                    NumeroDia = i,
                    CantidadTransacciones = 0
                });
            }
        }

        return todosLosDias.OrderBy(d => d.NumeroDia).ToList();
    }
}
