using GastoClass.Aplicacion.DTOs;
using GastoClass.Dominio.Interfaces;

namespace GastoClass.Aplicacion.UseCase.GastoUseCase;

public class ObtenerGastoTotalPorCategoriaMesCasoUso(IRepositorioGasto repositorioGasto)
{
    #region Obtener Gastos Totales Por Categoria Del Mes
    public async Task<List<ObtenerGastoTotalPorCategoriaMesDto>> ObtenerGastoTotalPorCategoriaMesAsync(int mes, int anio)
    {
        try
        {
            var listaGastos = await repositorioGasto.ObtenerTodosAsync();
            //Convertimos el mes y año en un rango de fechas
            var inicioMes = new DateTime(anio, mes, 1);
            var finMes = inicioMes.AddMonths(1);
            //Consulta para obtener los gastos del mes y ano por categoria especificados
            var totalGastos = listaGastos?
                .Where(g => g.Fecha.Valor!.Value >= inicioMes && g.Fecha.Valor.Value < finMes)
                .GroupBy(g => g.Categoria)
                .Select(g => new ObtenerGastoTotalPorCategoriaMesDto
                {
                    //Se pasa la categoria
                    Categoria = g.Key.Valor!,
                    //Se suma el monto por categoria
                    Total = g.Sum(x => x.Monto.Valor)
                    //Se ordena de mayor a menor
                }).OrderByDescending(g => g.Total);
            //Convertimos la consulta en lista y retornamos
            return totalGastos!.ToList();
        }
        catch (Exception ex)
        {
            // Manejo de errores
            throw new Exception("Error al obtener los gastos totales por categoria del mes en la base de datos", ex);
        }
    }

    #endregion
}
