using GastoClass.Dominio.Interfaces;

namespace GastoClass.Aplicacion.UseCase.GastoUseCase;

#region Obtener Transacciones Del Mes   
public class ObtenerTransaccionesDelMesCasoUso(IRepositorioGasto repositorioGasto)
{
    public async Task<int> Obtener(int mes, int anio)
    {
        try
        {
            var listaGastos = await repositorioGasto.ObtenerTodosAsync();
            //Convertimos el mes y año en un rango de fechas
            var inicioMes = new DateTime(anio, mes, 1);
            var finMes = inicioMes.AddMonths(1);
            //Consulta para obtener los gastos del mes y año especificados
            var totalTransacciones = listaGastos?
                .Where(g => g.Fecha.Valor!.Value >= inicioMes && g.Fecha.Valor!.Value < finMes)
                .Count();

            //retornamos la cantidad de transacciones
            return totalTransacciones!.Value;
        }
        catch (Exception ex)
        {
            // Manejo de errores
            throw new Exception("Error al obtener los gastos totales del mes.", ex);
        }
    }

    #endregion
}
