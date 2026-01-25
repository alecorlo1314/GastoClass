using GastoClass.Dominio.Entidades;

namespace GastoClass.Dominio.Interfaces;

public interface IRepositorioTarjetaCredito
{
    Task<List<TarjetaCredito>?> ObtenerTodosAsync();
    Task<int>? AgregarAsync(TarjetaCredito tarjetaCredito);
    Task<int>? EliminarAsync(Guid cardId);
    Task<int>? ActualizarAsync(TarjetaCredito tarjetaCredito); 
}
