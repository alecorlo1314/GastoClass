using GastoClass.Dominio.Excepciones.ExcepcionesGasto;
namespace GastoClass.GastoClass.Dominio.ValueObjects.ValueObjectsGasto;

public readonly record struct NombreImagen
{
    public string Valor { get; }

    public NombreImagen(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ExcepcionNombreImagenRequerido(nameof(Valor),
                "El nombre de la imagen es requerido");

        Valor = valor.Trim();
    }

    public override string ToString() => Valor;
}

