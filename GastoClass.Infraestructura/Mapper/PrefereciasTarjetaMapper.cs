using GastoClass.Dominio.Entidades;
using GastoClass.Infraestructura.Persistencia.Entidades;

namespace GastoClass.Infraestructura.Mappers;

public static class PrefereciasTarjetaMapper
{
    public static PreferenciasTarjetaEntidad ToEntidad(this PreferenciaTarjetaDominio dominio)
    {
        if (dominio == null)
            throw new ArgumentNullException(nameof(dominio));

        return new PreferenciasTarjetaEntidad
        {
            Id = dominio.Id,
            ColorHex1 = dominio.ColorHex1.Valor,
            ColorHex2 = dominio.ColorHex2.Valor,
            ColorBorde = dominio.ColorBorde.Valor,
            ColorTexto = dominio.ColorTexto.Valor,
            IconoTipoTarjeta = dominio.IconoTipoTarjeta.Valor,
            IconoChip = dominio.IconoChip.Valor
        };
    }
    public static PreferenciaTarjetaDominio ToDominio(this PreferenciasTarjetaEntidad entidad)
    {
        if (entidad == null)
            throw new ArgumentNullException(nameof(entidad));
        var preferencia = PreferenciaTarjetaDominio.Crear(
            ColorHex1: entidad.ColorHex1!,
            ColorHex2: entidad.ColorHex2!,
            ColorBorde: entidad.ColorBorde!,
            ColorTexto: entidad.ColorTexto!,
            IconoTipoTarjeta: entidad.IconoTipoTarjeta!,
            IconoChip: entidad.IconoChip!
        );
        preferencia.SetId(entidad.Id);

        return preferencia;
    }
    public static List<PreferenciaTarjetaDominio> ToDominio(this IEnumerable<PreferenciasTarjetaEntidad> entidades)
    {
        if (entidades == null)
            throw new ArgumentNullException(nameof(entidades));

       List<PreferenciaTarjetaDominio> dominios = new();
        foreach (var entidad in entidades)
        {
            dominios.Add(entidad.ToDominio());
        }
        return dominios;
    }
    public static List<PreferenciasTarjetaEntidad> ToEntidad(this IEnumerable<PreferenciaTarjetaDominio> dominios)
    {
        if (dominios == null)
            throw new ArgumentNullException(nameof(dominios));

        List<PreferenciasTarjetaEntidad> entidades = new();
        foreach (var dominio in dominios)
        {
            entidades.Add(dominio.ToEntidad());
        }
        return entidades;
    }
}
        
