namespace MLCategorias_WebApi.DTO
{
    public class PredictionResponseDto
    {
        public string Categoria { get; set; }
        public float Confidencial { get; set; }

        //lista de categorias en prediction.Score
        public IDictionary<string, float> scoreDict { get; set; }
    }

}
