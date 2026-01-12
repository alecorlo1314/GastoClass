using GastoClass.Dominio.Model;
using GastoClass.Dominio.Interfacez;

namespace GastoClass.Aplicacion.CasosUso
{
    //Este servicio se encargara de orquestar las operaciones relacionadas con el dashboard
    //Como peticiones directas a las bases de datos
    //Y conexion a otros servicios si es necesario
    //Servicios necesarios:
    // - Servicio para obtener gastos totales del mes
    // - Servicio para obtener cantidad de transacciones en este mes
    // - Servicio para obtener categoria con mayor gasto
    // - Servicio para obtener los ultimos 5 gastos
    public class ServicioGastos
    {
        #region Inyeccion de dependencias
        private readonly IServicioGastos _servicioGastos;
        #endregion
        public ServicioGastos(IServicioGastos servicioDashboard)
        {
            //Inyeccion de dependencias
            _servicioGastos = servicioDashboard;
        }
        //Metodo para obtener gastos totales del mes (parametros: mes y anio)
        public async Task<decimal> ObtenerGastosTotalesDelMesAsync(int mes, int anio)
        {
            ///retorna los gastos totales del mes
            return await _servicioGastos.ObtenerGastosTotalesDelMesAsync(mes, anio);
        }
        //Metodo para obtener cantidad de transacciones en este mes (parametros: mes y anio)
        public async Task<List<int>> ObtenerCantidadGastosDelMesAsync(int mes, int anio)
        {
            ///retorna la cantidad de transacciones en este mes
            return await _servicioGastos.ObtenerTransaccionesDelMesAsync(mes, anio);
        }
        //Metodo para obtener categoria con mayor gasto (parametros: mes y anio)
        public async Task<List<string>> ObtenerCategoriaMayorGastoDelMesAsync(int mes, int anio)
        {
            ///retorna la categoria con mayor gasto
            return await _servicioGastos.ObtenerCategoriasMayorGastoDelMess(mes, anio);
        }
        //Metodo para obtener los ultimos 5 gastos
        public async Task<List<Gasto>> ObtenerUltimos5GastosAsync()
        {
            ///retorna los ultimos 5 gastos
            return await _servicioGastos.ObtenerUltimos5GastosAsync();
        }
        public async Task<int> GuardarGastoAsync(Gasto gasto)
        {
            ///Guarda un nuevo gasto
            return await _servicioGastos.GuardarGastoAsync(gasto);
        }
    }
}
