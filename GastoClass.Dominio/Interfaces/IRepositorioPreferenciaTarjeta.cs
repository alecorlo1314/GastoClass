using GastoClass.Dominio.Entidades;

namespace GastoClass.Dominio.Interfaces;

public interface IRepositorioPreferenciaTarjeta
{
    Task AgregarAsync(PreferenciaTarjeta preferenciaTarjeta);
    Task<PreferenciaTarjeta?> ObtenerPorIdTarjeta(int idPreferencia);
    Task EliminarAsync(PreferenciaTarjeta preferencia);
}
