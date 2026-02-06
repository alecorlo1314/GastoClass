using GastoClass.GastoClass.Aplicacion.Tarjeta.DTOs;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Tarjeta.Consultas;

public record ObtenerGastoPorTarjetaCreditoConsulta
    : IRequest<List<GastoTarjetaDto>?>
{
}
