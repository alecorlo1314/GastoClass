using GastoClass.Aplicacion.Common;
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
            {
                validacion.Errores.Add("ExcepcionCapaAplicacion", "El gasto no existe");
                return validacion;
            }

            //Verificar si la tarjeta de credito existe
            var tarjetaCreditoExiste = await repositorioTarjetaCredito.ObtenerPorIdAsync(gastosExiste!.TarjetaId.IdTarjeta);
            if(tarjetaCreditoExiste == null)
            {
                //Como no hay tarjeta , solamente eliminamos el gastos
                await repositorioGasto.EliminarPorIdAsync(gastosExiste.Id);
                validacion.Errores.Add("ExcepcionCapaAplicacion", "No existe gasto con la tarjeta.");
                return validacion;
            }
                
            await repositorioGasto.EliminarPorIdAsync(gastosExiste.Id);

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
