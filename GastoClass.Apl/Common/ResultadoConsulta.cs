using GastoClass.Aplicacion.Common;

namespace GastoClass.GastoClass.Aplicacion.Common;

public class ResultadoConsulta<T>
{
    public bool EsValido => Error == null;
    public T? Datos { get; init; }
    public PopupError? Error { get; init; }

    public static ResultadoConsulta<T> Ok(T datos)
        => new() { Datos = datos };

    public static ResultadoConsulta<T> Fallo(string mensaje)
        => new() { Error = new PopupError("Error", mensaje) };
}
