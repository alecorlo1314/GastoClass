using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GastoClass.Aplicacion.CasosUso;
using GastoClass.Dominio.Model;
using System.Collections.ObjectModel;
using System.Timers;

namespace GastoClass.Presentacion.ViewModel;

/// <summary>
/// ViewModel principal del Dashboard
/// Gestiona la visualización de gastos, predicciones ML y operaciones CRUD
/// </summary>
public partial class DashboardViewModel : ObservableObject
{
    #region Servicios e Inyección de Dependencias

    private readonly PredictionApiService _serviceML;
    private readonly ServicioGastos _gastoService;

    #endregion

    #region Propiedades para Control de Predicción ML

    // Timer para retardo de 500ms en predicciones
    private static System.Timers.Timer? _timer;
    // Token de cancelación para predicciones en curso
    private CancellationTokenSource? _cts;

    #endregion

    #region Propiedades Observables - Formulario de Gasto

    /// <summary>
    /// Descripción del gasto ingresada por el usuario
    /// Al cambiar, dispara la predicción ML con retardo
    /// </summary>
    [ObservableProperty]
    private string? _descripcion;

    /// <summary>
    /// Monto del gasto en formato string
    /// </summary>
    [ObservableProperty]
    private string? _monto;

    /// <summary>
    /// Fecha del gasto, por defecto es hoy
    /// </summary>
    [ObservableProperty]
    private DateTime _fecha = DateTime.Now;

    /// <summary>
    /// Categoría recomendada por el modelo ML
    /// </summary>
    [ObservableProperty]
    private CategoriasRecomendadas? categoriaRecomendadaML;

    /// <summary>
    /// Categoría final seleccionada que se guardará en BD
    /// </summary>
    [ObservableProperty]
    private CategoriasRecomendadas? categoriaFinal;

    #endregion

    #region Propiedades Observables - Datos del Dashboard

    /// <summary>
    /// Total de dinero gastado en el mes actual
    /// </summary>
    [ObservableProperty]
    private decimal _gastoTotalMes;

    /// <summary>
    /// Cantidad de transacciones registradas en el mes
    /// </summary>
    [ObservableProperty]
    private int _cantidadTransacciones;

    /// <summary>
    /// Mensaje descriptivo sobre las transacciones del mes
    /// </summary>
    [ObservableProperty]
    private string? _mensajeCantidadTransacciones;

    #endregion

    #region Colecciones Observables

    /// <summary>
    /// Lista de resultados de predicción del modelo ML
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<ResultadoPrediccion> listaResultadoPredicciones = new();

    /// <summary>
    /// Lista de categorías recomendadas con sus probabilidades
    /// Ordenadas por score descendente
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<CategoriasRecomendadas> categoriasRecomendadas = new();

    /// <summary>
    /// Gastos agrupados por categoría para gráfico circular
    /// Contiene el monto total gastado por cada categoría en el mes
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<Gasto> gastoPorCategoriasMes = new();

    /// <summary>
    /// Últimos 5 movimientos/gastos registrados
    /// Para mostrar en el dashboard
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<Gasto>? ultimosCincoMovimientos = new();

    /// <summary>
    /// Diccionario con categorías y sus probabilidades
    /// Key: Nombre de categoría, Value: Probabilidad (0-1)
    /// </summary>
    public IDictionary<string, float>? ListaConPuntos { get; set; }

    #endregion

    #region Constructor

    /// <summary>
    /// Constructor del ViewModel
    /// Inicializa servicios, timer y carga datos iniciales del dashboard
    /// </summary>
    public DashboardViewModel(PredictionApiService predictionApiService, ServicioGastos servicioGastos)
    {
        // Inyección de dependencias
        _serviceML = predictionApiService;
        _gastoService = servicioGastos;

        // Cargar datos iniciales del dashboard
        _ = TotalGastadoEsteMes();
        _ = CantidadTransaccionesEsteMes();
        _ = CargarGastosPorCategoria();
        _ = ObtenerUltimos5GastosAsync();

        // Configurar timer para predicciones ML con retardo de 500ms
        _timer = new System.Timers.Timer(500);
        _timer.AutoReset = false;
        _timer.Elapsed += async (_, _) =>
        {
            await MainThread.InvokeOnMainThreadAsync(TiempoRealPrediccionAsync);
        };
    }

