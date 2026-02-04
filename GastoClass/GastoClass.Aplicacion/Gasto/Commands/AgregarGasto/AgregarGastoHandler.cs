using GastoClass.Aplicacion.Common;
using GastoClass.Dominio.Entidades;
using GastoClass.Dominio.Interfaces;
using GastoClass.GastoClass.Dominio.Excepciones;
using GastoClass.GastoClass.Dominio.Interfaces;
using GastoClass.Infraestructura.Excepciones;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Gasto.Commands.AgregarGasto;

public class AgregarGastoHandler(
    IRepositorioGasto repositorioGasto,
    IRepositorioTarjetaCredito repositorioTarjetaCredito)
    : IRequestHandler<AgregarGastoCommand, ResultadosValidacion>
{
    public async Task<ResultadosValidacion> Handle(
        AgregarGastoCommand request,
        CancellationToken cancellationToken)
    {
        var resultados = new ResultadosValidacion();

        try
        {
            // 1. Buscar la tarjeta PRIMERO y validar que existe
            var tarjeta = await repositorioTarjetaCredito
                .ObtenerPorIdAsync(request.TarjetaIdCommand!.Value);

            if (tarjeta == null)
            {
                resultados.Errores.Add("tarjetaId", "La tarjeta no existe.");
                return resultados;
            }

            // 2. Crear el objeto dominio del gasto
            var gastoDominio = GastoDominio.Crear(
                monto: request.MontoCommand!.Value,
                fecha: request.FechaCommand!.Value,
                tarjetaId: request.TarjetaIdCommand.Value,
                descripcion: request.DescripcionCommand,
                categoria: request.CategoriaCommand,
                comercio: request.ComercioCommand,
                estado: request.EstadoCommand,
                nombreImagen: request.NombreImagenCommand
            );

            // 3. Validar que el monto no exceda el crédito disponible
            if (gastoDominio.Monto.Valor > tarjeta.CreditoDisponible)
            {
                resultados.Errores.Add("monto",
                    $"El monto ₡{gastoDominio.Monto.Valor:N2} excede el crédito disponible de ₡{tarjeta.CreditoDisponible:N2}.");
                return resultados;
            }

            // 4. Insertar el gasto
            await repositorioGasto.AgregarAsync(gastoDominio);

            // 5. Actualizar balance y crédito disponible de la tarjeta
            tarjeta.AumentarBalance(gastoDominio.Monto.Valor);
            tarjeta.ActualizacionCreditoDisponible(
                tarjeta.LimiteCredito.Valor,
                tarjeta.Balance);

            // 6. Guardar cambios de la tarjeta
            await repositorioTarjetaCredito.ActualizarAsync(tarjeta);
        }
        catch (ExcepcionDominio ex)
        {
            if (ex is IExcepcionPopup)
            {
                resultados.Popup = new PopupError(
                    "Error de negocio",
                    ex.Message
                );
            }
            else
            {
                resultados.Errores.Add(ex.Campo, ex.Message);
            }
        }


        return resultados;
    }
}