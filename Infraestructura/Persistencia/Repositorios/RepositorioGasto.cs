using GastoClass.Dominio.Entidades;
using GastoClass.Dominio.Interfaces;
using Infraestructura.Mapper;
using Infraestructura.Persistencia.ContextoDB;
using Infraestructura.Persistencia.Entidades;

namespace Infraestructura.Persistencia.Repositorios;

public class RepositorioGasto(AppContextoDatos _conexion) : IRepositorioGasto
{
    public async Task ActualizarAsync(Gasto gasto)
    {
        var conexion = await _conexion.ObtenerConexionAsync();
        var entity = GastoMapper.ToEntidad(gasto);
        entity.Id = gasto.Id;
        await conexion.UpdateAsync(entity);
    }

    public async Task AgregarAsync(Gasto gasto)
    {
        var conexion =  await _conexion.ObtenerConexionAsync();
        var entidadGasto = GastoMapper.ToEntidad(gasto);
        if(entidadGasto.Id == 0)
        {
            await conexion.UpdateAsync(entidadGasto);
            gasto.SetId(entidadGasto.Id);
        }
        else
        {
            await conexion.InsertAsync(entidadGasto);
            gasto.SetId(entidadGasto.Id);
        }
    }

    public async Task EliminarAsync(int id)
    {
        var conexion = await _conexion.ObtenerConexionAsync();
        await conexion.DeleteAsync<GastoEntidad>(id);
    }

    public async Task<Gasto?> ObtenerPorIdAsync(int id)
    {
        var conexion = await _conexion.ObtenerConexionAsync();
        var entity = await conexion.FindAsync<GastoEntidad>(id);
        return entity == null ? null : GastoMapper.ToDomain(entity);
    }

    public async Task<List<Gasto>?> ObtenerPorTarjetaAsync(int tarjetaId)
    {
        var conexion = await _conexion.ObtenerConexionAsync();
        var entidades = await conexion
            .Table<GastoEntidad>()
            .Where(x => x.TarjetaId == tarjetaId)
            .ToListAsync();

        return entidades == null ? null : 
               entidades
               .Select(g => GastoMapper.ToDomain(g))
               .ToList();
    }
}
