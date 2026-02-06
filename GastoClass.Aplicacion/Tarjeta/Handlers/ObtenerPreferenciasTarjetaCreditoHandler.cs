using GastoClass.Dominio.Interfaces;
using GastoClass.GastoClass.Aplicacion.Tarjeta.Consultas;
using GastoClass.GastoClass.Aplicacion.Tarjeta.DTOs;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Tarjeta.Handlers;

class ObtenerPreferenciasTarjetaCredito(IRepositorioPreferenciaTarjeta repositorioPreferenciaTarjeta)
    : IRequestHandler<ObtenerPreferenciaTarjetaConsulta, PreferenciaTarjetaDto?>
{
    public async Task<PreferenciaTarjetaDto?> Handle(ObtenerPreferenciaTarjetaConsulta request, CancellationToken cancellationToken)
    {
       var preferencia = await repositorioPreferenciaTarjeta.ObtenerPorIdTarjeta(request.IdTarjeta!);
        //Mapear a entidad
        return new PreferenciaTarjetaDto
        {
            Id = request.IdTarjeta,
            ColorHex1 = preferencia!.ColorHex1.Valor,
            ColorHex2 = preferencia.ColorHex2.Valor,
            ColorBorde = preferencia.ColorBorde.Valor,
            ColorTexto = preferencia.ColorTexto.Valor,
            IconoChip = preferencia.IconoChip.Valor,
            IconoTipoTarjeta = preferencia.IconoTipoTarjeta.Valor
        };
    }
}
