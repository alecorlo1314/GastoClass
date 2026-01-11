
namespace GastoClass.Dominio.Model
{
    public class RespuestaPrediccion
    {
        public string Categoria { get; set; }
        public float Confidencial { get; set; }
        //lista de categorias en prediction.Score
        public IDictionary<string, float> scoreDict { get; set; }
    }
}
