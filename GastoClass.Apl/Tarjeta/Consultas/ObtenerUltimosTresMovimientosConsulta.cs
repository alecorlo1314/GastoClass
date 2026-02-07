using GastoClass.GastoClass.Aplicacion.Tarjeta.DTOs;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Tarjeta.Consultas;

public record ObtenerUltimosTresMovimientosConsulta(int idTarjeta)
    : IRequest<List<UltimosTresMovimientosDto>>
{
}
