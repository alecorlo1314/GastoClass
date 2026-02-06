using GastoClass.Infraestructura.Persistencia.Entidades;
using SQLite;

namespace GastoClass.Infraestructura.Persistencia.ContextoDB;

public class AppContextoDatos
{
    //Instancias la conexion a la base de datos SQLite
    SQLiteAsyncConnection? conexionBaseDatos;

    //Inicialización
    public async Task<SQLiteAsyncConnection> ObtenerConexionAsync()
    {
        //Si la conexion ya fue creada, no hacer nada
        if (conexionBaseDatos is not null) return conexionBaseDatos;
        try
        {
            //Si no fue creada, establecer la conexion pasando la ruta y las banderas
            conexionBaseDatos = new SQLiteAsyncConnection(Constantes.RutaBaseDatos, Constantes.Flags);
            //Creamos una tabla para almacenar los gastos
            await conexionBaseDatos.CreateTableAsync<GastoEntidad>();
            await conexionBaseDatos.CreateTableAsync<TarjetaCreditoEntidad>();
            await conexionBaseDatos.CreateTableAsync<PreferenciasTarjetaEntidad>();

            //Retornamos la conexion a la base de datos
            return conexionBaseDatos;
        }
        catch (Exception ex)
        {
            //En caso de error, lanzar una excepción
            throw new Exception("No se pudo crear la conexión a la base de datos", ex);
        }
    }
}
