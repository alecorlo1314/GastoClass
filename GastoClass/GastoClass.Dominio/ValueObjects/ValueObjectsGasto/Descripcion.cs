using GastoClass.Dominio.Excepciones.ExcepcionesGasto;
namespace GastoClass.GastoClass.Dominio.ValueObjects.ValueObjectsGasto;

public readonly record struct Descripcion
{
    public string Valor { get; }

    public Descripcion(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ExcepcionDominio(nameof(Valor), "La descripción es requerida");

        valor = valor.Trim();

        if (valor.Length < 3)
            throw new ExcepcionDominio(nameof(Valor),
                "La descripción debe tener al menos 3 caracteres");

        if (valor.Length > 200)
            throw new ExcepcionDominio(nameof(Valor), 
                "Debe tener entre 3 y 200 caracteres");

        Valor = valor;
    }

    public override string ToString() => Valor;
}
