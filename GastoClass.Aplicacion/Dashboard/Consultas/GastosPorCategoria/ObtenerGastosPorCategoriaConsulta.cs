using GastoClass.Aplicacion.Dashboard.DTOs;
using MediatR;

namespace GastoClass.Aplicacion.Dashboard.Consultas.GastosPorCategoria;

public record ObtenerGastosPorCategoriaConsulta(int mes, int anio) 
    : IRequest<List<GastoPorCategoriaDto>?> { }
