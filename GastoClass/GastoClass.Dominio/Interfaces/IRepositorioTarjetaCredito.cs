using GastoClass.Dominio.Entidades;

namespace GastoClass.Dominio.Interfaces;

public interface IRepositorioTarjetaCredito
{
    Task<List<TarjetaCreditoDominio>?> ObtenerTodosAsync();
    Task AgregarAsync(TarjetaCreditoDominio tarjetaCredito);
    Task ActualizarAsync(TarjetaCreditoDominio tarjetaCredito);
    Task EliminarPorIdAsync(int idTarjetaCredito);
    Task<TarjetaCreditoDominio?> ObtenerPorIdAsync(int id);
    Task<int> EliminarTodasAsync();
}
