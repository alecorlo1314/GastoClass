using GastoClass.Aplicacion.Tarjeta.DTOs;
using MediatR;

namespace GastoClass.Aplicacion.Tarjeta.Consultas;

public record ObtenerUltimosTresMovimientosConsulta(int idTarjeta)
    : IRequest<List<UltimosTresMovimientosDto>>
{
}
