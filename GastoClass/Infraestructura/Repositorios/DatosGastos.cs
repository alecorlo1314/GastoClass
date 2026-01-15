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
                //Convertimos el mes y año en un rango de fechas
                var inicioMes = new DateTime(anio, mes, 1); 
                var finMes = inicioMes.AddMonths(1);
                //Consulta para obtener los gastos del mes y año especificados
                var consulta = conexion.Table<Gasto>()
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
        public async Task<int> ObtenerTransaccionesDelMesAsync(int mes, int anio)
        {
            try
            {
                //obtener la conexion a la base de datos
                var conexion = await _repositorioBaseDatos.ObtenerConexion();
                //Convertimos el mes y año en un rango de fechas
                var inicioMes = new DateTime(anio, mes, 1);
                var finMes = inicioMes.AddMonths(1);
                //Consulta para obtener los gastos del mes y año especificados
                var consulta = conexion.Table<Gasto>()
                    .Where(g => g.Fecha >= inicioMes && g.Fecha < finMes);
                //contamos la cantidad de transacciones
                var cantidadTransacciones = await consulta.CountAsync();

                //retornamos la cantidad de transacciones
                return cantidadTransacciones;
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Error al obtener los gastos totales del mes.", ex);
            }
        }
        /// <summary>
        /// Metodo para obtener las categorias con mayor gasto del mes
        /// </summary>
        /// <param name="mes"></param>
        /// <param name="anio"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<Gasto>> ObtenerGastoTotalPorCategoriaMesAsync(int mes, int anio)
        {
            try
            {
                //obtener la conexion a la base de datos
                var conexion = await _repositorioBaseDatos.ObtenerConexion();
                //Convertimos el mes y año en un rango de fechas
                var inicioMes = new DateTime(anio, mes, 1);
                var finMes = inicioMes.AddMonths(1);
                //Consulta para obtener los gastos del mes y ano por categoria especificados
                var consulta = (await conexion.Table<Gasto>().ToListAsync())
                    .Where(g => g.Fecha >= inicioMes && g.Fecha < finMes)
                    .GroupBy(g => g.Categoria)
                    .Select(g => new Gasto
                    {
                        //Se pasa la categoria
                        Categoria = g.Key,
                        //Se suma el monto por categoria
                        Monto = g.Sum(x => x.Monto)
                        //Se ordena de mayor a menor
                    }).OrderByDescending(g => g.Monto);
                //Convertimos la consulta en lista y retornamos
                return consulta.ToList();
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Error al obtener los gastos totales por categoria del mes en la base de datos", ex);
            }
        }
        /// <summary>
        /// Metodo para obtener los ultimos 5 gastos realizados
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<Gasto>> ObtenerUltimos5GastosAsync()
        {
            try
            {
                //obtener la conexion a la base de datos
                var conexion = await _repositorioBaseDatos.ObtenerConexion();
                //Consulta para obtener los ultimos 5 gastos ordenados por fecha descendente
                var consulta = await conexion.Table<Gasto>()
                    .OrderByDescending(g => g.Fecha)
                    .Take(5)
                    .ToListAsync();

                return consulta;
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Error al obtener los ultimos 5 gastos de la base de datos", ex);
            }
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
        /// <summary>
        /// Metodo para obtener todos los gastos
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Gasto>> ObtenerGastosAsync()
        {
            try
            {
                //obtener la conexion a la base de datos
                var conexion = await _repositorioBaseDatos.ObtenerConexion();
                //Consulta para obtener todos los gastos
                var consulta = await conexion.Table<Gasto>()
                    .OrderByDescending(g => g.Fecha)
                    .ToListAsync();
                return consulta;
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Error al obtener todos los gastos de la base de datos", ex);
            }
        }
    }
}
