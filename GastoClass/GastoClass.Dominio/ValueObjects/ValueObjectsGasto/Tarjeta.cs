using GastoClass.Dominio.Excepciones;

public readonly record struct Tarjeta
{
    public int IdTarjeta { get; }

    public Tarjeta(int idTarjeta)
    {
        if (idTarjeta <= 0)
            throw new ExcepcionDominio(
                nameof(IdTarjeta),
                "Debes seleccionar una tarjeta válida"
            );

        IdTarjeta = idTarjeta;
    }
}
