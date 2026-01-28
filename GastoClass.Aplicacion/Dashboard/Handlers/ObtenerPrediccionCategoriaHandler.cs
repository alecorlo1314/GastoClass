using GastoClass.Aplicacion.Dashboard.Consultas;
using GastoClass.Aplicacion.Dashboard.DTOs;
using MediatR;

namespace GastoClass.Aplicacion.Dashboard.Handlers;

public class ObtenerPrediccionCategoriaHandler
    : IRequestHandler<ObtenerPrediccionCategoriaConsulta, PrediccionCategoriaDto>
{
    public Task<PrediccionCategoriaDto> Handle(ObtenerPrediccionCategoriaConsulta request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
