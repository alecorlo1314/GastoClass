using MediatR;

namespace GastoClass.Aplicacion.Dashboard.GastosPorCategoria;

public record ObtenerGastosPorCategoriaConsulta(int mes, int anio) 
    : IRequest<List<GastoPorCategoriaDto>?> { }
