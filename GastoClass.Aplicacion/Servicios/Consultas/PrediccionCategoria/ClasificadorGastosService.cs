using GastoClass.Aplicacion.Servicios.DTOs;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace GastoClass.Aplicacion.Servicios.Consultas.PrediccionCategoria;

public class ClasificadorGastosService
{
    private readonly InferenceSession _inferenceSession;
    private readonly Dictionary<string, int> _vocabulario;
    private readonly float[] _idf;
    private readonly int _featureCount;

    private readonly string[] _categorias = new[]
    {
            "Entretenimiento","Deportes","Educacion",
            "Alimentacion","Servicios",
            "Ropa","Salud","Viajes","Transporte",
            "Tecnologia","Mascotas","Hogar"
    };

    public ClasificadorGastosService()
    {
        // Carga modelo ONNX
        using var modelStream = FileSystem.OpenAppPackageFileAsync("modelo_mlp.onnx").Result;
        using var modelMs = new MemoryStream();
        modelStream.CopyTo(modelMs);
        _inferenceSession = new InferenceSession(modelMs.ToArray());

        // Carga vocabulario e IDF
        var vocabJson = FileSystem.OpenAppPackageFileAsync("tfidf_data.json").Result;
        using var vocabReader = new StreamReader(vocabJson);
        var json = vocabReader.ReadToEnd();
        var data = JsonSerializer.Deserialize<TfidfData>(json);

        _vocabulario = data!.vocabulary;
        _idf = data.idf.Select(x => (float)x).ToArray();
        _featureCount = _vocabulario.Count;

    }

    public string Clasificar(string descripcion)
    {
        var features = Vectorizar(descripcion);
        var tensor = new DenseTensor<float>(features, new[] { 1, _featureCount });

        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("input", tensor)
        };

        using var results = _inferenceSession.Run(inputs);
        var outputTensor = results.First().AsEnumerable<float>().ToArray();

        // Índice con mayor probabilidad
        int indice = outputTensor
            .Select((val, idx) => (val, idx))
            .OrderByDescending(x => x.val)
            .First().idx;

        return _categorias[indice];
    }

    private float[] Vectorizar(string texto)
    {
        // Exactamente igual que limpiar_texto() en Python
        texto = texto.ToLower().Trim();
        texto = Regex.Replace(texto, @"\s+", " ");  // espacios dobles

        var tokens = texto.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var tf = new Dictionary<int, float>();

        foreach (var token in tokens)
        {
            if (_vocabulario.TryGetValue(token, out int idx))
            {
                tf[idx] = tf.GetValueOrDefault(idx, 0) + 1;
            }
        }

        // TF-IDF
        var vector = new float[_featureCount];
        foreach (var (idx, count) in tf)
        {
            vector[idx] = (count / tokens.Length) * _idf[idx];
        }

        // Normalización L2
        float norm = (float)Math.Sqrt(vector.Sum(x => x * x));
        if (norm > 0)
            for (int i = 0; i < vector.Length; i++)
                vector[i] /= norm;

        return vector;
    }
}

