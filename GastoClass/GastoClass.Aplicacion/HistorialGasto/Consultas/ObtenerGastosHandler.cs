using GastoClass.GastoClass.Aplicacion.HistorialGasto.DTOs;
using GastoClass.GastoClass.Dominio.Interfaces;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.HistorialGasto.Consultas;

public class ObtenerGastosHandler(IRepositorioGasto repositorioGastos)
    : IRequestHandler<ObtenerGastosConsulta, List<GastoHistorialDto>?>
{
    public async Task<List<GastoHistorialDto>?> Handle(ObtenerGastosConsulta request, CancellationToken cancellationToken)
    {
        //Obtener Gastos
        var gastos = await repositorioGastos.ObtenerTodosAsync();
        List<GastoHistorialDto>? gastosDto = new(
            gastos!
            .Select(g => new GastoHistorialDto
            {
                Categoria = g.Categoria.Valor,
                Id = g.Id,
                TarjetaId = g.TarjetaId.idTarjeta,
                Fecha = g.Fecha.Valor!.Value,
                Descripcion = g.Descripcion.Valor,
                Monto = g.Monto.Valor
            }));

            return gastosDto;
    }
}
