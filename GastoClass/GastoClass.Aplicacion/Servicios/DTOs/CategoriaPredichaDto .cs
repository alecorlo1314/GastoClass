using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Servicios.DTOs;

public class CategoriaPredichaDto()
{
    public string? CategoriaPrincipal { get; set; }
    public float Confidencial { get; set; }
    //lista de categorias en prediction.Score
    public IDictionary<string, float>? ScoreDict { get; set; }
    public string? MostrarCategoriaPredicha => $"{CategoriaPrincipal} ({Confidencial:P1})";
}
