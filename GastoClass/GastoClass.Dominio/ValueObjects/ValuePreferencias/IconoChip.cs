using GastoClass.Dominio.Excepciones;

namespace GastoClass.GastoClass.Dominio.ValueObjects.ValuePreferencias;

public class IconoChip
{
    public string Valor { get; }

    public IconoChip(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ExcepcionDominio(nameof(Valor),"El icono de chip no puede estar vacío");

        Valor = valor;
    }
}
