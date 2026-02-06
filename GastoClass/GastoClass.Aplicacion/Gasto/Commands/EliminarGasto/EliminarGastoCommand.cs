using GastoClass.Aplicacion.Common;
using MediatR;

namespace GastoClass.Aplicacion.Gasto.Commands.EliminarGasto;

public record EliminarGastoCommand(int IdCommand)
    : IRequest<ResultadosValidacion> { }
