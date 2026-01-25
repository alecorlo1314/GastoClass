using GastoClass.Dominio.Entidades;

namespace GastoClass.Dominio.Interfaces;

public interface IRepositorioTarjetaCredito
{
    Task<List<TarjetaCredito>?> ObtenerTodosAsync();
    Task AgregarAsync(TarjetaCredito card);
    Task EliminarAsync(Guid cardId);
}
