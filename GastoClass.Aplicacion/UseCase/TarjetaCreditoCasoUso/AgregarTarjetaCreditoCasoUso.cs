using GastoClass.Aplicacion.Common;
using GastoClass.Aplicacion.DTOs;
using GastoClass.Aplicacion.Excepciones;
using GastoClass.Dominio.Entidades;
using GastoClass.Dominio.Excepciones.ExcepcionesTarjetaCredito;
using GastoClass.Dominio.Interfaces;
using GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;

namespace GastoClass.Aplicacion.UseCase.TarjetaCreditoCasoUso;

/// <summary>
/// Caso de Uso para Agregar Tarjeta de Credito
/// </summary>
public class AgregarTarjetaCreditoCasoUso
{
    #region Inyeccion Dependencias
    /// <summary>
    /// Inyeccion de Dependencias hacia Repositorio de Tarjeta de Credito
    /// </summary>
    private readonly IRepositorioTarjetaCredito _repositorioTarjetaCredito;

    #endregion

    #region Contructor
    public AgregarTarjetaCreditoCasoUso(IRepositorioTarjetaCredito repositorioTarjetaCredito)
    {
        _repositorioTarjetaCredito = repositorioTarjetaCredito;
    }
    #endregion

    #region Agregar Tarjeta de Credito
    public async Task<ResultadosValidacion> Ejecutar(TarjetaCreditoDto tarjetaCreditoDto)
    {
        //Intanciar el validador de errores
        var validador = new ResultadosValidacion();
        try
        {
            //Se mapea el Dto tarjeta de credito
            var tarjeta = new TarjetaCredito(
                        tarjetaCreditoDto.Id!.Value,
                        new TipoTarjeta(tarjetaCreditoDto.TipoTarjeta!),
                        new NombreTarjeta(tarjetaCreditoDto.Nombre!),
                        new UltimosCuatroDigitosTarjeta(tarjetaCreditoDto.UltimosCuatroDigitos!.Value),
                        new MesVencimiento(tarjetaCreditoDto.MesVencimiento),
                        new AnioVencimiento(tarjetaCreditoDto.AnioVencimiento),
                        new LimiteCredito(tarjetaCreditoDto.LimiteCredito!.Value),
                        new Moneda(tarjetaCreditoDto.Moneda!),
                        new DiaCorte(tarjetaCreditoDto.DiaCorte!.Value),
                        new DiaPago(tarjetaCreditoDto.DiaPago!.Value),
                        new NombreBanco(tarjetaCreditoDto.NombreBanco!)
                    );
            // Devolvera un conjunto de errores si la tarjeta de credito es invalida
            await _repositorioTarjetaCredito.AgregarAsync(tarjeta);
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
        }catch (ExcepcionDiaPagoInvalido ex)
        {
            validador.Errores["DiaPago"] = ex.Message;
        }catch (ExcepcionBancoNullInvalido ex)
        {
            validador.Errores["Banco"] = ex.Message;
        }catch (ExcepcionBancoLongitudInvalida ex)
        {
            validador.Errores["Banco"] = ex.Message;
        }catch(ExcepcionDominio ex)
        {
            throw new Exception("Ocurrio un error al agregar la tarjeta de credito: " + ex.Message);
        }
        return validador;
    }

    #endregion
}
