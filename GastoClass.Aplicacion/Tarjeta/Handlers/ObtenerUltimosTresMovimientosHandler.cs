using GastoClass.Dominio.Excepciones;
using GastoClass.Dominio.Interfaces;
using GastoClass.Aplicacion.Tarjeta.DTOs;
using MediatR;

namespace GastoClass.Aplicacion.Tarjeta.Consultas;

class ObtenerUltimosTresMovimientosHandler(IRepositorioGasto repositorioGasto, IRepositorioTarjetaCredito repositorioTarjetaCredito)
    : IRequestHandler<ObtenerUltimosTresMovimientosConsulta, List<UltimosTresMovimientosDto>?>
{
    public async Task<List<UltimosTresMovimientosDto>?> Handle(ObtenerUltimosTresMovimientosConsulta request, CancellationToken cancellationToken)
    {
        var gastos = await repositorioGasto.ObtenerTodosAsync();
        if (!gastos!.Any()) throw new ExcepcionDominio("Excepcion de negocio","No se encontro el gastos");
        var tarjeta = await repositorioTarjetaCredito.ObtenerPorIdAsync(request.idTarjeta);
        if (tarjeta == null) throw new ExcepcionDominio("Excepcion de negocio", "No se encontro la tarjeta");

        //Mapear el gasto y tarjeta
        var resultados = (from gasto in gastos
                          where gasto.TarjetaId.IdTarjeta == request.idTarjeta
                          select new UltimosTresMovimientosDto
                          {
                              Imagen = gasto.NombreImagen!.Value.Valor,
                              Descripcion = gasto.Descripcion.Valor,
                              Fecha = gasto.Fecha.Valor,
                              Categoria = gasto.Categoria.Valor,
                              UltimosCuatroDigitos = tarjeta.UltimosCuatroDigitos.Valor,
                              Estado = gasto.Estado.Valor,
                              Monto = gasto.Monto.Valor
                          }).Take(3).ToList();
        return resultados;
    }
}
