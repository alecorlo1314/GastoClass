using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GastoClass.Aplicacion.Interfaces;
using GastoClass.GastoClass.Aplicacion.Gasto.Commands.AgregarGasto;
using GastoClass.GastoClass.Aplicacion.Gasto.DTOs;
using GastoClass.GastoClass.Aplicacion.Servicios.DTOs;
using MediatR;
using System.Collections.ObjectModel;

namespace GastoClass.Presentacion.ViewModel;

public partial class AgregarGastoViewModel : ObservableObject
{
    #region Inyeccion de Dependencias
    private readonly IMediator _mediator;
    private readonly IPrediccionCategoriaServicio _prediccionCategoriaServicio;

    #endregion

    #region Propiedades del Forumlario
    [ObservableProperty] private string? descripcion;
    [ObservableProperty] private decimal? monto;
    [ObservableProperty] private string? comercio;
    [ObservableProperty] private string? estado;
    [ObservableProperty] private DateTime fecha = DateTime.Now;
    [ObservableProperty] private TarjetaGastoDto tarjetaSeleccionada;

    #endregion

    #region Listas Observables
    [ObservableProperty] private CategoriaPredichaDto categoriaPredicha;
    [ObservableProperty] private ObservableCollection<CategoriaPredichaDto> listaCategoriasPredichas = new();

    #endregion

    #region Propiedades para Control de Predicción ML

    // Timer para retardo de 500ms en predicciones
    private static System.Timers.Timer? _timer;
    // Token de cancelación para predicciones en curso
    private CancellationTokenSource? _cts;

    #endregion

    #region Mensajes de Erro Formulario
    [ObservableProperty] private string? errorMonto;
    [ObservableProperty] private string? errorDescripcion;
    [ObservableProperty] private string? errorTarjeta;
    [ObservableProperty] private string? errorEstado;
    [ObservableProperty] private string? errorFecha;
    [ObservableProperty] private string? errorCategoria;

    #endregion

    #region OnDescripcionChanged
    partial void OnDescripcionChanged(string? value)
    {
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
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    _ = PredecirCategoriaAsync(value);
                });
            }
            catch (TaskCanceledException)
            {
                await Shell.Current.DisplayAlertAsync("Predicción Cancelada", "La predicción de categoría fue cancelada debido a nueva entrada.", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
            }
        });
    }

    #endregion

    #region Predecir Categoria Async
    private async Task PredecirCategoriaAsync(string? descripcion)
    {
        if (string.IsNullOrWhiteSpace(descripcion) || descripcion.Length < 3)
            return;

        var prediccion = await _prediccionCategoriaServicio.PredecirAsync(descripcion);

        //Establecer la categoria precha
        CategoriaPredicha = prediccion;

        //LLenar la lista de categorias predichas
        ListaCategoriasPredichas.Clear();
        foreach (var cat in prediccion.ScoreDict!)
        {
            ListaCategoriasPredichas.Add(new CategoriaPredichaDto
            {
                CategoriaPrincipal = cat.Key,
                Confidencial = cat.Value
            });
        }
    }

    #endregion

    #region Guardar Gasto Command
    [RelayCommand]
    private async Task GuardarGasto()
    {
        var command = new AgregarGastoCommand
        {
            Monto = Monto!.Value,
            Fecha = Fecha,
            TarjetaId = TarjetaSeleccionada.Id,
            Descripcion = Descripcion,
            Categoria = CategoriaPredicha?.CategoriaPrincipal,
            Comercio = Comercio,
            Estado = "Activo",
            NombreImagen = $"icono_{CategoriaPredicha?.CategoriaPrincipal?.ToLower()}.png"
        };
        var resultado = await _mediator.Send(command);

        if (!resultado.EsValido)
        {
            // Mostrar errores de validación
            ErrorMonto = resultado.Errores.ContainsKey(nameof(Monto)) ? resultado.Errores[nameof(Monto)] : null;
            ErrorDescripcion = resultado.Errores.ContainsKey(nameof(Descripcion)) ? resultado.Errores[nameof(Descripcion)] : null;
            ErrorTarjeta = resultado.Errores.ContainsKey(nameof(TarjetaSeleccionada)) ? resultado.Errores[nameof(TarjetaSeleccionada)] : null;
            ErrorEstado = resultado.Errores.ContainsKey(nameof(Estado)) ? resultado.Errores[nameof(Estado)] : null;
            ErrorFecha = resultado.Errores.ContainsKey(nameof(Fecha)) ? resultado.Errores[nameof(Fecha)] : null;
            ErrorCategoria = resultado.Errores.ContainsKey("Categoria") ? resultado.Errores["Categoria"] : null;

        }
        //Borrar espacios
        LimpiarErrores();
        LimpiarCampos();
    }
    #endregion

    #region Limpiar Campos
    private void LimpiarCampos()
    {
        Monto = null;
        Descripcion = null;
        Comercio = null;
    }

    #endregion

    #region Limpiar Errores
    private void LimpiarErrores()
    {
        // Limpiar errores
        ErrorMonto = null;
        ErrorDescripcion = null;
        ErrorTarjeta = null;
        ErrorEstado = null;
        ErrorFecha = null;
        ErrorCategoria = null;
    }

    #endregion
}