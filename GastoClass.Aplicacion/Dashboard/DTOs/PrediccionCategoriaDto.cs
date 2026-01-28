namespace GastoClass.Aplicacion.Dashboard.DTOs;

public class PrediccionCategoriaDto
{
    public string? Categoria { get; set; }
    public float Confidencial { get; set; }
    //lista de categorias en prediction.Score
    public IDictionary<string, float>? ScoreDict { get; set; }
}
