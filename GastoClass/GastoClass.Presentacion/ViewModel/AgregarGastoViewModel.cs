using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GastoClass.Aplicacion.Interfaces;
using GastoClass.GastoClass.Aplicacion.Gasto.Commands.AgregarGasto;
using GastoClass.GastoClass.Aplicacion.Gasto.DTOs;
using GastoClass.GastoClass.Aplicacion.Servicios.DTOs;
using MediatR;
using System.Collections.ObjectModel;

namespace GastoClass.Presentacion.ViewModel;

public partial class AgregarGastoViewModel : ObservableObject, IDisposable
{
    #region Inyeccion de Dependencias
    private readonly IMediator? _mediator;
    private readonly IPrediccionCategoriaServicio? _prediccionCategoriaServicio;
    #endregion

    #region Eventos
    /// <summary>
    /// Evento que se dispara cuando se agrega un gasto
    /// </summary>
    public event Action? GastoAgregado;
    #endregion

    #region Propiedades del Formulario
    [ObservableProperty] private string? descripcion;
    [ObservableProperty] private decimal? monto;
    [ObservableProperty] private string? comercio;
    [ObservableProperty] private string? estado;
    [ObservableProperty] private DateTime fecha = DateTime.Now;
    [ObservableProperty] private TarjetaGastoDto? tarjetaSeleccionada;
    #endregion

    #region Listas Observables
    [ObservableProperty] private CategoriaPredichaDto? categoriaPredicha;
    [ObservableProperty] private ObservableCollection<CategoriaPredichaDto>? listaCategoriasPredichas = new();
    #endregion

    #region Propiedades para Control de Predicción ML
    // Token de cancelación para predicciones en curso
    private CancellationTokenSource? _cts;
    #endregion

    #region Mensajes de Error Formulario
    [ObservableProperty] private string? errorMonto;
    [ObservableProperty] private string? errorDescripcion;
    [ObservableProperty] private string? errorTarjeta;
    [ObservableProperty] private string? errorEstado;
    [ObservableProperty] private string? errorFecha;
    [ObservableProperty] private string? errorCategoria;
    #endregion

    #region Constructor
    public AgregarGastoViewModel(IMediator mediator, IPrediccionCategoriaServicio prediccionCategoriaServicio)
    {
        _mediator = mediator;
        _prediccionCategoriaServicio = prediccionCategoriaServicio;
    }
    #endregion

    #region OnDescripcionChanged
    partial void OnDescripcionChanged(string? value)
    {
        // Cancelar predicción anterior
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();

        var cancellationToken = _cts.Token;

        // Iniciar tarea de predicción con retardo
        _ = Task.Run(async () =>
        {
            try
            {
                // Esperar 500ms antes de hacer la predicción
                await Task.Delay(500, cancellationToken);

                // Si la tarea fue cancelada, salir
                if (cancellationToken.IsCancellationRequested)
                    return;

                // Ejecutar predicción en el hilo principal
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await PredecirCategoriaAsync(value);
                });
            }
            catch (OperationCanceledException)
            {
                // Esto es normal cuando el usuario sigue escribiendo
                // No necesitas mostrar un alert aquí
            }
            catch (Exception ex)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await Shell.Current.CurrentPage.DisplayAlertAsync("Error", ex.Message, "OK");
                });
            }
        }, cancellationToken);
    }
    #endregion

    #region Predecir Categoria Async
    private async Task PredecirCategoriaAsync(string? descripcion)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(descripcion) || descripcion.Length <= 3)
            {
                // Limpiar predicciones si la descripción es muy corta
                CategoriaPredicha = null;
                ListaCategoriasPredichas?.Clear();
                return;
            }

            var prediccion = await _prediccionCategoriaServicio!.PredecirAsync(descripcion);

            if (prediccion == null)
                return;

            // Establecer la categoria predicha
            CategoriaPredicha = prediccion;

            // Llenar la lista de categorias predichas
            ListaCategoriasPredichas!.Clear();

            if (prediccion.ScoreDict != null)
            {
                foreach (var cat in prediccion.ScoreDict)
                {
                    ListaCategoriasPredichas.Add(new CategoriaPredichaDto
                    {
                        CategoriaPrincipal = cat.Key,
                        Confidencial = cat.Value
                    });
                }
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync("Error", $"Error al predecir categoría: {ex.Message}", "OK");
        }
    }
    #endregion

    #region Guardar Gasto Command
    [RelayCommand]
    private async Task GuardarGasto()
    {
        try
        {
            // Cancelar cualquier predicción en curso
            _cts?.Cancel();

            var command = new AgregarGastoCommand
            {
                Monto = Monto!.Value,
                Fecha = Fecha,
                TarjetaId = TarjetaSeleccionada!.Id,
                Descripcion = Descripcion,
                Categoria = CategoriaPredicha?.CategoriaPrincipal,
                Comercio = Comercio,
                Estado = "Activo",
                NombreImagen = $"icono_{CategoriaPredicha?.CategoriaPrincipal?.ToLower()}.png"
            };

            var resultado = await _mediator!.Send(command);

            if (!resultado.EsValido)
            {
                // Mostrar errores de validación
                ErrorMonto = resultado.Errores.ContainsKey(nameof(Monto))
                    ? resultado.Errores[nameof(Monto)] : null;
                ErrorDescripcion = resultado.Errores.ContainsKey(nameof(Descripcion))
                    ? resultado.Errores[nameof(Descripcion)] : null;
                ErrorTarjeta = resultado.Errores.ContainsKey(nameof(TarjetaSeleccionada))
                    ? resultado.Errores[nameof(TarjetaSeleccionada)] : null;
                ErrorEstado = resultado.Errores.ContainsKey(nameof(Estado))
                    ? resultado.Errores[nameof(Estado)] : null;
                ErrorFecha = resultado.Errores.ContainsKey(nameof(Fecha))
                    ? resultado.Errores[nameof(Fecha)] : null;
                ErrorCategoria = resultado.Errores.ContainsKey("Categoria")
                    ? resultado.Errores["Categoria"] : null;
                return;
            }

            // Limpiar espacios
            LimpiarCampos();
            LimpiarErrores();

            // Disparar evento
            GastoAgregado?.Invoke();
        }
        catch (Exception ex)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync("Error",
                $"Error al guardar el gasto: {ex.Message}", "OK");
        }
    }
    #endregion

    #region Limpiar Campos
    private void LimpiarCampos()
    {
        Monto = null;
        Descripcion = null;
        Comercio = null;
        CategoriaPredicha = null;
        ListaCategoriasPredichas?.Clear();
    }
    #endregion

    #region Limpiar Errores
    private void LimpiarErrores()
    {
        ErrorMonto = null;
        ErrorDescripcion = null;
        ErrorTarjeta = null;
        ErrorEstado = null;
        ErrorFecha = null;
        ErrorCategoria = null;
    }
    #endregion

    #region IDisposable
    public void Dispose()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }
    #endregion
}