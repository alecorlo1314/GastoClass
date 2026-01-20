using GastoClass.Aplicacion.DTOs;
using GastoClass.Dominio.Interfacez;
using GastoClass.Dominio.Model;

namespace GastoClass.Aplicacion.CasosUso;
public class ServicioTarjetaCredito
{
    #region Inyeccion de dependencias
    private readonly IServicioTarjetaCredito _servicioTarjetaCredito;
    #endregion

    #region Constructor
    public ServicioTarjetaCredito(IServicioTarjetaCredito servicioTarjetaCredito)
    {
        //Inyeccion de dependencias
        _servicioTarjetaCredito = servicioTarjetaCredito;
    }
    #endregion

    #region Metodos
    /// <summary>
    /// Guarda una nueva tarjeta de credito
    /// </summary>
    /// <param name="tarjetaCredito"></param>
    /// <returns></returns>
    public async Task<int> AgregarTarjetaCreditoAsync(TarjetaCredito tarjetaCredito)
    {
        //Guarda una nueva tarjeta de credito
        return await _servicioTarjetaCredito.AgregarTarjetaCreditoAsync(tarjetaCredito);
    }

    public async Task<List<TarjetaCredito>?> ObtenerTarjetasCreditoAsync() 
    {
       return await _servicioTarjetaCredito.ObtenerTarjetasCreditoAsync();
    } 

    public async Task<int> EliminarTarjetasCreditoAsync()
    {
        return await _servicioTarjetaCredito.EliminarTarjetasCreditoAsync();
    }

    public async Task<List<TotalGastoPorTarjeta>?> ObtenerGastosPorTarjetasCreditoAsync()
    {
        return await _servicioTarjetaCredito.ObtenerGastosPorTarjetasCreditoAsync();
    }
    #endregion
}
