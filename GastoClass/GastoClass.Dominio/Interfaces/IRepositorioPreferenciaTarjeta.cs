using GastoClass.Dominio.Entidades;
using Infraestructura.Persistencia.Entidades;

namespace GastoClass.Dominio.Interfaces;

public interface IRepositorioPreferenciaTarjeta
{
    Task AgregarAsync(PreferenciaTarjetaDominio preferenciaTarjeta);
    Task<PreferenciaTarjetaDominio?> ObtenerPorIdTarjeta(int idPreferencia);
    Task<List<PreferenciaTarjetaDominio>?> ObtenerTodosAsync();
    Task<int> EliminarAsync(PreferenciaTarjetaDominio preferencia);
    Task<int> EliminarTodasAsync();
}
