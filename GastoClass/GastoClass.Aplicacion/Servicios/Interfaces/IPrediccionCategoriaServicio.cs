using GastoClass.GastoClass.Aplicacion.Servicios.DTOs;

namespace GastoClass.Aplicacion.Interfaces;

public interface IPrediccionCategoriaServicio
{
    Task<CategoriaPredichaDto> PredecirAsync(string descripcion);
}
