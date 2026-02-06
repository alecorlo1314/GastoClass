using GastoClass.Dominio.Interfaces;
using GastoClass.GastoClass.Aplicacion.Tarjeta.Consultas;
using GastoClass.GastoClass.Aplicacion.Tarjeta.DTOs;
using GastoClass.GastoClass.Dominio.Interfaces;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Tarjeta.Handlers;

class ObtenerGastosPorCategoriaSemanaHandler(IRepositorioTarjetaCredito repositorioTarjetaCredito, IRepositorioGasto repositorioGastos)
    : IRequestHandler<ObtenerGastosPorCategoriaSemanaConsulta, List<GastoCategoriaUltimosSieteDiasTarjetaDto>?>
{
    public async Task<List<GastoCategoriaUltimosSieteDiasTarjetaDto>?> Handle(ObtenerGastosPorCategoriaSemanaConsulta request, CancellationToken cancellationToken)
    {
        var gastos = await repositorioGastos.ObtenerTodosAsync();

        var fechaLimite = DateTime.Now.AddDays(-7);

        var query = from g in gastos
                    where g.TarjetaId.IdTarjeta == request.idTarjeta
                    where g.Fecha.Valor >= fechaLimite
                    group g by new
                    {
                        Dia = g.Fecha.Valor.DayOfWeek,
                        g.Categoria
                    }
                    into grupo
                    select new GastoCategoriaUltimosSieteDiasTarjetaDto
                    {
                        Dia = grupo.Key.Dia.ToString(),
                        Categoria = grupo.Key.Categoria.Valor,
                        TotalMonto = grupo.Sum(x => x.Monto.Valor)
                    };
        return query.ToList();
    }
}
