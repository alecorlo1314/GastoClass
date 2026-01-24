using GastoClass.Aplicacion.DTOs;
using GastoClass.Dominio.Interfacez;
using GastoClass.Dominio.Model;
using GastoClass.Infraestructura.Excepciones;
using Syncfusion.Maui.DataSource.Extensions;

namespace GastoClass.Infraestructura.Repositorios
{
    public class DatosGastos : IServicioGastos
    {
        //Inyeccion de dependecias
        private readonly AppDbContext _conexion;
        public DatosGastos(AppDbContext repositorioBaseDatos)
        {
            _conexion = repositorioBaseDatos;
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
                var conexion = await _conexion.ObtenerConexion();
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
                var conexion = await _conexion.ObtenerConexion();
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
                var conexion = await _conexion.ObtenerConexion();
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
                var conexion = await _conexion.ObtenerConexion();
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
        public async Task<int> GuardarGastoAsync(Gasto gasto)
        {
            try
            {
                //obtener la conexion a la base de datos
                var conexion = await _conexion.ObtenerConexion();
                //validar que la tarjeta exista
                var tarjeta = await conexion.FindAsync<TarjetaCredito>(gasto.TarjetaId);
                //Actualizar gasto si el id es diferente de 0
                if (gasto.Id != 0)
                {
                    //Validar que el gasto exista
                    var gastoExistente = await conexion.FindAsync<Gasto>(gasto.Id);
                    if(gastoExistente == null) throw new Exception("El gasto no existe.");

                    //Validar que el nuevo monto/categoría sean correctos.
                    if(gasto.Monto < 0 || gasto.Categoria == null) throw new Exception("El monto y la categoría deben ser correctos.");
                    //antes de guardar el gasto, guardaremos el monto anterior
                    decimal montoAnterior = gastoExistente.Monto;
                    //Ajustar el balance actual si el monto actual es diferente al monto anterior
                    if(gasto.Monto < montoAnterior)
                    {
                        //restamos la diferencia entre el monto anterior y el monto actual
                        tarjeta.Balance -= montoAnterior - gasto.Monto;
                    }else if(montoAnterior < gasto.Monto)
                    {
                        //sumamos la diferencia entre el monto actual y el monto anterior
                        tarjeta.Balance += gasto.Monto - montoAnterior;
                    }
                    // Validar límite
                    if (tarjeta.Balance > tarjeta.LimiteCredito) 
                        throw new RespositorioGastoExcepcion("El balance actual de la tarjeta es mayor al limite de credito.");
                    tarjeta.CreditoDisponible = tarjeta.LimiteCredito - tarjeta.Balance;
                    //Actualizar tarjeta y gasto
                    await conexion.UpdateAsync(gasto);
                    return await conexion.UpdateAsync(tarjeta);
                }
                if (tarjeta == null)
                    throw new Exception("La tarjeta no existe.");
                //Validar que el monto del gasto sea positivo y no exceda el crédito disponible.
                if(gasto.Monto < 0 || gasto.Monto > tarjeta.CreditoDisponible) 
                    throw new Exception($"El monto {gasto.Monto} del gasto no puede ser negativo o exceder el credito disponible.");
                //Insertar gasto
                await conexion.InsertAsync(gasto);
                //Incrementar el balance actual de la tarjeta.
                tarjeta.Balance += gasto.Monto;
                //Restar el monto del gasto al crédito disponible.
                tarjeta.CreditoDisponible = tarjeta.LimiteCredito - tarjeta.Balance;
                //Actualizar tarjeta de credito
                var resultado = await conexion.UpdateAsync(tarjeta);
                    return resultado;
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
        public async Task<List<Gasto>?> ObtenerGastosAsync()
        {
            try
            {
                //obtener la conexion a la base de datos
                var conexion = await _conexion.ObtenerConexion();
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
        /// <summary>
        /// Metodo para eliminar un gasto
        /// </summary>
        /// <param name="eliminarGasto"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> EliminarGastoAsync(Gasto eliminarGasto)
        {
            try
            {
                //obtener la conexion a la base de datos
                var conexion = await _conexion.ObtenerConexion();
                //Verificar que el gasto exista
                var gasto = await conexion.FindAsync<Gasto>(eliminarGasto.Id);
                var tarjeta = await conexion.FindAsync<TarjetaCredito>(gasto.TarjetaId);
                if(gasto == null) throw new Exception("El gasto no existe.");
                if (tarjeta == null)
                {
                    //Como no hay tarjeta , solamente eliminamos el gastos
                    await conexion.DeleteAsync(gasto);
                    throw new Exception("No existe gasto con la tarjeta.");
                }
                //Eliminar gastos
                await conexion.DeleteAsync(eliminarGasto);
                //Sumar el monto eliminado al crédito disponible
                tarjeta.CreditoDisponible += eliminarGasto.Monto;
                //Restar el monto del gasto al balance actual.
                tarjeta.Balance -= eliminarGasto.Monto;
                //Actualización de la tarjeta
                var resultado = await conexion.UpdateAsync(tarjeta);
                return resultado;
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Error al eliminar el gastos la base de datos", ex);
            }
        }

        /// <summary>
        /// Metodo para obtener un gasto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="RespositorioGastoExcepcion"></exception>
        public async Task<Gasto?> GastosPorIdAsync(int? id)
        {
            try
            {
                //obtener la conexion a la base de datos
                var conexion = await _conexion.ObtenerConexion();
                var gasto = await conexion.FindAsync<Gasto>(id!);
                return gasto;
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new RespositorioGastoExcepcion("Error al obtener todas las tarjetas de credito de la base de datos", ex);
            }
        }
    }
}
