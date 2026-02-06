using GastoClass.Dominio.Entidades;

namespace GastoClass.Dominio.Interfaces;

public interface IRepositorioGasto
{
    Task ActualizarAsync(GastoDominio gasto);
    Task AgregarAsync(GastoDominio gasto);
    Task<int> CantidadMesAsync(int mes, int ano);
    Task<int> EliminarPorIdAsync(int id);
    Task<GastoDominio?> ObtenerPorIdAsync(int id);
    Task<List<GastoDominio>?> ObtenerPorTarjetaAsync(int tarjetaId);
    Task<List<GastoDominio>?> ObtenerTodosAsync();
    Task<decimal> TotalMesAsync(int mes, int ano);
}
