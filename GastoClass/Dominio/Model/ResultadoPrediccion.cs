
namespace GastoClass.Dominio.Model
{
    public class ResultadoPrediccion
    {
        public string Categoria { get; set; }
        public float Confidencial { get; set; }
        //lista de categorias en prediction.Score
        public IDictionary<string, float> scoreDict { get; set; }

        public string Display =>
            $"{Categoria} ({Confidencial:P0})";
    }

}
