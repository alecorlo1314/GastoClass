using GastoClass.Aplicacion.DTOs;
using GastoClass.Dominio.Model;

namespace GastoClass.Dominio.Interfacez;

public interface IServicioTarjetaCredito
{
    Task<int> AgregarTarjetaCreditoAsync(TarjetaCredito tarjetaCredito);
    Task<List<TarjetaCredito>?> ObtenerTarjetasCreditoAsync();
    Task<int> EliminarTarjetasCreditoAsync();
    Task<List<TotalGastoPorTarjeta>?> ObtenerGastosPorTarjetasCreditoAsync();
    Task<List<Gasto>?> ObtenerUltimosTresGastosPorTarjetaCreditoAsync(int? idTarjetaCredito);
}
