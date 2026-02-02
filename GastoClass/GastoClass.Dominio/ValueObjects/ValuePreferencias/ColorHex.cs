using GastoClass.Dominio.Excepciones;

namespace GastoClass.GastoClass.Dominio.ValueObjects.ValuePreferencias;

public class ColorHex
{
    public string Valor { get; } 

    public ColorHex(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El color hex no puede estar vacío");

        Valor = valor;
    }
}
