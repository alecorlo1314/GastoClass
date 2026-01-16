using GastoClass.Aplicacion.CasosUso;

namespace GastoClass.Presentacion.ViewModel
{
    public class MLDetallesViewModel
    {
        #region Inyeccion de dependencias
        /// <summary>
        /// Para obtener los servicios las predicciones
        /// </summary>
        private readonly PredictionApiService _predictionApiService;
        #endregion
        public MLDetallesViewModel(PredictionApiService predictionApiService)
        {
            //Inyeccion de dependencias
            _predictionApiService = predictionApiService;
        }
    }
}
