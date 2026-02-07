using GastoClass.Aplicacion.Gasto.DTOs;
using GastoClass.Dominio.Interfaces;
using MediatR;

namespace GastoClass.Aplicacion.Gasto.Consultas.ObtenerGastosPorDiaSemana;

public class ObtenerGastosPorDiaSemanaHandler(IRepositorioGasto repositorioGasto)
    : IRequestHandler<ObtenerGastosPorDiaSemanaConsulta, List<GastoPorDiaSemanaDto>?>
{
    public async Task<List<GastoPorDiaSemanaDto>?> Handle(ObtenerGastosPorDiaSemanaConsulta request, CancellationToken cancellationToken)
    {
		try
		{
            var fechaInicio = DateTime.Now.AddDays(-7);
            var fechaFin = DateTime.Now;
            //Obtener datos de gastos
            var gastos = await repositorioGasto.ObtenerPorTarjetaAsync(request.IdTarjeta);
            if(gastos!.Count() <= 0)
            {
                return null;
            }
            //Agrupar y mapear a DTO
            var gastosAgrupados = gastos!
                .Where(g =>
                g.Fecha.Valor >= fechaInicio &&
                g.Fecha.Valor <= fechaFin).
                GroupBy(g => g.Fecha.Valor.DayOfWeek)
                .Select(grupo => new GastoPorDiaSemanaDto
                {
                    DiaSemana = ObtenerNombreDiaSemana(grupo.Key),
                    Total = grupo.Sum(g => g.Monto.Valor),
                    NumeroDia = (int)grupo.Key,
                    CantidadTransacciones = grupo.Count()
                })
                .ToList();
            //Completar días faltantes con valor 0
            var todosLosDias = CompletarDiasFaltantes(gastosAgrupados);

            return todosLosDias;
        }
		catch (Exception)
		{

			throw;
		}
    }

    private List<GastoPorDiaSemanaDto> CompletarDiasFaltantes(List<GastoPorDiaSemanaDto> gastosExistentes)
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
}
