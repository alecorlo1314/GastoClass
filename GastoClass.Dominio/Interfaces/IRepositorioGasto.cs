using GastoClass.Dominio.Entidades;

namespace GastoClass.Dominio.Interfaces
{
    public interface IRepositorioGasto
    {
        Task<List<Gasto>?> ObtenerPorTarjetaAsync(int tarjetaId);
        Task AgregarAsync(Gasto gasto);
        Task ActualizarAsync(Gasto gasto);
        Task EliminarAsync(int id);
        Task<Gasto?> ObtenerPorIdAsync(int id);
        Task<List<Gasto>?> ObtenerTodosAsync();
    }
}
