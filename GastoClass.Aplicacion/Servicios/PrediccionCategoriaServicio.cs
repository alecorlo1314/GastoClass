using GastoClass.Aplicacion.Dashboard.DTOs;
using GastoClass.Aplicacion.Interfaces;
using System.Net.Http.Json;

namespace GastoClass.Aplicacion.Servicios;

public class PrediccionCategoriaServicio(HttpClient httpClient) : IPrediccionCategoriaServicio
{
    public async Task<PrediccionCategoriaDto?> PredecirAsync(string descripcion)
    {
        var response = await httpClient.PostAsJsonAsync("https://localhost:55402/api/v1/Predict", descripcion);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<PrediccionCategoriaDto>();
    }
}
