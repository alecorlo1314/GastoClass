using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GastoClass.Aplicacion.CasosUso;
using GastoClass.Dominio.Model;
using System.Collections.ObjectModel;
using timer = System.Timers;

namespace GastoClass.Presentacion.ViewModel;

//Se conectara a los casos de uso del dominio para obtener datos
//Obtendra datos del modelo y los preparara para la vista
//Se usaran metodos para obener datos relacionados a lo siguiente:
// - Gastos totales del mes
// - Cantidad de transacciones en este mes
// - Categoria con mayor gasto
// - Grafica de gastos por categoria
// - Gastos recientes (los ultimos 5 gastos)

//Tareas para Agregar Gastos
//Validacion de entradas:
// - Monto debe ser un numero positivo 
// - Desccripcion debe ser mayor a 3 caracteres
// - Categoria debe actualizarce en tiempo real con un retardo de 500ms despues de dejar de escribir
// - Categoria debe sugerirse automaticamente usando un modelo de ML basado en la descripcion del gasto
// - Categoria debe mostrar una sugerencia basada en la descripcion del gasto
// - Categoria debe mostrar una lista con la probabilidad de cada categoria basada en el modelo de ML
public partial class DashboardViewModel : ObservableObject
{
    #region Inyeccion de Dependencias
    private readonly PredictionApiService _service;
    #endregion

    //Timer para retardo en prediccion
    private static timer.Timer? _timer;
    private CancellationTokenSource? _cts;

    #region Listas Observables
    [ObservableProperty]
    private ObservableCollection<PredictionOption>? predictionOptions = new();
    [ObservableProperty]
    private ObservableCollection<CategoriasRecomendadas> categoriasRecomendadas = new ();
    public IDictionary<string, float>? ListaConPuntos { get; set; }
    #endregion

    [ObservableProperty]
    private string? _descripcion;
    [ObservableProperty]
    private string? _categoriaRecomendada;

    public DashboardViewModel(PredictionApiService predictionApiService)
    {
        //Inyeccion de dependencias
        _service = predictionApiService;

        PredictionOptions = new ObservableCollection<PredictionOption>();

        _timer = new System.Timers.Timer(500); // medio segundo de intervalo
        _timer.AutoReset = false;
        _timer.Elapsed += async (_, _) =>
        {
            await MainThread.InvokeOnMainThreadAsync(TiempoRealPrediccionAsync);
        };
    }

    partial void OnDescripcionChanged(string? oldValue, string? newValue)
    {
        if (!EsDescripcionValida(newValue)) return;

        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        _ = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(500, _cts.Token);
                //No se hara nada hasta que esta operacion termine
                await MainThread.InvokeOnMainThreadAsync(TiempoRealPrediccionAsync);
            }
            catch (TaskCanceledException) { }
        });
    }

    private async Task TiempoRealPrediccionAsync()
    {
        if (!EsDescripcionValida(Descripcion)) return;

        try
        {
            var prediction = await _service.PredictAsync(Descripcion);

            PredictionOptions?.Clear();

            if (prediction != null)
                PredictionOptions?.Add(prediction);
            CategoriaRecomendada = prediction.Display;
            ListaConPuntos?.Clear();
            ListaConPuntos = prediction.scoreDict;
            await CargarCategoriasMLRecomendadas();
        }
        catch (Exception ex)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync("Error en predicción", ex.Message, "OK");
        }
    }

    private async Task CargarCategoriasMLRecomendadas()
    {
        try
        {
            //Cargar categorias recomendadas al iniciar recorriende la ListaConPuntos
            if (ListaConPuntos == null) return;
            //Limpiar categorias recomendadas antes de agregar nuevas
            CategoriasRecomendadas?.Clear();

            //ordernar lista de puntos, desdendentemente por puntos
            var listaPuntosOrdenadas = ListaConPuntos.
                OrderByDescending(s => s.Value);
            foreach (var (key, value) in ListaConPuntos)
            {
                CategoriasRecomendadas?.Add(new CategoriasRecomendadas
                {
                    DescripcionCategoriaRecomendada = key,
                    ScoreCategoriaRecomendada = value
                });
            }
        }catch(Exception ex)
        {
           await Shell.Current.CurrentPage.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }
    private bool EsDescripcionValida(string? texto) 
        => !string.IsNullOrWhiteSpace(texto) && texto.Length >= 3;
    //Metodo carga asincrona inicial
    //Metodo para obtener gastos totales del mes
    //Metodo para obtener cantidad de transacciones en este mes
    //Metodo para obtener categoria con mayor gasto
    //Metodo para obtener los ultimos 5 gastos

    //Comando Abrir ventana de formulario de gastos
    [RelayCommand]
    public async Task PaginaAgregarGasto()
    {
        //Resultado del formulario
    }
}
