using GastoClass.Aplicacion.Dashboard.DTOs;
using MediatR;

namespace GastoClass.Aplicacion.Dashboard.Consultas;

public record ObtenerPrediccionCategoriaConsulta(string Descripcion)
    : IRequest<PrediccionCategoriaDto>
{
}
