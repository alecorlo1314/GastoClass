using GastoClass.Aplicacion.Interfaces;
using GastoClass.Aplicacion.Servicios.DTOs;
using System.Net.Http.Json;

namespace GastoClass.Aplicacion.Servicios.Consultas.CategoriaPredicha;

public class PrediccionCategoriaServicio(HttpClient httpClient) : IPrediccionCategoriaServicio
{
    public async Task<CategoriaPredichaDto?> PredecirAsync(string descripcion)
    {
        var solicitud = new SolicitudPrediccion
        {
            Descripcion = descripcion
        };

        var response = await httpClient
            .PostAsJsonAsync("https://localhost:55402/api/v1/Predict", solicitud);

        if (!response.IsSuccessStatusCode)
            return null;

        var respuesta = await response.Content
            .ReadFromJsonAsync<CategoriaPredichaDto>();

        return new CategoriaPredichaDto
        {
            CategoriaPrincipal = respuesta!.CategoriaPrincipal,
            Confidencial = respuesta.Confidencial,
            ScoreDict = respuesta.ScoreDict
        };
    }
}
