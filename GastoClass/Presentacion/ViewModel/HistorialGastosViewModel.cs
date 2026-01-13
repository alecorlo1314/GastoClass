using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GastoClass.Aplicacion.CasosUso;
using GastoClass.Dominio.Model;
using System.Collections.ObjectModel;

namespace GastoClass.Presentacion.ViewModel
{
    /// <summary>
    /// ViewModel encargado de:
    /// - Mostrar el historial de gastos
    /// - Filtrar gastos por texto
    /// - Editar un gasto existente
    /// - Solicitar sugerencias de categorías mediante ML
    /// </summary>
    public partial class HistorialGastosViewModel : ObservableObject
    {
        #region Dependencias

        /// <summary>
        /// Servicio encargado del acceso y gestión de gastos
        /// </summary>
        private readonly ServicioGastos _gastoService;

        /// <summary>
        /// Servicio que consume la API de predicción de categorías (ML)
        /// </summary>
        private readonly PredictionApiService _servicioPrediccionCategoriaML;

        #endregion

        #region Colecciones observables

        /// <summary>
        /// Lista completa de gastos obtenidos desde la base de datos
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Gasto>? listaGastos = new();

        /// <summary>
        /// Lista de gastos filtrados para la búsqueda
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Gasto>? listaGastosFiltrados = new();

        /// <summary>
        /// Lista final de categorías que se mostrarán al usuario
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<CategoriasRecomendadas> listaCategoriaFinal = new();

        /// <summary>
        /// Categorías sugeridas por el modelo de Machine Learning
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<ResultadoPrediccion> listaCategoriasSugeridasML = new();

        #endregion

        #region Propiedades de búsqueda

        /// <summary>
        /// Texto utilizado para filtrar el historial de gastos
        /// </summary>
        [ObservableProperty]
        private string? textoBusqueda;

        #endregion

        #region Propiedades de edición

        /// <summary>
        /// Monto del gasto seleccionado para edición
        /// </summary>
        [ObservableProperty]
        private decimal montoSeleccionado;

        /// <summary>
        /// Descripción del gasto seleccionado
        /// </summary>
        [ObservableProperty]
        private string? descripcionSeleccionada;

        /// <summary>
        /// Categoría seleccionada (manual o sugerida por ML)
        /// </summary>
        [ObservableProperty]
        private string? categoriaSeleccionada;

        /// <summary>
        /// Fecha del gasto seleccionado
        /// </summary>
        [ObservableProperty]
        private DateTime fechaSeleccionada;

        #endregion

        #region Control de estado interno

        /// <summary>
        /// Descripción original del gasto antes de ser editado
        /// </summary>
        private string? _descripcionOriginal;

        /// <summary>
        /// Indica si el ViewModel se encuentra en modo edición
        /// </summary>
        private bool _modoEdicion;

        /// <summary>
        /// Indica si la descripción fue modificada por el usuario
        /// </summary>
        private bool _descripcionCambiadaPorUsuario;

        /// <summary>
        /// Token de cancelación para controlar el debounce de predicción
        /// </summary>
        private CancellationTokenSource? _cts;

        /// <summary>
        /// Categoría recomendada principal por el modelo ML
        /// </summary>
        private CategoriasRecomendadas categoriaRecomendadaML;

        #endregion

        #region Constructor

        /// <summary>
        /// Inicializa el ViewModel e inyecta las dependencias necesarias
        /// </summary>
        public HistorialGastosViewModel(
            ServicioGastos gastoService,
            PredictionApiService servicioPrediccionCategoriaML)
        {
            _gastoService = gastoService;
            _servicioPrediccionCategoriaML = servicioPrediccionCategoriaML;

            // Carga inicial del historial de gastos
            _ = CargarListaMovimientos();
        }

        #endregion

        #region Carga de datos

        /// <summary>
        /// Obtiene los gastos desde la base de datos y carga las listas observables
        /// </summary>
        private async Task CargarListaMovimientos()
        {
            try
            {
                var consulta = await _gastoService.ObtenerGastosAsync();

                ListaGastos?.Clear();

                foreach (var gasto in consulta)
                    ListaGastos?.Add(gasto);

                ListaGastosFiltrados = new ObservableCollection<Gasto>(ListaGastos!);
            }
            catch (Exception)
            {
                // Manejo de errores (logging o notificación)
            }
        }

