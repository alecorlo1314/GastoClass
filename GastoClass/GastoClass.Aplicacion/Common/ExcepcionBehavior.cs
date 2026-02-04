using MediatR;

namespace GastoClass.Aplicacion.Common;

public class ExcepcionBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TResponse : ResultadosValidacion,
    new()
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (ExcepcionDominio ex)
        {
            var resultado = new TResponse();
            resultado.Errores.Add(ex.Campo,ex.Message);
            return resultado;
        }
    }
}
