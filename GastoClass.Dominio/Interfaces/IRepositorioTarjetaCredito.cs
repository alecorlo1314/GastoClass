using GastoClass.Dominio.Entidades;

namespace GastoClass.Dominio.Interfaces;

public interface IRepositorioTarjetaCredito
{
    Task<List<TarjetaCredito>?> ObtenerTodosAsync();
    Task AgregarAsync(TarjetaCredito tarjetaCredito);
    Task ActualizarAsync(TarjetaCredito tarjetaCredito);
    Task EliminarAsync(int idTarjetaCredito);
    Task<TarjetaCredito?> ObtenerPorIdAsync(int id);
}
