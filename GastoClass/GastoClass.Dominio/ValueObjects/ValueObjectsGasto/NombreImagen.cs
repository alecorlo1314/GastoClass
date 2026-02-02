using GastoClass.Dominio.Excepciones.ExcepcionesGasto;

namespace GastoClass.GastoClass.Dominio.ValueObjects.ValueObjectsGasto;

public readonly record struct NombreImagen
{
    public NombreImagen(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ExcepcionNombreImagenRequerido(nameof(valor),"Nombre imagen es requerida");
        }
        Valor = valor;
    }

    public string? Valor { get; }
    
}
