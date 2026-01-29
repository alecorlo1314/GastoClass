using GastoClass.Dominio.Entidades;
using Infraestructura.Persistencia.Entidades;

namespace Infraestructura.Mapper;

public static class PrefereciasTarjetaMapper
{
    public static PreferenciasTarjetaEntidad ToEntidad(PreferenciaTarjetaDominio preferenciaTarjeta)
    {
        return new PreferenciasTarjetaEntidad
        {
            ColorHex1 = preferenciaTarjeta.ColorHex1,
            ColorHex2 = preferenciaTarjeta.ColorHex2,
            ColorTexto = preferenciaTarjeta.ColorTexto,
            ColorBorde = preferenciaTarjeta.ColorBorde,
            IconoTipoTarjeta = preferenciaTarjeta.IconoTipoTarjeta,
            IconoChip = preferenciaTarjeta.IconoChip
        };
    }
    public static PreferenciaTarjetaDominio ToDomain(PreferenciasTarjetaEntidad entity)
    {
        var preferencia = new PreferenciaTarjetaDominio(
                entity.Id,
                entity.ColorHex1,
                entity.ColorHex2,
                entity.ColorTexto,
                entity.ColorBorde,
                entity.IconoTipoTarjeta,
                entity.IconoChip
                );
        return preferencia;
    }
}
        
