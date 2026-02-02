using GastoClass.Dominio.Excepciones.ExcepcionesGasto;

namespace GastoClass.GastoClass.Dominio.ValueObjects.ValueObjectsGasto;

public readonly record struct Monto
{
    public decimal Valor { get;}
    public Monto(decimal valor)
    {
        if(string.IsNullOrWhiteSpace(valor.ToString()))
        {
            throw new ExcepcionMontoRequerido(nameof(valor), "Monto es requerido");
        }
        Valor = valor;
        if(valor <= 0)
        {
            throw new ExcepcionMontoNegativo(nameof(valor),"Monto no puede ser 0 o negativo");
        }
        Valor = valor;
    } 
}
