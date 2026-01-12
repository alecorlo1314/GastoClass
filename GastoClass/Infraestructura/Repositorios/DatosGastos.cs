using GastoClass.Dominio.Interfacez;
using GastoClass.Dominio.Model;

namespace GastoClass.Infraestructura.Repositorios
{
    public class DatosGastos : IServicioGastos
    {
        //Inyeccion de dependecias
        private readonly RepositorioBaseDatos _repositorioBaseDatos;
        public DatosGastos(RepositorioBaseDatos repositorioBaseDatos)
        {
            _repositorioBaseDatos = repositorioBaseDatos;
        }

        /// <summary>
        /// Metodo para obtener los gastos totales del mes
        /// </summary>
        /// <param name="mes"></param>
        /// <param name="anio"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<decimal> ObtenerGastosTotalesDelMesAsync(int mes, int anio)
        {
            try
            {
                //obtener la conexion a la base de datos
                var conexion = await _repositorioBaseDatos.ObtenerConexion();

                //var total = await conexion.ExecuteScalarAsync<float>("SELECT SUM(Monto) FROM Gasto");

                //Consulta para obtener los gastos del mes y año especificados
                var consulta = conexion.Table<Gasto>()
                    .Where(g => g.Fecha.Month == mes && g.Fecha.Year == anio);
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
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Error al obtener los gastos totales del mes.", ex);
            }
        }
        /// <summary>
        /// Metodo para obtener la cantidad de transacciones del mes
        /// </summary>
        /// <param name="mes"></param>
        /// <param name="anio"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<List<int>> ObtenerTransaccionesDelMesAsync(int mes, int anio)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Metodo para obtener las categorias con mayor gasto del mes
        /// </summary>
        /// <param name="mes"></param>
        /// <param name="anio"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<List<string>> ObtenerCategoriasMayorGastoDelMess(int mes, int anio)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Metodo para obtener los ultimos 5 gastos realizados
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<List<Gasto>> ObtenerUltimos5GastosAsync()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Metodo para guardar un nuevo gasto
        /// </summary>
        /// <param name="nuevoGasto"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> GuardarGastoAsync(Gasto nuevoGasto)
        {
            try
            {
                //obtener la conexion a la base de datos
                var conexion = await _repositorioBaseDatos.ObtenerConexion();
                if(nuevoGasto.Id != 0)
                {
                    return await conexion.UpdateAsync(nuevoGasto);
                }
                else
                {
                    return await conexion.InsertAsync(nuevoGasto);
                }

            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Error al guardar el gasto.", ex);
            }
        }
    }
}
