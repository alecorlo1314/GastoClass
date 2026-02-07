using GastoClass.Aplicacion.Tarjeta.DTOs;
using MediatR;

namespace GastoClass.Aplicacion.Tarjeta.Consultas;

public record ObtenerPreferenciaTarjetaConsulta(int IdTarjeta)
    : IRequest<PreferenciaTarjetaDto?> { }
