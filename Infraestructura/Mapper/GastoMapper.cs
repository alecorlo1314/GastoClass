using GastoClass.Dominio.Entidades;
using GastoClass.Dominio.ValueObjects.ValueObjectsGasto;
using Infraestructura.Persistencia.Entidades;

namespace Infraestructura.Mapper;

public static class GastoMapper
{
    public static GastoEntidad ToEntidad(Gasto gasto)
    {
        return new GastoEntidad
        {
            Descripcion = gasto.Descripcion.Valor,
            Monto = gasto.Monto.Valor,
            Categoria = gasto.Categoria.Valor,
            Fecha = gasto.Fecha.Valor!.Value,
            Comercio = gasto.Comercio!.Value.Valor,
            Estado = gasto.Estado.Valor,
            NombreImagen = gasto.NombreImagen!.Value.Valor,
            TarjetaId = gasto.TarjetaId
        };
    }

    public static Gasto ToDomain(GastoEntidad entity)
    {
        var gasto = new Gasto(
                new Descripcion(entity.Descripcion!),
                new Monto(entity.Monto),
                new Categoria(entity.Categoria!),
                new Comercio(entity.Comercio!),
                new Fecha(entity.Fecha),
                new Estado(entity.Estado!),
                new NombreImagen(entity.NombreImagen!),
                entity.TarjetaId
                );

        gasto.SetId(entity.Id);
        return gasto;
    }
}
