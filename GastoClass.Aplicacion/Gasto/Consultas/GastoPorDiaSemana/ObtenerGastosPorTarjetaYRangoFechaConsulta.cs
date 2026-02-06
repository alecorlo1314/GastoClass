using GastoClass.GastoClass.Aplicacion.Gasto.DTOs;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Gasto.Consultas.GastoPorDiaSemana;

public record ObtenerGastosPorTarjetaYRangoFechaConsulta(int idTarjeta, int mes, int anio)
    : IRequest<List<GastoPorDiaSemanaDto>> { }
