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
        //Establecer conexion
        var conexion = await _conexion.ObtenerConexionAsync();
        //Convertimos el mes y año en un rango de fechas
        var inicioMes = new DateTime(anio, mes, 1);
        var finMes = inicioMes.AddMonths(1);
        //Consulta para obtener los gastos del mes y año especificados
        var consulta = conexion.Table<GastoEntidad>()
            .Where(g => g.Fecha >= inicioMes && g.Fecha < finMes);
        //lo convertimos en lista asyncrona
        var gastosDelMes = await consulta.ToListAsync();
        //inicializamos el total de gastos
        decimal totalGastos = 0;
        //iteramos sobre los gastos y sumamos los montos
        foreach (var gasto in gastosDelMes)
        {
            totalGastos += gasto.Monto;
        }
        //retornamos el total de gastos
        return totalGastos;
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
        var cantidadTransacciones = await consulta.CountAsync();

        //retornamos la cantidad de transacciones
        return cantidadTransacciones;
    }
    public async Task<List<GastoDominio>?> GastoPorCategoriaMes(int mes, int anio)
    {
        //obtener la conexion a la base de datos
        var conexion = await _conexion.ObtenerConexionAsync();
        //Convertimos el mes y año en un rango de fechas
        var inicioMes = new DateTime(anio, mes, 1);
        var finMes = inicioMes.AddMonths(1);
        //Consulta para obtener los gastos del mes y ano por categoria especificados
        var consulta = (await conexion.Table<GastoEntidad>().ToListAsync())
            .Where(g => g.Fecha >= inicioMes && g.Fecha < finMes)
            .GroupBy(g => g.Categoria)
            .Select(g => new GastoEntidad
            {
                //Se pasa la categoria
                Categoria = g.Key,
                //Se suma el monto por categoria
                Monto = g.Sum(x => x.Monto)
                //Se ordena de mayor a menor
            }).OrderByDescending(g => g.Monto);
         List<GastoDominio> listaGasto = new();
        foreach (var gasto in consulta)
        {
            //Mapear a dominio
            listaGasto.Add(GastoMapper.ToDomain(gasto));
        }
        return listaGasto;

    }
}
