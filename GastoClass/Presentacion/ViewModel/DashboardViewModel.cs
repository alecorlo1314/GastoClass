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
    private readonly PredictionApiService _serviceML;
    private readonly ServicioGastos _gastoService;
    #endregion

    //Timer para retardo en prediccion
    private static timer.Timer? _timer;
    private CancellationTokenSource? _cts;

    #region Listas Observables
    [ObservableProperty]
    private ObservableCollection<ResultadoPrediccion> listaResultadoPredicciones = new();
    //Esta lista contendra las categorias recomendadas basadas en la prediccion del modelo ML
    [ObservableProperty]
    private ObservableCollection<CategoriasRecomendadas> categoriasRecomendadas = new ();
    //Lista Observable para el grafico cirular que se basara en la cantidad de dinero gastado por categoria
    // -Se traera la categoria y el monto gastado en esa categoria
    // -Se tiene que sumar los montos gastados por cada categoria
    // -Incluirlo en una clase GastoPorCategoria con propiedades Categoria y MontoTotal
    [ObservableProperty]
    private ObservableCollection<Gasto> gastoPorCategoriasMes = new();
    [ObservableProperty]
    private ObservableCollection<Gasto>? ultimosCincoMovimientos = new();
    public IDictionary<string, float>? ListaConPuntos { get; set; }
    #endregion

    [ObservableProperty]
    private string? _descripcion;
    [ObservableProperty]
    private CategoriasRecomendadas categoriaRecomendadaML;
    [ObservableProperty]
    private CategoriasRecomendadas? categoriaFinal; // la que se guarda
    [ObservableProperty]
    private string? _monto;
    [ObservableProperty]
    private DateTime _fecha = DateTime.Now;
    [ObservableProperty]
    private decimal _gastoTotalMes;
    [ObservableProperty]
    private int _cantidadTransacciones;
    [ObservableProperty]
    private string? _mensajeCantidadTransacciones;

    public DashboardViewModel(PredictionApiService predictionApiService, ServicioGastos servicioGastos)
    {
        //Inyeccion de dependencias
        _serviceML = predictionApiService;
        _gastoService = servicioGastos;

        listaResultadoPredicciones = new ObservableCollection<ResultadoPrediccion>();
        _ = TotalGastadoEsteMes();
        _ = CantidadTransaccionesEsteMes();
        _ = CargarGastosPorCategoria();
        _ = ObtenerUltimos5GastosAsync();
        _timer = new System.Timers.Timer(500); // medio segundo de intervalo
        _timer.AutoReset = false;
        _timer.Elapsed += async (_, _) =>
        {
            await MainThread.InvokeOnMainThreadAsync(TiempoRealPrediccionAsync);
        };
    }
    //Metodo para manejar el cambio en la descripcion al momento de escribir y actualizar la categoria recomendada y lista de categorias
    partial void OnDescripcionChanged(string? oldValue, string? newValue)
    {
        //Validar que la descripcion sea valida
        if (!EsDescripcionValida(newValue)) return;
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

    //Metodo para obtener la prediccion en tiempo real, se usan en los Thread del timer y en el cambio de descripcion
    private async Task TiempoRealPrediccionAsync()
    {
        //Validar que la descripcion sea valida
        if (!EsDescripcionValida(Descripcion)) return;

        try
        {
            //Obtener la prediccion desde el servicio
            var prediction = await _serviceML.PredictAsync(Descripcion!);
            //Limpiar opciones de prediccion antes de agregar nuevas
            ListaResultadoPredicciones?.Clear();
            //Agregar la nueva prediccion a la lista
            if (prediction != null)
                ListaResultadoPredicciones?.Add(prediction);
            //Actualizar la categoria recomendada y se mostrara asi (Alimentos 80%)
            CategoriaRecomendadaML = new CategoriasRecomendadas
            {
                DescripcionCategoriaRecomendada = prediction!.Categoria,
                ScoreCategoriaRecomendada = prediction.Confidencial
            };
            CategoriaFinal = CategoriaRecomendadaML;
            //Limpiar lista con puntos antes de agregar nuevas
            ListaConPuntos?.Clear();
            //Asignar la nueva lista con puntos
            ListaConPuntos = prediction.scoreDict;
            //Cargar categorias recomendadas basadas en la prediccion
            await CargarCategoriasMLRecomendadas();
        }
        catch (Exception ex)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync("Error en predicción", ex.Message, "OK");
        }
    }

    //Metodo para cargar las categorias recomendadas basadas en la prediccion
    private async Task CargarCategoriasMLRecomendadas()
    {
        try
        {
            //Cargar categorias recomendadas al iniciar recorriende la ListaConPuntos
            if (ListaConPuntos == null) return;
            //Limpiar categorias recomendadas antes de agregar nuevas
            CategoriasRecomendadas?.Clear();
            //Asignar la lista ordenada a ListaConPuntos
            foreach (var (key, value) in ListaConPuntos)
            {
                CategoriasRecomendadas?.Add(new CategoriasRecomendadas
                {
                    DescripcionCategoriaRecomendada = key,
                    ScoreCategoriaRecomendada = value
                });
            }
            //ordernar lista de puntos, desdendentemente por puntos
            var listaPuntosOrdenadas = ListaConPuntos.
                OrderByDescending(s => s.Value);
        }
        catch(Exception ex)
        {
           await Shell.Current.CurrentPage.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

 
    [RelayCommand]
    private async Task AgregarGasto()
    {
        //Validar entradas antes de agregar gasto
        if (CategoriasRecomendadas == null || CategoriasRecomendadas.Count == 0)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync("Error", "No hay categorias recomendadas disponibles.", "OK");
            return;
        }
        if (CategoriaFinal == null) {
            await Shell.Current.CurrentPage.DisplayAlertAsync("Error", "Debe seleccionar una categoría", "OK"); 
            return; 
        }
        try
        {
            //Crear un nuevo gasto basado en los datos ingresados
            var gasto = new Gasto
            {
                Descripcion = Descripcion,
                Categoria = CategoriaFinal.DescripcionCategoriaRecomendada,
                Monto = decimal.Parse(Monto),
                Fecha = Fecha
            };

            //Realizar la consulta al servicio de agregar gasto
            var resultado = await _gastoService.GuardarGastoAsync(gasto);

            if (resultado == 1)
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync("Éxito", "Gasto agregado correctamente.", "OK");
                //Limpiar campos despues de agregar gasto
                Descripcion = string.Empty;
                CategoriaRecomendadaML = new CategoriasRecomendadas();
                Monto = string.Empty;
                ListaResultadoPredicciones?.Clear();
                CategoriasRecomendadas?.Clear();
                ListaConPuntos?.Clear();
                CategoriaFinal = null;
                _ = TotalGastadoEsteMes();
                _ = CantidadTransaccionesEsteMes();
                _ = CargarGastosPorCategoria();
                _ = ObtenerUltimos5GastosAsync();
            }
            else
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync("Error", "No se pudo agregar el gasto.", "OK");
            }
        }catch (NullReferenceException)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync("Error", "Por favor, complete todos los campos antes de agregar el gasto.", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    #region Metodo de Validacion
    private bool EsDescripcionValida(string? texto)
    => !string.IsNullOrWhiteSpace(texto) && texto.Length >= 3;
    private bool EsNumeroValido()
    {
        if (Monto == null) return false;
        if (string.IsNullOrWhiteSpace(Monto)) return false;
        if (decimal.TryParse(Monto, out decimal numero))
        {
            return numero > 0;
        }
        return false;
    }
    #endregion
    //Metodo carga asincrona inicial
    //Metodo para obtener gastos totales del mes
    private async Task TotalGastadoEsteMes()
    {
        try
        {
            var total = await _gastoService.ObtenerGastosTotalesDelMesAsync(DateTime.Now.Month, DateTime.Now.Year);
            GastoTotalMes = total;
        }
        catch (Exception ex)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }
    //Metodo para obtener cantidad de transacciones en este mes
    private async Task CantidadTransaccionesEsteMes()
    {
        try
        {
            var total = await _gastoService.ObtenerTransaccionesDelMesAsync(DateTime.Now.Month, DateTime.Now.Year);
            CantidadTransacciones = total;
            _ = MostrarMensajeCantidadTransacciones();
        }
        catch (Exception ex)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }
    private async Task MostrarMensajeCantidadTransacciones()
    {
        if (CantidadTransacciones == 0)
        {
            MensajeCantidadTransacciones = "No se han registrado transacciones este mes.";
        }
        if (CantidadTransacciones == 1)
        {
            MensajeCantidadTransacciones = "Basado en 1 transaccion de este mes.";
        }
        if (CantidadTransacciones > 1)
        {
            MensajeCantidadTransacciones = $"Basado en {CantidadTransacciones} transaccion de este mes.";
        }
    }
    //Metodo para cargar gastos por categoria
    private async Task CargarGastosPorCategoria()
    {
        try
        {
            var gastosPorCategoria = await _gastoService.ObtenerGastoTotalPorCategoriaMesAsync(DateTime.Now.Month, DateTime.Now.Year);
            GastoPorCategoriasMes.Clear();
            foreach (var gasto in gastosPorCategoria)
            {
                GastoPorCategoriasMes.Add(gasto);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }
    //Metodo para obtener los ultimos 5 gastos
    private async Task ObtenerUltimos5GastosAsync()
    {
        try
        {
            var ultimos5Gastos = await _gastoService.ObtenerUltimos5GastosAsync();
            //Se agregan los ultimos 5 gastos a una lista observable 
            ultimosCincoMovimientos?.Clear();
            foreach (var gasto in ultimos5Gastos)
            {
                ultimosCincoMovimientos?.Add(gasto);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }
}