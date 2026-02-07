using GastoClass.Aplicacion.Tarjeta.Consultas;
using GastoClass.Aplicacion.Tarjeta.DTOs;
using GastoClass.Dominio.Interfaces;
using MediatR;

namespace GastoClass.Aplicacion.Tarjeta.Handlers;

class ObtenerGastosPorCategoriaSemanaHandler(IRepositorioGasto repositorioGastos)
    : IRequestHandler<ObtenerGastosPorCategoriaSemanaConsulta, List<GastoCategoriaUltimosSieteDiasTarjetaDto>?>
{
    public async Task<List<GastoCategoriaUltimosSieteDiasTarjetaDto>?> Handle(ObtenerGastosPorCategoriaSemanaConsulta request, CancellationToken cancellationToken)
    {
        var fechaInicio = DateTime.Now.AddDays(-7);
        var fechaFin = DateTime.Now;

        //Obtener datos de gastos
        var gastos = await repositorioGastos.ObtenerPorTarjetaAsync(request.idTarjeta);
        if (gastos!.Count() <= 0)
        {
            return null;
        }

        //Agrupar y mapear a DTO
        var gastosAgrupados = gastos!
            .Where(g =>
            g.Fecha.Valor >= fechaInicio &&
            g.Fecha.Valor <= fechaFin)
            .GroupBy(g => new
            {
            Dia = g.Fecha.Valor.DayOfWeek,
            g.Categoria
            })
            .Select(grupo => new GastoCategoriaUltimosSieteDiasTarjetaDto
            {
            Categoria = grupo.Key.Categoria.Valor,
            DiaSemana = ObtenerNombreDiaSemana(grupo.Key.Dia),
            NumeroDia = (int)grupo.Key.Dia,
            TotalMonto = grupo.Sum(g => g.Monto.Valor)
            })
            .ToList();

        //Completar días faltantes con valor 0
        var todosLosDias = CompletarDiasPorCategoria(gastosAgrupados);

        return todosLosDias;
    }

    private List<GastoCategoriaUltimosSieteDiasTarjetaDto> CompletarDiasPorCategoria(
    List<GastoCategoriaUltimosSieteDiasTarjetaDto> datos)
    {
        var resultado = new List<GastoCategoriaUltimosSieteDiasTarjetaDto>();

        var categorias = datos.Select(d => d.Categoria).Distinct();
        var dias = Enumerable.Range(0, 7);

        foreach (var categoria in categorias)
        {
            foreach (var dia in dias)
            {
                var existente = datos.FirstOrDefault(d =>
                    d.Categoria == categoria &&
                    d.NumeroDia == dia);

                resultado.Add(existente ?? new GastoCategoriaUltimosSieteDiasTarjetaDto
                {
                    Categoria = categoria,
                    NumeroDia = dia,
                    DiaSemana = ObtenerNombreDiaSemana((DayOfWeek)dia),
                    TotalMonto = 0
                });
            }
        }

        return resultado
            .OrderBy(r => r.NumeroDia)
            .ToList();
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
