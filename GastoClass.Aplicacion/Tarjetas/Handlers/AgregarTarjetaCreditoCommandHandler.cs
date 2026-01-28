using GastoClass.Aplicacion.Common;
using GastoClass.Aplicacion.Gastos.Commands;
using GastoClass.Dominio.Entidades;
using GastoClass.Dominio.Interfaces;
using GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;
using MediatR;

namespace GastoClass.Aplicacion.Gastos.Handlers;

public class AgregarTarjetaCreditoCommandHandler(
    IRepositorioTarjetaCredito repositorioTarjetaCredito, 
    IRepositorioPreferenciaTarjeta repositorioPreferenciaTarjeta) :
    IRequestHandler<AgragarTarjetaCreditoCommand, ResultadosValidacion>
{
    #region Agregar Gasto Handle

    public async Task<ResultadosValidacion> Handle(AgragarTarjetaCreditoCommand request, CancellationToken cancellationToken)
    {
            //Se mapea el Dto tarjeta de credito
            var tarjeta = new TarjetaCredito(
                        request.Id!.Value,
                        new TipoTarjeta(request.TipoTarjeta!),
                        new NombreTarjeta(request.Nombre!),
                        new UltimosCuatroDigitosTarjeta(request.UltimosCuatroDigitos!.Value),
                        new MesVencimiento(request.MesVencimiento!.Value),
                        new AnioVencimiento(request.AnioVencimiento!.Value),
                        new LimiteCredito(request.LimiteCredito!.Value),
                        new TipoMoneda(request.Moneda!),
                        new DiaCorte(request.DiaCorte!.Value),
                        new DiaPago(request.DiaPago!.Value),
                        new NombreBanco(request.NombreBanco!)
                    );
        var preferenciaTarjeta = new PreferenciaTarjeta(
            request.PreferenciaTarjeta!.Id,
            request.PreferenciaTarjeta.ColorHex1,
            request.PreferenciaTarjeta.ColorHex2,
            request.PreferenciaTarjeta.ColorBorde,
            request.PreferenciaTarjeta.ColorTexto,
            request.PreferenciaTarjeta.IconoTipoTarjeta,
            request.PreferenciaTarjeta.IconoChip);
        // Devolvera un conjunto de errores si la tarjeta de credito es invalida
        await repositorioTarjetaCredito.AgregarAsync(tarjeta);

        await repositorioPreferenciaTarjeta.AgregarAsync(preferenciaTarjeta);
        //retorna los resultados de las validaciones que se capturaron
        //usando ExcepcionBehavior
        return new ResultadosValidacion();
    }

    #endregion
}
