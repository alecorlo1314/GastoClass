using GastoClass.Aplicacion.Servicios.DTOs;
using MediatR;

namespace GastoClass.Aplicacion.Dashboard.Consultas;

public record ObtenerPrediccionCategoriaConsulta(string Descripcion)
    : IRequest<CategoriaPredichaDto>
{
}
