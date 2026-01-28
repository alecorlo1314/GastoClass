using GastoClass.Dominio.Entidades;
using Infraestructura.Persistencia.Entidades;

namespace Infraestructura.Mapper;

public static class PrefereciasTarjetaMapper
{
    public static PreferenciasTarjetaEntidad ToEntidad(PreferenciaTarjeta preferenciaTarjeta)
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
    public static PreferenciaTarjeta ToDomain(PreferenciasTarjetaEntidad entity)
    {
        var preferencia = new PreferenciaTarjeta(
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
        
