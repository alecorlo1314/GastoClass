using GastoClass.GastoClass.Aplicacion.Tarjeta.DTOs;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Tarjeta.Consultas;

public record ObtenerPreferenciaTarjetaConsulta(int IdTarjeta)
    : IRequest<PreferenciaTarjetaDto?> { }
