using GastoClass.Dominio.Entidades;
using GastoClass.Dominio.Interfaces;
using GastoClass.GastoClass.Dominio.ValueObjects.ValueObjectsGasto;
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

    public Task ActualizarAsync(TarjetaCreditoDominio tarjetaCredito)
    {
        throw new NotImplementedException();
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

    public async Task EliminarAsync(int idTarjetaCredito)
    {
        var conexion = await _conexion.ObtenerConexionAsync();
        await conexion.Table<TarjetaCreditoEntidad>().DeleteAsync();
    }

    public Task<TarjetaCreditoDominio?> ObtenerPorIdAsync(int id)
    {
        throw new NotImplementedException();
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
