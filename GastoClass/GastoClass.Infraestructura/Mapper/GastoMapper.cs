using GastoClass.Dominio.Entidades;
using GastoClass.GastoClass.Dominio.ValueObjects.ValueObjectsGasto;

using Infraestructura.Persistencia.Entidades;

namespace Infraestructura.Mapper;

public static class GastoMapper
{
    public static GastoEntidad ToEntidad(GastoDominio gastoDominio)
    {
        return new GastoEntidad
        {
            Descripcion = gastoDominio.Descripcion.Valor,
            Monto = gastoDominio.Monto.Valor,
            Categoria = gastoDominio.Categoria.Valor,
            Fecha = gastoDominio.Fecha.Valor!,
            Comercio = gastoDominio.Comercio!.Value.Valor,
            Estado = gastoDominio.Estado.Valor,
            NombreImagen = gastoDominio.NombreImagen!.Value.Valor,
            TarjetaId = gastoDominio.TarjetaId.IdTarjeta
        };
    }

    public static GastoDominio ToDomain(GastoEntidad GastoEntidad)
    {
        var gasto = new GastoDominio(
                new Descripcion(GastoEntidad.Descripcion!),
                new Monto(GastoEntidad.Monto),
                new Categoria(GastoEntidad.Categoria!),
                new Comercio(GastoEntidad.Comercio!),
                new Fecha(GastoEntidad.Fecha),
                new Estado(GastoEntidad.Estado!),
                new NombreImagen(GastoEntidad.NombreImagen!),
                new Tarjeta(GastoEntidad.TarjetaId)
                );

        gasto.SetId(GastoEntidad.Id);
        return gasto;
    }
}
