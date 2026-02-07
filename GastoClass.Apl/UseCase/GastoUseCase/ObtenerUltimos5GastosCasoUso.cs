using GastoClass.Aplicacion.Excepciones;
using GastoClass.Dominio.Entidades;
using GastoClass.Dominio.Interfaces;

namespace GastoClass.Aplicacion.UseCase.GastoUseCase;

public class ObtenerUltimos5GastosCasoUso(IRepositorioGasto repositorioGasto)
{
    public async Task<List<Gasto>> ObtenerUltimos5GastosAsync()
    {
        try
        {
            var listaGastos = await repositorioGasto.ObtenerTodosAsync();
            //Consulta para obtener los ultimos 5 gastos ordenados por fecha descendente
            var consulta = listaGastos?
                .OrderByDescending(g => g.Fecha)
                .Take(5)
                .ToList();

            return consulta!;
        }
        catch (Exception ex)
        {
            // Manejo de errores
            throw new ExcepcionDominio("Error al obtener los ultimos 5 gastos de la base de datos" + ex.Message);
        }
    }
}
