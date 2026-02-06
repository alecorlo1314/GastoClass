using GastoClass.Dominio.ValueObjects.ValuePreferencias;
using GastoClass.GastoClass.Dominio.ValueObjects.ValuePreferencias;

namespace GastoClass.Dominio.Entidades;

public class PreferenciaTarjetaDominio
{
    public int Id { get; private set; }
    public ColorHex ColorHex1 { get; private set; }
    public ColorHex ColorHex2 { get; private set; }
    public ColorHex ColorBorde { get; private set; }
    public ColorHex ColorTexto { get; private set; }
    public IconoTarjeta IconoTipoTarjeta { get; private set; }
    public IconoChip IconoChip { get; private set; }

    public PreferenciaTarjetaDominio(
        int id,
        ColorHex colorHex1,
        ColorHex colorHex2,
        ColorHex colorBorde,
        ColorHex colorTexto,
        IconoTarjeta iconoTipoTarjeta,
        IconoChip iconoChip)
    {
        Id = id;
        ColorHex1 = new ColorHex(colorHex1.Valor);
        ColorHex2 = new ColorHex(colorHex2.Valor);
        ColorBorde = new ColorHex(colorBorde.Valor);
        ColorTexto = new ColorHex(colorTexto.Valor);
        IconoTipoTarjeta = new IconoTarjeta(iconoTipoTarjeta.Valor);
        IconoChip = new IconoChip(iconoChip.Valor);
    }

    public static PreferenciaTarjetaDominio Crear(
           string? ColorHex1,
           string? ColorHex2,
           string? ColorBorde,
           string? ColorTexto,
           string? IconoTipoTarjeta,
           string? IconoChip)
    {
        return new PreferenciaTarjetaDominio(
            id: 0,
            new ColorHex(ColorHex1!),
            new ColorHex(ColorHex2!),
            new ColorHex(ColorBorde!),
            new ColorHex(ColorTexto!),
            new IconoTarjeta(IconoTipoTarjeta!),
            new IconoChip(IconoChip!));
    }
    public void SetId(int id) => Id = id;
}
