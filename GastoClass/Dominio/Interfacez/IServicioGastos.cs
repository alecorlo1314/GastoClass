using GastoClass.Dominio.Model;

namespace GastoClass.Dominio.Interfacez
{
    /// <summary>
    /// Interfaz que define los servicios relacionados con los gastos
    /// </summary>
    //Servicios necesarios:
    // - Servicio para obtener gastos totales del mes
    // - Servicio para obtener cantidad de transacciones en este mes
    // - Servicio para obtener categoria con mayor gasto
    // - Servicio para obtener los ultimos 5 gastos
    public interface IServicioGastos
    {
        Task<decimal> ObtenerGastosTotalesDelMesAsync(int mes, int anio);
        Task<int> ObtenerTransaccionesDelMesAsync(int mes, int anio);
        Task<List<string>> ObtenerCategoriasMayorGastoDelMess(int mes, int anio);
        Task<List<Gasto>> ObtenerUltimos5GastosAsync();
        Task<int> GuardarGastoAsync(Gasto gasto);
    }
}
