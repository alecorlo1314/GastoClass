using GastoClass.Dominio.Excepciones;

namespace GastoClass.Dominio.ValueObjects.ValuePreferencias;

public class IconoTarjeta
{
    public string Valor { get; } 

    public IconoTarjeta(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ExcepcionDominio(nameof(Valor),("El icono de tarjeta no puede estar vacío"));

        Valor = valor;
    }
}
