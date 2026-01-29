using MediatR;

namespace GastoClass.Aplicacion.Dashboard.Consultas.ObtenerGastosPorCategoria;

public record ObtenerGastosPorCategoriaConsulta(int mes, int anio) 
    : IRequest<List<GastoPorCategoriaDto>?> { }
