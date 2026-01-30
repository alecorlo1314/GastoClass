using GastoClass.Aplicacion.Dashboard.Consultas;
using GastoClass.Aplicacion.Servicios.DTOs;
using MediatR;

namespace GastoClass.Aplicacion.Dashboard.Handlers;

public class ObtenerPrediccionCategoriaHandler
    : IRequestHandler<ObtenerPrediccionCategoriaConsulta, CategoriaPredichaDto>
{
    public Task<CategoriaPredichaDto> Handle(ObtenerPrediccionCategoriaConsulta request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
