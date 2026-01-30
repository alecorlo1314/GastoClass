using GastoClass.Dominio.Excepciones.ExcepcionesGasto;
using GastoClass.GastoClass.Dominio.Excepciones.ExcepcionesGasto;

namespace GastoClass.GastoClass.Dominio.ValueObjects.ValueObjectsGasto;

public readonly record struct Comercio
{
    public string? Valor { get; }

    public Comercio(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ExcepcionComercioRequerido(nameof(valor), "Comercio es requerido");
        }
        if (string.IsNullOrEmpty(valor))
        {
            throw new ExcepcionComercioRequerido(nameof(valor), "Comercio es requerido");
        } 
        if (valor.Length > 100)
        {
            throw new ExcepcionComercioMaximoCaracteres(nameof(valor), "Comercio debe tener un maximo de 100 caracteres");
        }
        Valor = valor;
    }

}
