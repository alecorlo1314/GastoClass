namespace GastoClass.Aplicacion.Servicios.DTOs;

public class TfidfData
{
    public Dictionary<string, int> vocabulary { get; set; }
    public List<double> idf { get; set; }
    public int max_features { get; set; }
    public List<int> ngram_range { get; set; }
}
