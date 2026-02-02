using GastoClass.Dominio.Excepciones.ExcepcionesGasto;

namespace GastoClass.GastoClass.Dominio.ValueObjects.ValueObjectsGasto;

public readonly record struct Descripcion
{
    public string Valor { get; }

    public Descripcion(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor) || string.IsNullOrEmpty(valor))
        {
            throw new ExcepcionDescripcionInvalida(nameof(valor), "Descripcion es requerida");
        }
        Valor = valor.Trim();
        if (valor.Length > 200)
        {
            throw new ExcepcionDescripcionInvalida(nameof(valor), "No puede ser mayor a 200 caracteres");
        }
        if (valor.Length < 3)
        {
            throw new ExcepcionDescripcionInvalida(nameof(valor), "No puede ser menor a 3 caracteres");
        }
        Valor = valor;
    }
    public override string ToString() => Valor;
}
