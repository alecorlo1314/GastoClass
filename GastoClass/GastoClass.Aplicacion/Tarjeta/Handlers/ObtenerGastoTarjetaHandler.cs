using GastoClass.Dominio.Interfaces;
using GastoClass.GastoClass.Aplicacion.Tarjeta.Consultas;
using GastoClass.GastoClass.Aplicacion.Tarjeta.DTOs;
using GastoClass.GastoClass.Dominio.Interfaces;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Tarjeta.Handlers;

public class ObtenerGastoTarjetaHandler(
    IRepositorioTarjetaCredito repositorioTarjetaCredito, 
    IRepositorioGasto repositorioGasto)
    : IRequestHandler<ObtenerGastoPorTarjetaCreditoConsulta, List<GastoTarjetaDto>?>
{
    public async Task<List<GastoTarjetaDto>?> Handle(
        ObtenerGastoPorTarjetaCreditoConsulta request, 
        CancellationToken cancellationToken)
    {
        //Consultas tarjetas
        var tarjetas = await repositorioTarjetaCredito.ObtenerTodosAsync();
        //consultas gastos
        var gastos = await repositorioGasto.ObtenerTodosAsync();

        //Agruparlos con join
        var resultado = (from g in gastos
                         join t in tarjetas! on g.TarjetaId.idTarjeta equals t.Id
                         group g by t.NombreTarjeta into grupo
                         select new GastoTarjetaDto
                         {
                             NombreTarjeta = grupo.Key.Valor,
                             BalanceTotal = grupo.Sum(x => x.Monto.Valor)
                         }).ToList();

        return resultado;
    }
}
