using GastoClass.Dominio.Entidades;

namespace GastoClass.Dominio.Interfaces;

public interface IRepositorioPreferenciaTarjeta
{
    Task AgregarAsync(PreferenciaTarjetaDominio preferenciaTarjeta);
    Task<PreferenciaTarjetaDominio?> ObtenerPorIdTarjeta(int idPreferencia);
    Task EliminarAsync(PreferenciaTarjetaDominio preferencia);
}
