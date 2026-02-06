using GastoClass.GastoClass.Aplicacion.HistorialGasto.DTOs;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.HistorialGasto.Consultas;

public record ObtenerGastosConsulta
    : IRequest<List<GastoHistorialDto>?> { }
