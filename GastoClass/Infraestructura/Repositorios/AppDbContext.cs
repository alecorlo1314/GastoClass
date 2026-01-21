using SQLite;
using GastoClass.Dominio.Model;

namespace GastoClass.Infraestructura.Repositorios
{
    /// <summary>
    /// Proporciona acceso a los datos de gastos
    /// </summary>
    public class AppDbContext
    {
        //Instancias la conexion a la base de datos SQLite
        SQLiteAsyncConnection? conexionBaseDatos;

        //Inicialización
        public async Task<SQLiteAsyncConnection> ObtenerConexion()
        {
            //Si la conexion ya fue creada, no hacer nada
            if (conexionBaseDatos is not null) return conexionBaseDatos;
            try
            {
                //Si no fue creada, establecer la conexion pasando la ruta y las banderas
                conexionBaseDatos = new SQLiteAsyncConnection(Constantes.RutaBaseDatos, Constantes.Flags);
                //Creamos una tabla para almacenar los gastos
                await conexionBaseDatos.CreateTableAsync<Gasto>();
                await conexionBaseDatos.CreateTableAsync<TarjetaCredito>();
                await conexionBaseDatos.CreateTableAsync<PreferenciaTarjeta>();

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
}
