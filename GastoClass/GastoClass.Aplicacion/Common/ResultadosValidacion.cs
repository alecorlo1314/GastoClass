namespace GastoClass.Aplicacion.Common;

/// <summary>
/// Representa los resultados de una validación
/// Contiene:
/// - EsValido si no hay errores en el dicionario Errores
/// - Diccionario de Errores que contiene el campo y el mensaje
/// </summary>
public class ResultadosValidacion
{
    public bool EsValido => !Errores.Any() && Popup == null;

    public Dictionary<string, string> Errores { get; } = new();

    public PopupError? Popup { get; set; }
}

public record PopupError(string Titulo, string Mensaje);

