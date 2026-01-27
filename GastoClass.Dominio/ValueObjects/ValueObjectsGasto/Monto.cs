using GastoClass.Dominio.Excepciones.ExcepcionesGasto;

namespace GastoClass.Dominio.ValueObjects.ValueObjectsGasto;

public readonly record struct Monto
{
    public decimal Valor { get;}
    public Monto(decimal valor)
    {
        if(string.IsNullOrWhiteSpace(valor.ToString()))
        {
            throw new ExcepcionMontoRequerido();
        }
        Valor = valor;
        if(valor <= 0)
        {
            throw new ExcepcionMontoNegativo();
        }
        Valor = valor;
    } 
}
