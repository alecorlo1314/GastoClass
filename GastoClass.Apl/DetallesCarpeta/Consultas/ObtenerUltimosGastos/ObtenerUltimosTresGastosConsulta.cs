using MediatR;

namespace GastoClass.Aplicacion.DetallesCarpeta.Consultas.ObtenerUltimosGastos;

public record ObtenerUltimosTresGastosConsulta(int? IdTarjeta)
    : IRequest<List<GastoUltimosTresDto>>
{
}
