using GastoClass.Dominio.Excepciones.ExcepcionesGasto;

public readonly record struct Estado
{
    public string Valor { get; }

    public Estado(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ExcepcionEstadoInvalido(nameof(Valor), "El estado es requerido");

        Valor = valor.Trim();
    }

    public override string ToString() => Valor;
}
