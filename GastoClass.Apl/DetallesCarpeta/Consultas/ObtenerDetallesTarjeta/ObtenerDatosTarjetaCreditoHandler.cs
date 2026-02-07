using GastoClass.Dominio.Interfaces;
using MediatR;

namespace GastoClass.Aplicacion.DetallesCarpeta.Consultas.ObtenerDetallesTarjeta;

public class ObtenerDatosTarjetaCreditoHandler(
    IRepositorioTarjetaCredito repositorioTarjetaCredito, 
    IRepositorioPreferenciaTarjeta repositorioPreferenciaTarjeta)
    : IRequestHandler<ObtenerDatosTarjetaCreditoConsulta, DatosTarjetaCreditoDto>
{
    public async Task<DatosTarjetaCreditoDto> Handle(ObtenerDatosTarjetaCreditoConsulta request, CancellationToken cancellationToken)
    {
        //Obtiene la tarjeta de credito que le pertenece
        var preferenciasTarjeta = await repositorioPreferenciaTarjeta.ObtenerPorIdTarjeta(request.IdTarjeta);
        //Obtener la tarjeta de credito
        var tarjetaCredito = await repositorioTarjetaCredito.ObtenerPorIdAsync(request.IdTarjeta);
        //retornar detalles tarjeta
        return new DatosTarjetaCreditoDto
        {
            BalanceDetalles = tarjetaCredito!.Balance,
            IconoChipDetalles = preferenciasTarjeta!.IconoChip,
            NombreBancoDetalles = tarjetaCredito.NombreTarjeta.Valor,
            MesVencimientoDetalles = tarjetaCredito.MesVencimiento.Mes,
            UltimosCuatroDigitos = tarjetaCredito.UltimosCuatroDigitos.Valor,
            IconoTipoTarjetaDetalles = preferenciasTarjeta.IconoTipoTarjeta,
            ColorHex1Detalles = preferenciasTarjeta.ColorHex1,
            ColorHex2Detalles = preferenciasTarjeta.ColorHex2,
            ColorBordeDetalles = preferenciasTarjeta.ColorBorde,
            ColorTextoDetalles = preferenciasTarjeta.ColorTexto
        };
    }
}
