using GastoClass.Aplicacion.Dashboard.DTOs;

namespace GastoClass.Aplicacion.Interfaces;

public interface IPrediccionCategoriaServicio
{
    Task<PrediccionCategoriaDto> PredecirAsync(string descripcion);
}
