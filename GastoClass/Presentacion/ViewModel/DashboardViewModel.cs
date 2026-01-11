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
public partial class DashboardViewModel : ObservableObject
{
    //Inyeccion de Dependencias
    private readonly PredictionApiService _service;
    private static timer.Timer _timer;

    //Listas Observables
    public ObservableCollection<PredictionOption> PredictionOptions { get; }
    public IDictionary<string, float> ListaConPuntos { get; set; }

    [ObservableProperty]
    private string _descripcion;
    [ObservableProperty]
    private string _categoriaRecomendada;

    public DashboardViewModel(PredictionApiService predictionApiService)
    {
        //Inyeccion de dependencias
        _service = predictionApiService;

        PredictionOptions = new ObservableCollection<PredictionOption>();

        _timer = new System.Timers.Timer(500); // 1 medio segundo de intervalo
        _timer.AutoReset = false;
        _timer.Elapsed += async (_, _) =>
        {
            await MainThread.InvokeOnMainThreadAsync(PredictRealtimeAsync);
        };
    }

    partial void OnDescripcionChanged(string? oldValue, string newValue)
    {
        if(string.IsNullOrWhiteSpace(newValue) || newValue.Length < 3)
        {
            return;
        }
        _timer.Stop();
        _timer.Start();
    }

    private async Task PredictRealtimeAsync()
    {
        if (string.IsNullOrWhiteSpace(Descripcion) || Descripcion.Length < 5)
        {
            return;
        }

       var prediction = await _service.PredictAsync(Descripcion);

        PredictionOptions.Clear();

        if (prediction != null)
            PredictionOptions.Add(prediction);
        CategoriaRecomendada = prediction.Display;
        ListaConPuntos = prediction.scoreDict;
    }

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
