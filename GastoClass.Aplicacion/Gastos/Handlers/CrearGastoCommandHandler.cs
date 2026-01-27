using GastoClass.Aplicacion.Common;
using GastoClass.Aplicacion.Excepciones;
using GastoClass.Aplicacion.Gastos.Commands;
using GastoClass.Dominio.Entidades;
using GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;
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
            //Intanciar el validador de errores
            var validador = new ResultadosValidacion();
            try
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
            }
            catch (ExcepcionTipoTarjetaInvalido ex)
            {
                validador.Errores["TipoTarjeta"] = ex.Message;
            }
            catch (ExceptionNombreTarjetaInvalida ex)
            {
                validador.Errores["NombreTarjeta"] = ex.Message;
            }
            catch (ExcepcionNumeroTarjetaInvalida ex)
            {
                validador.Errores["UltimosCuatroDigitos"] = ex.Message;
            }
            catch (ExcepcionLimiteCreditoInvalido ex)
            {
                validador.Errores["LimiteCredito"] = ex.Message;
            }
            catch (ExcepcionMonedaInvalida ex)
            {
                validador.Errores["Moneda"] = ex.Message;
            }
            catch (ExcepcionDiaCorteInvalido ex)
            {
                validador.Errores["DiaCorte"] = ex.Message;
            }
            catch (ExcepcionDiaPagoInvalido ex)
            {
                validador.Errores["DiaPago"] = ex.Message;
            }
            catch (ExcepcionBancoNullInvalido ex)
            {
                validador.Errores["Banco"] = ex.Message;
            }
            catch (ExcepcionBancoLongitudInvalida ex)
            {
                validador.Errores["Banco"] = ex.Message;
            }
            catch (ExcepcionDominio ex)
            {
                validador.Errores.Add("Informacion", ex.Message);
            }
            return validador;
        }

        #endregion
    }
}
