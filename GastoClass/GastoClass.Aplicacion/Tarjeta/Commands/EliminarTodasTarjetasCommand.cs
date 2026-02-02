using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Tarjeta.Commands;

public record EliminarTodasTarjetasCommand
    : IRequest<int>
{
}
