using GastoClass.Aplicacion.DTOs;
using GastoClass.Dominio.Interfaces;
using GastoClass.GastoClass.Aplicacion.Tarjeta.Consultas;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Tarjeta.Handlers;

public class ObtenerTarjetasCreditoHandler(
    IRepositorioTarjetaCredito repositorioTarjetaCredito,
    IRepositorioPreferenciaTarjeta repositorioPreferenciaTarjeta)
    : IRequestHandler<ObtenerTarjetaCreditoConsulta, List<DetallesTarjetaDto>?>
{
    public async Task<List<DetallesTarjetaDto>?> Handle(ObtenerTarjetaCreditoConsulta request, CancellationToken cancellationToken)
    {
        //Consultar tarjetas de credito
        var TarjetasCredito = await repositorioTarjetaCredito.ObtenerTodosAsync();
        if (TarjetasCredito is null || TarjetasCredito.Count == 0)
            return new List<DetallesTarjetaDto>();

        var resultado = new List<DetallesTarjetaDto>();
        //Obtener preferencias de tarjetas de credito
        var preferenciasTarjetasCredito = await repositorioPreferenciaTarjeta.ObtenerTodosAsync();
        //Mapear tarjetas de credito con preferencias
        foreach (var tarjeta in TarjetasCredito)
        {
            var preferencia = preferenciasTarjetasCredito!.FirstOrDefault(t => t.Id == tarjeta.Id);
            if (preferencia is not null)
            {
                resultado.Add(DetallesTarjetaDto.DeEntidad(tarjeta, preferencia));
            }
        }
        return resultado;
    }
}
