public class ExcepcionDominio : Exception
{
    public string Campo { get; }

    public ExcepcionDominio(string campo, string mensaje)
        : base(mensaje)
    {
        Campo = campo;
    }
}
