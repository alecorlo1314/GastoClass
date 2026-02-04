using GastoClass.GastoClass.Aplicacion.Common;
using GastoClass.GastoClass.Aplicacion.Dashboard.DTOs;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Dashboard.Consultas.GastosPorCategoria;

public record ObtenerGastosPorCategoriaConsulta(int mes, int anio) 
    : IRequest<ResultadoConsulta<List<GastoPorCategoriaDto>?>> { }
