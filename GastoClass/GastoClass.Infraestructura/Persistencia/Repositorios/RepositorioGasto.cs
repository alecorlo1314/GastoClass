using GastoClass.Dominio.Entidades;
using GastoClass.GastoClass.Dominio.Interfaces;
using Infraestructura.Mapper;
using Infraestructura.Persistencia.ContextoDB;
using Infraestructura.Persistencia.Entidades;

namespace Infraestructura.Persistencia.Repositorios;

public class RepositorioGasto(AppContextoDatos _conexion) : IRepositorioGasto
{
    public async Task ActualizarAsync(GastoDominio gasto)
    {
        var conexion = await _conexion.ObtenerConexionAsync();
        var entity = GastoMapper.ToEntidad(gasto);
        entity.Id = gasto.Id;
        await conexion.UpdateAsync(entity);
    }

    public async Task AgregarAsync(GastoDominio gasto)
    {
        var conexion = await _conexion.ObtenerConexionAsync();
        var entidadGasto = GastoMapper.ToEntidad(gasto);

        await conexion.InsertAsync(entidadGasto);
        gasto.SetId(entidadGasto.Id);
    }


    public async Task<int> EliminarPorIdAsync(int id)
    {
        var conexion = await _conexion.ObtenerConexionAsync();
        return await conexion.DeleteAsync<GastoEntidad>(id);
    }

    public async Task<GastoDominio?> ObtenerPorIdAsync(int id)
    {
        var conexion = await _conexion.ObtenerConexionAsync();
        var entity = await conexion.FindAsync<GastoEntidad>(id);
        return entity == null ? null : GastoMapper.ToDomain(entity);
    }

    public async Task<List<GastoDominio>?> ObtenerPorTarjetaAsync(int tarjetaId)
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

    public async Task<List<GastoDominio>?> ObtenerTodosAsync()
    {
        //obtener la conexion a la base de datos
        var conexion = await _conexion.ObtenerConexionAsync();
        //Obtener todos los gastos
        var entidades = await conexion
            .Table<GastoEntidad>()
            .ToListAsync();
        //Mapear a dominio
        return entidades == null ? null : 
               entidades
               .Select(g => GastoMapper.ToDomain(g))
               .ToList();
    }

    public async Task<decimal> TotalMesAsync(int mes, int anio)
    {
        var conexion = await _conexion.ObtenerConexionAsync();
        var inicioMes = new DateTime(anio, mes, 1);
        var finMes = inicioMes.AddMonths(1);

        var gastosDelMes = await conexion.Table<GastoEntidad>()
            .Where(g => g.Fecha >= inicioMes && g.Fecha < finMes)
            .ToListAsync();

        return gastosDelMes.Sum(g => g.Monto);
    }
    public async Task<int> CantidadMesAsync(int mes, int anio)
    {
        //obtener la conexion a la base de datos
        var conexion = await _conexion.ObtenerConexionAsync();
        //Convertimos el mes y año en un rango de fechas
        var inicioMes = new DateTime(anio, mes, 1);
        var finMes = inicioMes.AddMonths(1);
        //Consulta para obtener los gastos del mes y año especificados
        var consulta = conexion.Table<GastoEntidad>()
            .Where(g => g.Fecha >= inicioMes && g.Fecha < finMes);
        //contamos la cantidad de transacciones
        return await consulta.CountAsync();
    }
}
