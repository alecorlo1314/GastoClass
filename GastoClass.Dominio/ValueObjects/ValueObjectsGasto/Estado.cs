using GastoClass.Dominio.Excepciones.ExcepcionesGasto;

namespace GastoClass.Dominio.ValueObjects.ValueObjectsGasto;

public readonly record struct Estado
{
    public string? Valor { get; }
    public Estado(string? estado)
    {
        if (string.IsNullOrWhiteSpace(estado))
        {
            throw new ExcepcionEstadoInvalido();
        }
        Valor = estado;
    }
}
