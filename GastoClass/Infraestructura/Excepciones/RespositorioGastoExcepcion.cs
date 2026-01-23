namespace GastoClass.Infraestructura.Excepciones;

public class RespositorioGastoExcepcion : Exception
{
    public RespositorioGastoExcepcion(string mensaje, Exception? capturaExcepcion = null) : base(mensaje, capturaExcepcion) { }
    public RespositorioGastoExcepcion(string mensaje) : base(mensaje) { }
}
