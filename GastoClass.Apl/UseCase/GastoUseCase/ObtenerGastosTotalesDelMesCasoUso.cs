using GastoClass.Dominio.Interfaces;

namespace GastoClass.Aplicacion.UseCase.GastoUseCase
{
    #region Obtener Gastos Totales Del Mes 
    public class ObtenerGastosTotalesDelMesCasoUso (IRepositorioGasto _repositorioGasto)
    {
        public async Task<decimal> Obtener(int mes, int anio)
        {
            var listaGastos = await _repositorioGasto.ObtenerTodosAsync();
            //Convertimos el mes y año en un rango de fechas
            var inicioMes = new DateTime(anio, mes, 1);
            var finMes = inicioMes.AddMonths(1);
            //Consulta para obtener los gastos del mes y año especificados
            var gastosMes = listaGastos!
                .Where(g => g.Fecha.Valor!.Value >= inicioMes && g.Fecha.Valor!.Value < finMes)
                .ToList();
            //inicializamos el total de gastos
            decimal totalGastos = 0;
            //iteramos sobre los gastos y sumamos los montos
            foreach (var gasto in gastosMes)
            {
                totalGastos += gasto.Monto.Valor;
            }
            //retornamos el total de gastos
            return totalGastos;
        }
    }

    #endregion
}
