using GastoClass.Aplicacion.Tarjeta.DTOs;
using MediatR;

namespace GastoClass.Aplicacion.Tarjeta.Consultas;

public record ObtenerGastosPorCategoriaSemanaConsulta(int idTarjeta)
    : IRequest<List<GastoCategoriaUltimosSieteDiasTarjetaDto>>
{
}
