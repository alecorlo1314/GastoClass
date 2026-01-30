using GastoClass.Dominio.Excepciones.ExcepcionesGasto;

namespace GastoClass.GastoClass.Dominio.ValueObjects.ValueObjectsGasto;

public readonly record struct Estado
{
    public string? Valor { get; }
    public Estado(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ExcepcionEstadoInvalido(nameof(valor),"El estado es requerido");
        }
        Valor = valor;
    }
}
