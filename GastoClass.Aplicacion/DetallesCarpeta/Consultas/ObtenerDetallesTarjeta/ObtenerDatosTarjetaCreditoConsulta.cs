using MediatR;

namespace GastoClass.Aplicacion.DetallesCarpeta.Consultas.ObtenerDetallesTarjeta;

public record ObtenerDatosTarjetaCreditoConsulta(int? IdTarjeta)
    : IRequest<DatosTarjetaCreditoDto>
{
}
