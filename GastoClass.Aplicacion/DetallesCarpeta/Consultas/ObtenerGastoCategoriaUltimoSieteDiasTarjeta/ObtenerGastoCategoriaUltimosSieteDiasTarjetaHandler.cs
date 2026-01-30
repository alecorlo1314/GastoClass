using GastoClass.Dominio.Interfaces;
using MediatR;

namespace GastoClass.Aplicacion.DetallesCarpeta.Consultas.ObtenerGastoCategoriaUltimoSieteDiasTarjeta;

public class ObtenerGastoCategoriaUltimosSieteDiasTarjetaHandler(
    IRepositorioGasto repositorioGasto)
    : IRequestHandler<ObtenerGastoCategoriaUltimosSieteDiasTarjetaConsulta, 
        IEnumerable<GastoCategoriaUltimosSieteDiasTarjetaDto>>
{
    public async Task<IEnumerable<GastoCategoriaUltimosSieteDiasTarjetaDto>> Handle(
        ObtenerGastoCategoriaUltimosSieteDiasTarjetaConsulta request, CancellationToken cancellationToken)
    {
        var gastos = await repositorioGasto.ObtenerTodosAsync();

        var fechaLimite = DateTime.Now.AddDays(-7);

        var query = from g in gastos
                    where g.TarjetaId == request.IdTarjeta
                    where g.Fecha.Valor >= fechaLimite
                    group g by new
                    {
                        Dia = g.Fecha.Valor!.Value.DayOfWeek,
                        g.Categoria
                    }
                    into grupo
                    select new GastoCategoriaUltimosSieteDiasTarjetaDto
                    {
                        Dia = grupo.Key.Dia.ToString(),
                        Categoria = grupo.Key.Categoria.Valor!,
                        TotalMonto = grupo.Sum(x => x.Monto.Valor)
                    };

        return query.ToList();
    }
}
