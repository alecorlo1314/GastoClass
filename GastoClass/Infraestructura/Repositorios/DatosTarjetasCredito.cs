using GastoClass.Dominio.Interfacez;
using GastoClass.Dominio.Model;

namespace GastoClass.Infraestructura.Repositorios;
public class DatosTarjetasCredito : IServicioTarjetaCredito
{
    #region Inyeccion de dependencias
    /// <summary>
    /// Conexion a la base de datos
    /// </summary>
    private readonly RepositorioBaseDatos _conexionBaseDatos;
    #endregion

    #region Contructor
    public DatosTarjetasCredito(RepositorioBaseDatos repositorioBaseDatos)
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
            //obtener la conexion a la base de datos
            var conexion = await _conexionBaseDatos.ObtenerConexion();

            //Insertamos la preferencia de la tarjeta credito
            if (tarjetaCredito.PreferenciaTarjeta?.Id != 0)
            {
                //Si la preferencia ya fue creada, no hacer nada
                var preferencia = await conexion.UpdateAsync(tarjetaCredito.PreferenciaTarjeta);
            }
            else
            {
                //Si no fue creada, establecer la conexion pasando la ruta y las banderas
                var preferencia = await conexion.InsertAsync(tarjetaCredito.PreferenciaTarjeta);
            }

            //Insertamos la tarjeta de credito
            if (tarjetaCredito.Id != 0)
            {
                //Si la preferencia ya fue creada, no hacer nada
                return await conexion.UpdateAsync(tarjetaCredito);
            }
            else
            {
                //Si no fue creada, establecer la conexion pasando la ruta y las banderas
                return await conexion.InsertAsync(tarjetaCredito);
            }

        }
        catch (Exception ex)
        {
            // Manejo de errores
            throw new Exception("Error al agregar la tarjeta de credito.", ex);
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
}
