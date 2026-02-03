using GastoClass.Dominio.Entidades;
using GastoClass.Dominio.Interfaces;
using GastoClass.GastoClass.Infraestructura.Mapper;
using Infraestructura.Mapper;
using Infraestructura.Persistencia.ContextoDB;
using Infraestructura.Persistencia.Entidades;

namespace Infraestructura.Persistencia.Repositorios;

public class RepositorioTarjetaCredito : IRepositorioTarjetaCredito
{
    #region Inyeccion de Dependencias
    /// <summary>
    /// Inyeccion de Dependencias para usar los servicios e inicializar el repositorio
    /// </summary>
    private readonly AppContextoDatos _conexion;

    #endregion

    #region Constructor
    public RepositorioTarjetaCredito(AppContextoDatos conexion)
    {
        _conexion = conexion;
    }

    #endregion

    public async Task ActualizarAsync(TarjetaCreditoDominio tarjetaCredito)
    {
        var conexion = await _conexion.ObtenerConexionAsync();
        if(tarjetaCredito.Id == 0) return;
        var preferencia = await conexion.Table<PreferenciasTarjetaEntidad>().FirstOrDefaultAsync(t => t.Id == tarjetaCredito.Id);
        //Mapear a entidad 
        var tarjetaEntidad = TarjetaCreditoMapper.ToEntidadConPreferencia(tarjetaCredito,preferencia.ToDominio());
        await conexion.UpdateAsync(tarjetaEntidad);
    }

    public async Task AgregarAsync(TarjetaCreditoDominio tarjetaCredito)
    {
        var conexion = await _conexion.ObtenerConexionAsync();

        var preferenciaEntidad = tarjetaCredito.Preferencia.ToEntidad();

        await conexion.InsertAsync(preferenciaEntidad);

        tarjetaCredito.Preferencia.SetId(preferenciaEntidad.Id);

        var tarjetaEntidad = TarjetaCreditoMapper.ToEntidad(tarjetaCredito);
        await conexion.InsertAsync(tarjetaEntidad);
    }

    public async Task EliminarPorIdAsync(int idTarjetaCredito)
    {
        var conexion = await _conexion.ObtenerConexionAsync();
        await conexion.Table<TarjetaCreditoEntidad>().DeleteAsync();
    }

    public async Task<int> EliminarTodasAsync()
    {
       var conexion = await _conexion.ObtenerConexionAsync();
        return await conexion.DeleteAllAsync<TarjetaCreditoEntidad>();
    }

    public async Task<TarjetaCreditoDominio?> ObtenerPorIdAsync(int id)
    {
        var conexion = await _conexion.ObtenerConexionAsync();
        var resultado = await conexion.Table<TarjetaCreditoEntidad>().
             Where(t => t.Id == id).FirstOrDefaultAsync();
        return TarjetaCreditoMapper.ToDominio(resultado);
    }

    public async Task<List<TarjetaCreditoDominio>?> ObtenerTodosAsync()
    {
        var conexion = await _conexion.ObtenerConexionAsync();
        var entidades = await conexion.Table<TarjetaCreditoEntidad>().ToListAsync();

        return entidades == null ? null :
               entidades
               .Select(t => TarjetaCreditoMapper.ToDominio(t))
               .ToList();
    }
}
