using GastoClass.Aplicacion.DTOs;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Tarjeta.Consultas;

public record ObtenerTarjetaCreditoConsulta
    : IRequest<List<DetallesTarjetaDto>?>
{
}
