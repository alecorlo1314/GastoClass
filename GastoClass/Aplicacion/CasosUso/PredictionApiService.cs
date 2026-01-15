

using GastoClass.Dominio.Model;
using System.Net.Http.Json;

namespace GastoClass.Aplicacion.CasosUso
{
    public class PredictionApiService
    {
        private readonly HttpClient _httpClient;

        public PredictionApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResultadoPrediccion> PredictAsync(string descripcion)
        {
            var request = new SolicitudPrediccion
            {
                Descripcion = descripcion
            };

            var response = await _httpClient
                .PostAsJsonAsync("https://localhost:55402/api/v1/Predict", request);

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content
                .ReadFromJsonAsync<RespuestaPrediccion>();

            return new ResultadoPrediccion
            {
                Categoria = result!.Categoria,
                Confidencial = result.Confidencial,
                scoreDict = result.scoreDict
            };
        }
    }
}
