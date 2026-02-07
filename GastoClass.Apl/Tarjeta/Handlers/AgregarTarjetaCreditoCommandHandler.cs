using GastoClass.Aplicacion.Common;
using GastoClass.Aplicacion.Tarjeta.Commands;
using GastoClass.Dominio.Entidades;
using GastoClass.Dominio.Interfaces;
using MediatR;

namespace GastoClass.Aplicacion.CarpetaGastos.Handlers;

public class AgregarTarjetaCreditoCommandHandler(
    IRepositorioTarjetaCredito repositorioTarjetaCredito, 
    IRepositorioPreferenciaTarjeta repositorioPreferenciaTarjeta) :
    IRequestHandler<AgragarTarjetaCreditoCommand, ResultadosValidacion>
{
    #region Agregar Gasto Handle

    public async Task<ResultadosValidacion> Handle(AgragarTarjetaCreditoCommand request, CancellationToken cancellationToken)
    {
        var resultado = new ResultadosValidacion();

        var preferencia = PreferenciaTarjetaDominio.Crear(
                ColorHex1: request.ColorHex1,
                ColorHex2: request.ColorHex2,
                ColorBorde: request.ColorBorde,
                ColorTexto: request.ColorTexto,
                IconoChip: request.IconoChip,
                IconoTipoTarjeta: request.IconoTipoTarjeta
            );

        var tarjeta = TarjetaCreditoDominio.Crear(
                tipo: request.Tipo!,
                nombre: request.Nombre!,
                ultimosCuatroDigitos: request.UltimosCuatroDigitos,
                mesVencimiento: request.MesVencimiento,
                anioVencimiento: request.AnioVencimiento,
                tipoMoneda: request.TipoMoneda!,
                limiteCredito: request.LimiteCredito,
                diaCorte: request.DiaCorte,
                diaPago: request.DiaPago,
                nombreBanco: request.NombreBanco!,
                preferencia: preferencia
            );

        await repositorioTarjetaCredito.AgregarAsync(tarjeta);

        //await repositorioPreferenciaTarjeta.AgregarAsync(preferenciaTarjeta);
        //retorna los resultados de las validaciones que se capturaron
        //usando ExcepcionBehavior
        return new ResultadosValidacion();
    }

    #endregion
}
