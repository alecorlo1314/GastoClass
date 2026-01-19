using GastoClass.Dominio.Model;

namespace GastoClass.Dominio.Interfacez;

public interface IServicioTarjetaCredito
{
    Task<int> AgregarTarjetaCreditoAsync(TarjetaCredito tarjetaCredito);
    Task<List<TarjetaCredito>?> ObtenerTarjetasCreditoAsync();
}
