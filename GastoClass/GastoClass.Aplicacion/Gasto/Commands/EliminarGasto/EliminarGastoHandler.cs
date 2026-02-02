using GastoClass.Aplicacion.Common;
using GastoClass.Dominio.Excepciones;
using GastoClass.Dominio.Interfaces;
using GastoClass.GastoClass.Dominio.Interfaces;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Gasto.Commands.EliminarGasto;

public class EliminarGastoHandler(
    IRepositorioGasto repositorioGasto, 
    IRepositorioTarjetaCredito repositorioTarjetaCredito) : 
    IRequestHandler<EliminarGastoCommand, ResultadosValidacion>
{
    public async Task<ResultadosValidacion> Handle(EliminarGastoCommand request, CancellationToken cancellationToken)
    {
        var validacion = new ResultadosValidacion();

        try
        {
            //Verificar si el gasto existe
            var gastosExiste = await repositorioGasto.ObtenerPorIdAsync(request.IdCommand);
            if (gastosExiste == null)
                validacion.Errores.Add("ExcepcionCapaAplicacion", "El gasto no existe");
            //Verificar si la tarjeta de credito existe
            var tarjetaCreditoExiste = await repositorioTarjetaCredito.ObtenerPorIdAsync(gastosExiste!.TarjetaId.idTarjeta);
            if (tarjetaCreditoExiste == null)
                validacion.Errores.Add("ExcepcionCapaAplicacion", "La tarjeta de credito no existe");
            //Sumar el monto eliminado al crédito disponible
            tarjetaCreditoExiste!.RevertirGasto(gastosExiste.Monto.Valor);
            //Actualización de la tarjeta
            await repositorioGasto.EliminarPorIdAsync(gastosExiste.Id);
            await repositorioTarjetaCredito.ActualizarAsync(tarjetaCreditoExiste);
        }
        catch (ExcepcionDominio ex)
        {
            // Convertir errores de dominio a errores de aplicación
            validacion.Errores.Add(ex.Campo, ex.Message);
        }

        return validacion;
    }
}
