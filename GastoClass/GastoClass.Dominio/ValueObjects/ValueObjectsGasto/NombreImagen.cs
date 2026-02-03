using GastoClass.Dominio.Excepciones.ExcepcionesGasto;

public readonly record struct Monto
{
    public decimal Valor { get; }

    public Monto(decimal valor)
    {
        if (valor <= 0)
            throw new ExcepcionMontoNegativo(nameof(Valor),
                "El monto debe ser mayor a cero");

        Valor = valor;
    }
}
