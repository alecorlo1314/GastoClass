using GastoClass.Dominio.Excepciones;
using GastoClass.Dominio.Interfaces;
using GastoClass.Aplicacion.Common;
using GastoClass.Aplicacion.Dashboard.DTOs;
using MediatR;

namespace GastoClass.Aplicacion.Dashboard.Consultas.UltimosCincoGastos;

public class ObtenerUltimosCincoGastosHandler(
    IRepositorioGasto repositorioGasto)
    : IRequestHandler<ObtenerUltimosCincoGastosConsulta, ResultadoConsulta<List<UltimoCincoGastosDto>>>
{
    public async Task<ResultadoConsulta<List<UltimoCincoGastosDto>>> Handle(
        ObtenerUltimosCincoGastosConsulta request, CancellationToken cancellationToken)
    {
        try
        {
            var gastos = await repositorioGasto.ObtenerTodosAsync();

            var resultado = gastos!
                .OrderByDescending(g => g.Fecha.Valor)
                .Take(5)
                .Select(g => new UltimoCincoGastosDto
                {
                    Icono = g.NombreImagen!.Value.Valor ?? "default.png",
                    Descripcion = g.Descripcion.Valor,
                    Fecha = g.Fecha.Valor!,
                    Monto = g.Monto.Valor
                })
                .ToList();

            return ResultadoConsulta<List<UltimoCincoGastosDto>>.Ok(resultado);
        }
        catch (ExcepcionDominio ex)
        {
            return ResultadoConsulta<List<UltimoCincoGastosDto>>
                .Fallo(ex.Message);
        }
        catch (Exception)
        {
            return ResultadoConsulta<List<UltimoCincoGastosDto>>
                .Fallo("Ocurrió un error al cargar los últimos gastos.");
        }
    }
}
