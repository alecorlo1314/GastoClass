using GastoClass.Dominio.Excepciones.ExcepcionesGasto;

namespace GastoClass.Dominio.ValueObjects.ValueObjectsGasto;

public readonly record struct NombreImagen
{
    public NombreImagen(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ExcepcionNombreImagenRequerido();
        }
        Valor = valor;
    }

    public string? Valor { get; }
    
}
