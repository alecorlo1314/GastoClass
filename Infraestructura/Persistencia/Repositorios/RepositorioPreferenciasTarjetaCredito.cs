using GastoClass.Dominio.Entidades;
using GastoClass.Dominio.Interfaces;
using Infraestructura.Mapper;
using Infraestructura.Persistencia.ContextoDB;
using Infraestructura.Persistencia.Entidades;

namespace Infraestructura.Persistencia.Repositorios;

public class RepositorioPreferenciasTarjetaCredito(AppContextoDatos contextoDatos)
    : IRepositorioPreferenciaTarjeta
{
    public async Task AgregarAsync(PreferenciaTarjeta preferenciaTarjeta)
    {
        //Establecer Conexion
        var conexion = await contextoDatos.ObtenerConexionAsync();
        //Mapear a entidadPreferenciaTarjeta
        var gastoEntidad = PrefereciasTarjetaMapper.ToEntidad(preferenciaTarjeta);
        await conexion.InsertAsync(gastoEntidad);
    }

    public async Task EliminarAsync(PreferenciaTarjeta preferencia)
    {
        //Establecer conexion 
        var conexion = await contextoDatos.ObtenerConexionAsync();
        //Mapear toEntidad
        var preferenciaEnidad = PrefereciasTarjetaMapper.ToEntidad(preferencia);
        await conexion.DeleteAsync(preferenciaEnidad);
    }

    public async Task<PreferenciaTarjeta?> ObtenerPorIdTarjeta(int idPreferencia)
    {
        //Establecer conexion 
        var conexion = await contextoDatos.ObtenerConexionAsync();
        //obtener proferencias por id
        var preferencia = await conexion.FindAsync<PreferenciasTarjetaEntidad>(idPreferencia);
        //Mapear el resultado a dominio y retornar
        return PrefereciasTarjetaMapper.ToDomain(preferencia);
    }
}
