namespace GastoClass.GastoClass.Dominio.ValueObjects.ValueObjectsGasto;

public readonly record struct Tarjeta
{
    public int idTarjeta { get; }
    public Tarjeta(int idTarjeta)
    {
        if (string.IsNullOrWhiteSpace(idTarjeta.ToString()) || string.IsNullOrEmpty(idTarjeta.ToString()))
            throw new ArgumentException(nameof(idTarjeta),"Debes seleccionar una tarjeta");
    }
}
