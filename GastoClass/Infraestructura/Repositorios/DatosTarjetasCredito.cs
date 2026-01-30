using GastoClass.Aplicacion.DTOs;
using GastoClass.Dominio.Interfacez;
using GastoClass.Dominio.Model;

namespace GastoClass.Infraestructura.Repositorios;
public class DatosTarjetasCredito : IServicioTarjetaCredito
{
    #region Inyeccion de dependencias
    /// <summary>
    /// Conexion a la base de datos
    /// </summary>
    private readonly AppDbContext _conexionBaseDatos;
    #endregion

    #region Contructor
    public DatosTarjetasCredito(AppDbContext repositorioBaseDatos)
    {
        //Inyectamos la dependencia
        _conexionBaseDatos = repositorioBaseDatos;
    }
    #endregion

    /// <summary>
    /// Metodo para agregar una nueva tarjeta de credito
    /// </summary>
    /// <param name="tarjetaCredito"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<int> AgregarTarjetaCreditoAsync(TarjetaCredito tarjetaCredito)
    {
        try
        {
            var conexion = await _conexionBaseDatos.ObtenerConexion();

            // Guardar o actualizar la preferencia si existe
            if (tarjetaCredito.PreferenciaTarjeta != null)
            {
                if (tarjetaCredito.PreferenciaTarjeta.Id > 0)
                {
                    await conexion.UpdateAsync(tarjetaCredito.PreferenciaTarjeta);
                }
                else
                {
                    await conexion.InsertAsync(tarjetaCredito.PreferenciaTarjeta);
                }

                // Asegurar que la tarjeta tenga el Id de la preferencia
                tarjetaCredito.IdPreferenciaTarjeta = tarjetaCredito.PreferenciaTarjeta.Id;
            }

            // Guardar o actualizar la tarjeta
            if (tarjetaCredito.Id > 0)
            {
                return await conexion.UpdateAsync(tarjetaCredito);
            }
            else
            {
                return await conexion.InsertAsync(tarjetaCredito);
            }
        }
        catch (Exception ex)
        {
            // Manejo de errores más claro
            throw new InvalidOperationException("Error al agregar la tarjeta de crédito.", ex);
        }
    }

    /// <summary>
    /// Metodo para obtener todas las tarjetas de credito
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<List<TarjetaCredito>?> ObtenerTarjetasCreditoAsync()
    {
        try
        {
            //obtener la conexion a la base de datos
            var conexion = await _conexionBaseDatos.ObtenerConexion();
            //Consulta para obtener todos los gastos
            var listaTarjetaCredito = await conexion.Table<TarjetaCredito>().ToListAsync();

            foreach(var tarjeta  in listaTarjetaCredito)
            {
                if (tarjeta.IdPreferenciaTarjeta.HasValue)
                {
                    //Guardamos la preferencia de la tarjeta
                    tarjeta.PreferenciaTarjeta = await conexion.FindAsync<PreferenciaTarjeta>(tarjeta.IdPreferenciaTarjeta.Value);
                }
            }

            return listaTarjetaCredito;
        }
        catch (Exception ex)
        {
            // Manejo de errores
            throw new Exception("Error al obtener todas las tarjetas de credito de la base de datos", ex);
        }
    }

    /// <summary>
    /// Metodo para eliminar todas las tarjetas de credito
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<int> EliminarTarjetasCreditoAsync()
    {
        try
        {
            var conexion = await _conexionBaseDatos.ObtenerConexion();
            var resultado = await conexion.DeleteAllAsync<TarjetaCredito>();
            if(resultado > 0)
            {
                return await conexion.DeleteAllAsync<PreferenciaTarjeta>();
            }
            return 0;
        }
        catch (Exception ex)
        {
            // Manejo de errores
            throw new Exception("Error al eliminar todas las tarjetas de credito de la base de datos", ex);
        }
    }

    /// <summary>
    /// Metodo para obtener todas las tarjetas de credito y mostrarlas en un grafico de donut
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<List<TotalGastoPorTarjeta>?> ObtenerGastosPorTarjetasCreditoAsync()
    {
        try
        {
            //obtener la conexion a la base de datos
            var conexion = await _conexionBaseDatos.ObtenerConexion();
            // Obtener todas las tarjetas
            var tarjetas = await conexion.Table<TarjetaCredito>().ToListAsync();
            var gastos = await conexion.Table<Gasto>().ToListAsync();

            //Agruparlos con join
            var resultado = (from g in gastos
                             join t in tarjetas on g.TarjetaId equals t.Id
                             group g by t.NombreTarjeta into grupo
                             select new TotalGastoPorTarjeta
                             {
                                 NombreTarjeta = grupo.Key,
                                 BalanceTotal = grupo.Sum(x => x.Monto)
                             }).ToList();

            return resultado;
        }
        catch (Exception ex)
        {
            // Manejo de errores
            throw new Exception("Error al obtener todas las tarjetas de credito de la base de datos", ex);
        }
    }
    public async Task<TarjetaCredito>? TarjetaPorIdAsync(int? idTarjetaCredito)
    {
        var conexion = await _conexionBaseDatos.ObtenerConexion();
        return await conexion.FindAsync<TarjetaCredito>(idTarjetaCredito!);
    }
}