    #endregion

    #region Manejadores de Cambios de Propiedades

    /// <summary>
    /// Se ejecuta cuando cambia la descripción del gasto
    /// Cancela predicciones previas e inicia nueva predicción con retardo de 500ms
    /// </summary>
    partial void OnDescripcionChanged(string? oldValue, string? newValue)
    {
        // Validar que la descripción sea válida (mínimo 3 caracteres)
        if (!EsDescripcionValida(newValue)) return;

        // Cancelar cualquier predicción en curso
        _cts?.Cancel();

        // Crear nuevo token de cancelación
        _cts = new CancellationTokenSource();

        // Iniciar tarea de predicción con retardo
        _ = Task.Run(async () =>
        {
            try
            {
                // Esperar 500ms antes de hacer la predicción
                await Task.Delay(500, _cts.Token);

                // Ejecutar predicción en el hilo principal
                await MainThread.InvokeOnMainThreadAsync(TiempoRealPrediccionAsync);
            }
            catch (TaskCanceledException)
            {
                // La tarea fue cancelada porque el usuario siguió escribiendo
            }
        });
    }

    #endregion

    #region Métodos de Predicción ML

    /// <summary>
    /// Obtiene predicción en tiempo real del modelo ML
    /// Actualiza categoría recomendada y lista de probabilidades
    /// </summary>
    private async Task TiempoRealPrediccionAsync()
    {
        // Validar que la descripción sea válida
        if (!EsDescripcionValida(Descripcion)) return;

        try
        {
            // Obtener predicción desde el servicio ML
            var prediction = await _serviceML.PredictAsync(Descripcion!);

            // Limpiar lista de predicciones anteriores
            ListaResultadoPredicciones?.Clear();

            // Agregar nueva predicción
            if (prediction != null)
                ListaResultadoPredicciones?.Add(prediction);

            // Actualizar categoría recomendada con mayor probabilidad
            CategoriaRecomendadaML = new CategoriasRecomendadas
            {
                DescripcionCategoriaRecomendada = prediction!.Categoria,
                ScoreCategoriaRecomendada = prediction.Confidencial
            };

            // Establecer como categoría final por defecto
            CategoriaFinal = CategoriaRecomendadaML;

            // Limpiar y actualizar diccionario de probabilidades
            ListaConPuntos?.Clear();
            ListaConPuntos = prediction.scoreDict;

            // Cargar todas las categorías con sus probabilidades
            await CargarCategoriasMLRecomendadas();
        }
        catch (Exception ex)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync("Error en predicción", ex.Message, "OK");
        }
    }

    /// <summary>
    /// Carga la lista de categorías recomendadas ordenadas por probabilidad
    /// Convierte el diccionario de puntos en una colección observable
    /// </summary>
    private async Task CargarCategoriasMLRecomendadas()
    {
        try
        {
            // Validar que exista el diccionario de puntos
            if (ListaConPuntos == null) return;

            // Limpiar categorías anteriores
            CategoriasRecomendadas?.Clear();

            // Agregar cada categoría con su probabilidad a la colección
            foreach (var (key, value) in ListaConPuntos)
            {
                CategoriasRecomendadas?.Add(new CategoriasRecomendadas
                {
                    DescripcionCategoriaRecomendada = key,
                    ScoreCategoriaRecomendada = value
                });
            }

            // Ordenar lista descendentemente por probabilidad
            var listaPuntosOrdenadas = ListaConPuntos.OrderByDescending(s => s.Value);
        }
        catch (Exception ex)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    #endregion

    #region Comandos - Operaciones CRUD

    /// <summary>
    /// Comando para agregar un nuevo gasto
    /// Valida datos, guarda en BD y actualiza el dashboard
    /// </summary>
    [RelayCommand]
    private async Task AgregarGasto()
    {
        // Validar monto
        if (!EsNumeroValido())
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync("Error", "Ingrese un monto válido mayor a 0", "OK");
            return;
        }
        // Validar que existan categorías recomendadas
        if (CategoriasRecomendadas == null || CategoriasRecomendadas.Count == 0)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync("Error", "No hay categorias recomendadas disponibles.", "OK");
            return;
        }

        // Validar que se haya seleccionado una categoría
        if (CategoriaFinal == null)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync("Error", "Debe seleccionar una categoría", "OK");
            return;
        }

        try
        {
            // Crear objeto gasto con los datos del formulario
            var gasto = new Gasto
            {
                Descripcion = Descripcion,
                Categoria = CategoriaFinal.DescripcionCategoriaRecomendada,
                Monto = decimal.Parse(Monto!),
                Fecha = Fecha
            };

            // Guardar gasto en la base de datos
            var resultado = await _gastoService.GuardarGastoAsync(gasto);

            if (resultado == 1)
            {
                // Gasto guardado exitosamente
                await Shell.Current.CurrentPage.DisplayAlertAsync("Éxito", "Gasto agregado correctamente.", "OK");

                // Limpiar formulario
                Descripcion = string.Empty;
                CategoriaRecomendadaML = new CategoriasRecomendadas();
                Monto = string.Empty;
                ListaResultadoPredicciones?.Clear();
                CategoriasRecomendadas?.Clear();
                ListaConPuntos?.Clear();
                CategoriaFinal = null;

                // Recargar datos del dashboard
                _ = TotalGastadoEsteMes();
                _ = CantidadTransaccionesEsteMes();
                _ = CargarGastosPorCategoria();
                _ = ObtenerUltimos5GastosAsync();
            }
            else
            {
                // Error al guardar
                await Shell.Current.CurrentPage.DisplayAlertAsync("Error", "No se pudo agregar el gasto.", "OK");
            }
        }
        catch (NullReferenceException)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync("Error", "Por favor, complete todos los campos antes de agregar el gasto.", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    #endregion

    #region Métodos de Validación

    /// <summary>
    /// Valida que la descripción tenga al menos 3 caracteres
    /// </summary>
    private bool EsDescripcionValida(string? texto)
        => !string.IsNullOrWhiteSpace(texto) && texto.Length >= 3;

    /// <summary>
    /// Valida que el monto sea un número positivo
    /// </summary>
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

    #region Métodos de Carga de Datos del Dashboard

    /// <summary>
    /// Obtiene el total de dinero gastado en el mes actual
    /// Actualiza la propiedad GastoTotalMes
    /// </summary>
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

    /// <summary>
    /// Obtiene la cantidad de transacciones realizadas en el mes actual
    /// Actualiza el mensaje descriptivo de transacciones
    /// </summary>
    private async Task CantidadTransaccionesEsteMes()
    {
        try
        {
            var total = await _gastoService.ObtenerTransaccionesDelMesAsync(DateTime.Now.Month, DateTime.Now.Year);
            CantidadTransacciones = total;
           
            MostrarMensajeCantidadTransacciones();
        }
        catch (Exception ex)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    /// <summary>
    /// Genera mensaje descriptivo basado en la cantidad de transacciones
    /// </summary>
    private void MostrarMensajeCantidadTransacciones()
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
            MensajeCantidadTransacciones = $"Basado en {CantidadTransacciones} transacciones de este mes.";
        }
    }

    /// <summary>
    /// Carga los gastos agrupados por categoría del mes actual
    /// Para mostrar en gráfico circular
    /// </summary>
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

    /// <summary>
    /// Obtiene los últimos 5 gastos registrados
    /// Para mostrar en la sección de movimientos recientes
    /// </summary>
    private async Task ObtenerUltimos5GastosAsync()
    {
        try
        {
            var ultimos5Gastos = await _gastoService.ObtenerUltimos5GastosAsync();

            // Limpiar y agregar los últimos 5 gastos a la colección observable
            UltimosCincoMovimientos?.Clear();

            foreach (var gasto in ultimos5Gastos)
            {
                UltimosCincoMovimientos?.Add(new Gasto
                {
                    Id = gasto.Id,
                    Monto = gasto.Monto,
                    Fecha = gasto.Fecha,
                    Descripcion = gasto.Descripcion,
                    Categoria = gasto.Categoria,
                    NombreImagen = $"icono_{gasto.Categoria?.ToLower()}.png"
                });
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    #endregion

    #region Metodo de limpieza de suprocesos
    /// <summary>
    /// Para detener el timer y evitar problemas de rendimiento
    /// </summary>
    public void Dispose()
    {
        _timer?.Stop();
        _timer?.Dispose();
        _cts?.Cancel();
        _cts?.Dispose();
    }
    #endregion
}