        #endregion

        #region Búsqueda y filtrado

        /// <summary>
        /// Se ejecuta automáticamente cuando cambia el texto de búsqueda
        /// </summary>
        partial void OnTextoBusquedaChanged(string? value)
        {
            _ = Filtrar(value);
        }

        /// <summary>
        /// Filtra la lista de gastos por descripción, categoría, id o monto
        /// </summary>
        private async Task Filtrar(string? textoBusqueda)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textoBusqueda))
                {
                    ListaGastosFiltrados = new ObservableCollection<Gasto>(ListaGastos!);
                    return;
                }

                textoBusqueda = textoBusqueda.ToLower();

                var resultado = ListaGastos?.Where(g =>
                    g.Descripcion!.ToLower().Contains(textoBusqueda) ||
                    g.Id!.ToString().Contains(textoBusqueda) ||
                    g.Categoria!.ToLower().Contains(textoBusqueda) ||
                    g.Monto.ToString().Contains(textoBusqueda));

                ListaGastosFiltrados = new ObservableCollection<Gasto>(resultado!);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
            }
        }

        #endregion

        #region Edición de gastos

        /// <summary>
        /// Prepara el ViewModel para editar un gasto seleccionado
        /// </summary>
        [RelayCommand]
        private void EditarGasto(Gasto gasto)
        {
            _modoEdicion = true;
            _descripcionOriginal = gasto.Descripcion;
            _descripcionCambiadaPorUsuario = false;

            MontoSeleccionado = gasto.Monto;
            DescripcionSeleccionada = gasto.Descripcion;
            FechaSeleccionada = gasto.Fecha;
            CategoriaSeleccionada = gasto.Categoria;

            ListaCategoriasSugeridasML?.Clear();
            ListaCategoriaFinal?.Clear();
        }

        /// <summary>
        /// Se ejecuta cuando cambia la descripción del gasto en edición
        /// </summary>
        partial void OnDescripcionSeleccionadaChanged(string? value)
        {
            if (!_modoEdicion)
                return;

            if (!EsDescripcionValida(value))
                return;

            if (value == _descripcionOriginal)
            {
                ListaCategoriasSugeridasML?.Clear();
                return;
            }

            _descripcionCambiadaPorUsuario = true;

            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            _ = Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(500, _cts.Token);

                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        await CargarCategoriasSugeridasMLAsync();
                    });
                }
                catch (TaskCanceledException) { }
            });
        }

        #endregion

        #region Predicción ML

        /// <summary>
        /// Obtiene categorías sugeridas por el modelo ML según la descripción
        /// </summary>
        private async Task CargarCategoriasSugeridasMLAsync()
        {
            if (!_descripcionCambiadaPorUsuario)
                return;

            if (!EsDescripcionValida(DescripcionSeleccionada))
                return;

            try
            {
                var prediction = await _servicioPrediccionCategoriaML
                    .PredictAsync(DescripcionSeleccionada!);

                if (prediction == null)
                    return;

                ListaCategoriasSugeridasML?.Clear();
                ListaCategoriasSugeridasML?.Add(prediction);

                CategoriaSeleccionada = prediction.Categoria;

                ListaCategoriaFinal = new ObservableCollection<CategoriasRecomendadas>
                {
                    new()
                    {
                        DescripcionCategoriaRecomendada = prediction.Categoria,
                        ScoreCategoriaRecomendada = prediction.Confidencial
                    }
                };
            }
            catch (Exception ex)
            {
                await Shell.Current.CurrentPage
                    .DisplayAlertAsync("Error ML", ex.Message, "OK");
            }
        }

        #endregion

        #region Validaciones

        /// <summary>
        /// Valida que la descripción tenga un formato mínimo válido
        /// </summary>
        private bool EsDescripcionValida(string? texto)
            => !string.IsNullOrWhiteSpace(texto) && texto.Length >= 3;

        #endregion
    }
}
