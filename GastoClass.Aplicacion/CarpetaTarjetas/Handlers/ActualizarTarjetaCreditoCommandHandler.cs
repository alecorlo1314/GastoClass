using GastoClass.Aplicacion.CarpetaGastos.Commands;
using GastoClass.Aplicacion.Common;
using GastoClass.Dominio.Entidades;
using GastoClass.Dominio.Excepciones;
using GastoClass.Dominio.Interfaces;
using GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;
using MediatR;

namespace GastoClass.Aplicacion.CarpetaGastos.Handlers;

public class ActualizarTarjetaCreditoCommandHandler(IRepositorioTarjetaCredito repositorioTarjetaCredito) :
    IRequestHandler<ActualizarTarjetaCreditoCommand, ResultadosValidacion>
{
    public async Task<ResultadosValidacion> Handle(ActualizarTarjetaCreditoCommand request, CancellationToken cancellationToken)
    {
        var tarjeta = await repositorioTarjetaCredito.ObtenerPorIdAsync(request.IdTarjeta);

        if (tarjeta == null)
            throw new ExcepcionDominio("General", "Tarjeta no encontrada");
        //Se manda al dominio para el negocio
        tarjeta.ActualizarDatos(
            new TipoTarjeta(request.TipoTarjeta!),
            new NombreTarjeta(request.NombreTarjeta!),
            new TipoMoneda(request.TipoMoneda!),
            new NombreBanco(request.NombreBanco!)
            );
        //Realizamos la actualizacio si no sale algo mal
        await repositorioTarjetaCredito.ActualizarAsync(tarjeta);

        return new ResultadosValidacion();
    }
}
