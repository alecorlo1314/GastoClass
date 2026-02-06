using GastoClass.Aplicacion.Tarjeta.DTOs;
using MediatR;

namespace GastoClass.Aplicacion.Tarjeta.Consultas;

public record ObtenerGastoPorTarjetaCreditoConsulta
    : IRequest<List<GastoTarjetaDto>?>
{
}
