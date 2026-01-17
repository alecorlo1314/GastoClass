using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GastoClass.Aplicacion.CasosUso;
using GastoClass.Dominio.Model;
using System.Collections.ObjectModel;

namespace GastoClass.Presentacion.ViewModel
{
    public partial class MLDetallesViewModel : ObservableObject
    {
        #region Inyeccion de dependencias
        /// <summary>
        /// Para obtener los servicios las predicciones
        /// </summary>
        private readonly PredictionApiService _predictionApiService;
        #endregion

        #region Constructor
        public MLDetallesViewModel(PredictionApiService predictionApiService)
        {
            //Inyeccion de dependencias
            _predictionApiService = predictionApiService;
            //inicializar datos
            ResultadosVisibles = false;        
        }
        #endregion

        #region Propiedades de UI
        /// <summary>
        /// Propiedad que toma la descripcion para la prediccion de la categoria
        /// </summary>
        [ObservableProperty]
        private string? _descripcion;

        /// <summary>
        /// Permite mostrar los resultados de las predicciones en la UI
        /// </summary>
        [ObservableProperty]
        private bool? _resultadosVisibles = false;

        /// <summary>
        /// Oculata o muestra el boton de predecir
        /// </summary>
        [ObservableProperty]
        private bool? _botonPredecirOculto;

        /// <summary>
        /// Oculata o muestra el boton de predecir
        /// </summary>
        [ObservableProperty]
        private bool? _botonCargandoOculto = false;
        #endregion

        #region Propiedades Tiempo
        /// <summary>
        /// Cancelacion de cualquier token en curso
        /// </summary>
        private CancellationTokenSource? _cts;
        #endregion

        #region Objetos
        /// <summary>
        /// Contiene la categoria recomendada por el ML
        /// </summary>
        [ObservableProperty]
        private CategoriasRecomendadas? _categoriaRecomendadaML;

        /// <summary>
        /// Contiene el resultado de la prediccion
        /// </summary>
        [ObservableProperty]
        private ResultadoPrediccion? _resultadoPrediccion;
        #endregion

        #region Listas
        /// <summary>
        /// Contiene las categorias recomendadas con descripcio y puntaje
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<CategoriasRecomendadas>? _categoriasRecomendadas = new();
        #endregion

        #region Tiempo de Prediccion
        /// <summary>
        /// Utilizado para la gestion del tiempo en las predicciones
        /// </summary>
        /// <returns></returns>
        private async Task TiempoRealPrediccionAsync()
        {
            //Validar que la descripcion sea valida
            if (!EsDescripcionValida(Descripcion))
            {
                CategoriaRecomendadaML = null;
                return;
            }

            try
            {
                //Obtener la prediccion desde el servicio
                var prediccion = await _predictionApiService.PredictAsync(Descripcion!);
                //Limpiar la lista de categorias recomendadas
                CategoriasRecomendadas?.Clear();
                //Agregar la nueva prediccion a la lista
                foreach (var (key, value) in prediccion!.scoreDict!)
                {
                    CategoriasRecomendadas?.Add(new CategoriasRecomendadas
                    {
                        DescripcionCategoriaRecomendada = key,
                        ScoreCategoriaRecomendada = value * 100
                    });
                }
                // ordernar lista de puntos, desdendentemente por puntos
                var ordenada = CategoriasRecomendadas
                    ?.OrderBy(s => s.ScoreCategoriaRecomendada)
                    .ToList();
                //Actualizar la lista final con los datos ordenados por scores
                CategoriasRecomendadas = new ObservableCollection<CategoriasRecomendadas>(ordenada!);

                //Actualizar la categoria recomendada y se mostrara asi (Alimentos 80%)
                CategoriaRecomendadaML = new CategoriasRecomendadas
                {
                    DescripcionCategoriaRecomendada = prediccion!.Categoria,
                    ScoreCategoriaRecomendada = prediccion.Confidencial
                };
                ResultadosVisibles = true;
                BotonPredecirOculto = true;
                BotonCargandoOculto = false;
            }
            catch (Exception ex)
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync("Error en TiempoRealPrediccionAsync: ", ex.Message, "OK");
            }
        }
        #endregion

        #region Realizar Prediccion
        [RelayCommand]
        private async Task RealizarPrediccion()
        {
            try
            {
                //Validar que la descripcion sea valida
                if (!EsDescripcionValida(Descripcion)) return;

                //SE OCULTA EL BOTON DE PREDECIR
                BotonPredecirOculto = true;
                //Se muestra el boton de carga
                BotonCargandoOculto = !BotonPredecirOculto;
                //Cancelar cualquier prediccion en curso
                _cts?.Cancel();
                //Crear un nuevo token de cancelacion
                _cts = new CancellationTokenSource();
                //Iniciar una nueva tarea para la prediccion con retardo
                _ = Task.Run(async () =>
                {
                    try
                    {
                        //Esperar el retardo antes de hacer la prediccion
                        await Task.Delay(500, _cts.Token);
                        //No se hara nada hasta que esta operacion termine
                        await MainThread.InvokeOnMainThreadAsync(TiempoRealPrediccionAsync);
                    }
                    catch (TaskCanceledException) { }
                });
            }
            catch (Exception ex)
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync("Error en RealizarPrediccion: ", ex.Message, "OK");
            }
            finally
            {
                BotonPredecirOculto = false;
                BotonCargandoOculto = !BotonPredecirOculto;
            }
        }
        #endregion

        #region Metodo de Validacion
        private bool EsDescripcionValida(string? texto)
        => !string.IsNullOrWhiteSpace(texto) && texto.Length >= 3;
        #endregion
    }
}
