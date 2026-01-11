

namespace GastoClass.Dominio.Model
{
    public class CategoriasRecomendadas
    {
        public string? DescripcionCategoriaRecomendada { get; set; }
        public float ScoreCategoriaRecomendada { get; set; }
        //mostrar en porcentaje
        public string Display =>
            $"{DescripcionCategoriaRecomendada} ({ScoreCategoriaRecomendada:P0})";
    }
}
