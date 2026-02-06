using GastoClass.Aplicacion.HistorialGasto.DTOs;
using MediatR;

namespace GastoClass.Aplicacion.HistorialGasto.Consultas;

public record ObtenerGastosConsulta
    : IRequest<List<GastoHistorialDto>?> { }
