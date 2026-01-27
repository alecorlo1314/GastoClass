using GastoClass.Aplicacion.Common;
using GastoClass.Aplicacion.Gastos.Commands;
using GastoClass.Dominio.Entidades;
using GastoClass.Dominio.Interfaces;
using GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;
using MediatR;

namespace GastoClass.Aplicacion.Gastos.Handlers
{
    public class CrearTarjetaCreditoCommandHandler(IRepositorioTarjetaCredito repositorioTarjetaCredito) :
        IRequestHandler<CrearTarjetaCreditoCommand, ResultadosValidacion>
    {
        #region Inyeccion de Dependencias
        /// <summary>
        /// Utiliza los servicios de la interfaz IGastoRepositorio
        /// </summary>
        private readonly IRepositorioTarjetaCredito _repositorioTarjetaCredito = repositorioTarjetaCredito;

        #endregion

        #region Agregar Gasto Handle

        public async Task<ResultadosValidacion> Handle(CrearTarjetaCreditoCommand request, CancellationToken cancellationToken)
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
                            new Moneda(request.Moneda!),
                            new DiaCorte(request.DiaCorte!.Value),
                            new DiaPago(request.DiaPago!.Value),
                            new NombreBanco(request.NombreBanco!)
                        );
                // Devolvera un conjunto de errores si la tarjeta de credito es invalida
                await repositorioTarjetaCredito.AgregarAsync(tarjeta);
            //retorna los resultados de las validaciones que se capturaron
            //usando ExcepcionBehavior
                return new ResultadosValidacion();
        }

        #endregion
    }
}
