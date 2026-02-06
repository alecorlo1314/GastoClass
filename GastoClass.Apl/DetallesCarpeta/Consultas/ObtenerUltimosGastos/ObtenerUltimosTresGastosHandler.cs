using GastoClass.Dominio.Interfaces;
using MediatR;

namespace GastoClass.Aplicacion.DetallesCarpeta.Consultas.ObtenerUltimosGastos;

public class ObtenerUltimosTresGastosHandler(IRepositorioGasto repositorioGasto, IRepositorioTarjetaCredito repositorioTarjetaCredito)
    : IRequestHandler<ObtenerUltimosTresGastosConsulta, List<GastoUltimosTresDto>>
{
    public async Task<List<GastoUltimosTresDto>> Handle(ObtenerUltimosTresGastosConsulta request, CancellationToken cancellationToken)
    {
        var listaGastos = await repositorioGasto.ObtenerTodosAsync();
        var listaTarjetasCredito = await repositorioTarjetaCredito.ObtenerTodosAsync();

        var tarjeta = listaTarjetasCredito!.FirstOrDefault(t => t.Id == request.IdTarjeta);
        if (tarjeta is null) return new List<GastoUltimosTresDto>();

        return listaGastos!
            .Where(g => g.TarjetaId == request.IdTarjeta)
            .OrderByDescending(g => g.Fecha.Valor)
            .Take(3)
            .Select(g => new GastoUltimosTresDto
            {
                IconoCategoriaGasto = g.NombreImagen!.Value.Valor,
                DescripcionGasto = g.Descripcion.Valor,
                FechaGasto = g.Fecha.Valor!.Value,
                CategoriaGasto = g.Categoria.Valor,
                UltimosCuatroDigitosTarjeta = tarjeta.UltimosCuatroDigitos.Valor,
                EstadoGasto = g.Estado.Valor,
                MontoGasto = g.Monto.Valor
            })
            .ToList();
    }

}
