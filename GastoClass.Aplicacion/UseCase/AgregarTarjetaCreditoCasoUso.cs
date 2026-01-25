using GastoClass.Aplicacion.Common;
using GastoClass.Aplicacion.DTOs;
using GastoClass.Aplicacion.Excepciones;
using GastoClass.Dominio.Entidades;
using GastoClass.Dominio.Enums;
using GastoClass.Dominio.Excepciones;
using GastoClass.Dominio.Interfaces;
using GastoClass.Dominio.ValueObjects;

namespace GastoClass.Aplicacion.CasosUso;

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
                        Guid.NewGuid(),
                        TipoTarjeta.Visa,
                        new NombreTarjeta(tarjetaCreditoDto.Nombre!),
                        new UltimosCuatroDigitosTarjeta(tarjetaCreditoDto.UltimosCuatroDigitos!),
                        new FechaVencimiento(tarjetaCreditoDto.MesVencimiento, tarjetaCreditoDto.AnioVencimiento)
                    );

            //Se envial al repositorio
            //devuelve 1 si se agrego correctamente, 0 si no
            await _repositorioTarjetaCredito.AgregarAsync(tarjeta);
        }
        catch (ExceptionNombreTarjetaInvalida ex)
        {
            validador.Errores["NombreTarjeta"] = ex.Message;
        }
        catch (ExcepcionNumeroTarjetaInvalida ex)
        {
            validador.Errores["UltimosCuatroDigitos"] = ex.Message;
        }
        catch (ExcepcionVencimientoTarjeta ex)
        {
            validador.Errores["Vencimiento"] = ex.Message;
        }
        return validador;
    }

    #endregion
}
