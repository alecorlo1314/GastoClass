using MediatR;

namespace GastoClass.Aplicacion.Tarjeta.Commands;

public record EliminarTodasTarjetasCommand
    : IRequest<int>
{
}
