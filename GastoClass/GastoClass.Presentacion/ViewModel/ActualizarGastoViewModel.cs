using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GastoClass.Aplicacion.Common;
using GastoClass.Aplicacion.Interfaces;
using GastoClass.GastoClass.Aplicacion.Gasto.Commands.ActualizarGasto;
using GastoClass.GastoClass.Aplicacion.Gasto.DTOs;
using GastoClass.GastoClass.Aplicacion.HistorialGasto.DTOs;
using GastoClass.GastoClass.Aplicacion.Servicios.DTOs;
using MediatR;
using System.Collections.ObjectModel;

namespace GastoClass.Presentacion.ViewModel;

/// <summary>
/// ViewModel responsable ÚNICAMENTE de actualizar gastos existentes
/// </summary>
public partial class ActualizarGastoViewModel : ObservableObject, IDisposable
{
    #region Inyección de Dependencias
    private readonly IMediator? _mediator;
    private readonly IPrediccionCategoriaServicio? _prediccionCategoriaServicio;
    #endregion

    #region Eventos
    /// <summary>
    /// Evento que se dispara cuando se actualiza un gasto exitosamente
    /// </summary>
    public event Action? GastoActualizado;
    #endregion

    #region Propiedades del Formulario
    [ObservableProperty] private int? idSeleccionado;
    [ObservableProperty] private string? descripcionSeleccionada;
    [ObservableProperty] private decimal? montoSeleccionado;
    [ObservableProperty] private string? comercioSeleccionado;
    [ObservableProperty] private string? estadoSeleccionado;
    [ObservableProperty] private DateTime fechaSeleccionada = DateTime.Now;
    [ObservableProperty] private TarjetaGastoDto? tarjetaSeleccionada;
    #endregion

    #region Listas Observables
    [ObservableProperty] private CategoriaPredichaDto? categoriaPredicha;
    [ObservableProperty] private ObservableCollection<CategoriaPredichaDto>? listaCategoriasPredichas = new();
    #endregion

    #region Propiedades de Control
    [ObservableProperty] private bool popupVisible;
    private CancellationTokenSource? _cts;
    #endregion

    #region Mensajes de Error
    [ObservableProperty] private string? mensajeErrorMonto;
    [ObservableProperty] private string? mensajeErrorDescripcion;
    [ObservableProperty] private string? mensajeErrorTarjeta;
    [ObservableProperty] private string? mensajeErrorEstado;
    [ObservableProperty] private string? mensajeErrorComercio;
    [ObservableProperty] private string? mensajeErrorFecha;
    [ObservableProperty] private string? mensajeErrorCategoria;

    public bool MostrarErrorMonto => !string.IsNullOrWhiteSpace(MensajeErrorMonto);
    public bool MostrarErrorDescripcion => !string.IsNullOrWhiteSpace(MensajeErrorDescripcion);
    public bool MostrarErrorTarjeta => !string.IsNullOrWhiteSpace(MensajeErrorTarjeta);
    public bool MostrarErrorEstado => !string.IsNullOrWhiteSpace(MensajeErrorEstado);
    public bool MostrarErrorComercio => !string.IsNullOrWhiteSpace(MensajeErrorComercio);
    public bool MostrarErrorFecha => !string.IsNullOrWhiteSpace(MensajeErrorFecha);
    public bool MostrarErrorCategoria => !string.IsNullOrWhiteSpace(MensajeErrorCategoria);
    #endregion

    #region Constructor
    public ActualizarGastoViewModel(
        IMediator mediator,
        IPrediccionCategoriaServicio prediccionCategoriaServicio)
    {
        _mediator = mediator;
        _prediccionCategoriaServicio = prediccionCategoriaServicio;
    }
    #endregion

    #region Cargar Gasto para Edición
    /// <summary>
    /// Carga los datos del gasto seleccionado en el formulario
    /// </summary>
    public void CargarGastoParaEdicion(GastoHistorialDto gasto)
    {
        IdSeleccionado = gasto.Id;
        MontoSeleccionado = gasto.Monto;
        DescripcionSeleccionada = gasto.Descripcion;
        FechaSeleccionada = gasto.Fecha;
        CategoriaPredicha = new CategoriaPredichaDto
        {
            CategoriaPrincipal = gasto.Categoria
        };

        // Limpiar errores y predicciones
        LimpiarErrores();
        ListaCategoriasPredichas?.Clear();

        // Mostrar popup
        PopupVisible = true;
    }
    #endregion

    #region Validaciones en Tiempo Real
    partial void OnDescripcionSeleccionadaChanged(string? value)
    {
        ValidarDescripcion(value);
        DispararPrediccionML(value);
    }

    partial void OnMontoSeleccionadoChanged(decimal? value) => ValidarMonto(value);
    partial void OnCategoriaPredichaChanged(CategoriaPredichaDto? value) => ValidacionCategoria(value);
    partial void OnComercioSeleccionadoChanged(string? value) => ValidarComercio(value);
    partial void OnEstadoSeleccionadoChanged(string? value) => ValidarEstado(value);
    partial void OnFechaSeleccionadaChanged(DateTime value) => ValidarFecha(value);
    partial void OnTarjetaSeleccionadaChanged(TarjetaGastoDto? value) => ValidarTarjeta(value);

    private void ValidarDescripcion(string? descripcion)
    {
        if (string.IsNullOrWhiteSpace(descripcion))
            MensajeErrorDescripcion = "La descripción es requerida";
        else if (descripcion.Length < 3)
            MensajeErrorDescripcion = "La descripción debe tener al menos 3 caracteres";
        else if (descripcion.Length > 200)
            MensajeErrorDescripcion = "La descripción no puede exceder 200 caracteres";
        else
            MensajeErrorDescripcion = null;

        OnPropertyChanged(nameof(MostrarErrorDescripcion));
    }

    private void ValidarMonto(decimal? monto)
    {
        if (monto == null)
            MensajeErrorMonto = "El monto es requerido";
        else if (monto <= 0)
            MensajeErrorMonto = "El monto debe ser mayor a cero";
        else if (monto > 1000000)
            MensajeErrorMonto = "El monto no puede exceder 1,000,000";
        else
            MensajeErrorMonto = null;

        OnPropertyChanged(nameof(MostrarErrorMonto));
    }

    private void ValidacionCategoria(CategoriaPredichaDto? categoria)
    {
        if (categoria == null)
            MensajeErrorCategoria = "La categoría es requerida";
        else
            MensajeErrorCategoria = null;

        OnPropertyChanged(nameof(MostrarErrorCategoria));
    }

    private void ValidarComercio(string? comercio)
    {
        if (string.IsNullOrWhiteSpace(comercio))
            MensajeErrorComercio = "El comercio es requerido";
        else if (comercio.Length < 3)
            MensajeErrorComercio = "El comercio debe tener al menos 3 caracteres";
        else if (comercio.Length > 100)
            MensajeErrorComercio = "El comercio no puede exceder 100 caracteres";
        else
            MensajeErrorComercio = null;

        OnPropertyChanged(nameof(MostrarErrorComercio));
    }

    private void ValidarEstado(string? estado)
    {
        if (string.IsNullOrWhiteSpace(estado))
            MensajeErrorEstado = "El estado es requerido";
        else
            MensajeErrorEstado = null;

        OnPropertyChanged(nameof(MostrarErrorEstado));
    }

    private void ValidarFecha(DateTime fecha)
    {
        if (fecha.Year < 2024)
            MensajeErrorFecha = "La fecha no puede ser anterior al año 2024";
        else if (fecha > DateTime.Now)
            MensajeErrorFecha = "La fecha del gasto no puede ser mayor a la fecha actual";
        else
            MensajeErrorFecha = null;

        OnPropertyChanged(nameof(MostrarErrorFecha));
    }

    private void ValidarTarjeta(TarjetaGastoDto? tarjeta)
    {
        if (tarjeta == null)
            MensajeErrorTarjeta = "Debe seleccionar una tarjeta";
        else
            MensajeErrorTarjeta = null;

        OnPropertyChanged(nameof(MostrarErrorTarjeta));
    }
    #endregion

    #region Predicción ML
    private void DispararPrediccionML(string? value)
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();

        var cancellationToken = _cts.Token;

        _ = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(500, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    return;

                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await PredecirCategoriaAsync(value);
                });
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await Shell.Current.CurrentPage.DisplayAlertAsync("Error", ex.Message, "OK");
                });
            }
        }, cancellationToken);
    }

    private async Task PredecirCategoriaAsync(string? descripcion)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(descripcion) || descripcion.Length <= 3)
            {
                CategoriaPredicha = null;
                ListaCategoriasPredichas?.Clear();
                return;
            }

            var prediccion = await _prediccionCategoriaServicio!.PredecirAsync(descripcion);

            if (prediccion == null)
                return;

            CategoriaPredicha = prediccion;

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
            await Shell.Current.CurrentPage.DisplayAlertAsync(
                "Error",
                $"Error al predecir categoría: {ex.Message}",
                "OK");
        }
    }
    #endregion

    #region Actualizar Gasto Command
    [RelayCommand]
    private async Task ActualizarGasto()
    {
        try
        {
            _cts?.Cancel();

            // Validar todos los campos
            ValidarFecha(FechaSeleccionada);
            ValidarMonto(MontoSeleccionado);
            ValidarDescripcion(DescripcionSeleccionada);
            ValidacionCategoria(CategoriaPredicha);
            ValidarComercio(ComercioSeleccionado);
            ValidarEstado(EstadoSeleccionado);
            ValidarTarjeta(TarjetaSeleccionada);

            bool hayErrores = MostrarErrorFecha || MostrarErrorMonto ||
                            MostrarErrorDescripcion || MostrarErrorCategoria ||
                            MostrarErrorComercio || MostrarErrorEstado ||
                            MostrarErrorTarjeta;

            if (hayErrores)
                return;

            var command = new ActualizarGastoCommand
            {
                IdCommand = IdSeleccionado!.Value,
                MontoCommand = MontoSeleccionado!.Value,
                FechaCommand = FechaSeleccionada,
                TarjetaIdCommand = TarjetaSeleccionada!.Id,
                DescripcionCommand = DescripcionSeleccionada,
                CategoriaCommand = CategoriaPredicha?.CategoriaPrincipal,
                ComercioCommand = ComercioSeleccionado,
                EstadoCommand = EstadoSeleccionado ?? "Activo",
                NombreImagenCommand = $"icono_{CategoriaPredicha?.CategoriaPrincipal?.ToLower()}.png"
            };

            var resultado = await _mediator!.Send(command);

            if (!resultado.EsValido)
            {
                MostrarErroresDelDominio(resultado);
                return;
            }

            await Shell.Current.CurrentPage.DisplayAlertAsync(
                "Éxito",
                "El gasto se actualizó correctamente",
                "OK");

            LimpiarCampos();
            LimpiarErrores();
            PopupVisible = false;

            GastoActualizado?.Invoke();
        }
        catch (Exception ex)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync(
                "Error",
                $"Error al actualizar el gasto: {ex.Message}",
                "OK");
        }
    }

    private void MostrarErroresDelDominio(ResultadosValidacion resultado)
    {
        MensajeErrorMonto = resultado.Errores.ContainsKey("Monto")
            ? resultado.Errores["Monto"] : MensajeErrorMonto;
        MensajeErrorDescripcion = resultado.Errores.ContainsKey("Descripcion")
            ? resultado.Errores["Descripcion"] : MensajeErrorDescripcion;
        MensajeErrorTarjeta = resultado.Errores.ContainsKey("TarjetaSeleccionada")
            ? resultado.Errores["TarjetaSeleccionada"] : MensajeErrorTarjeta;
        MensajeErrorEstado = resultado.Errores.ContainsKey("Estado")
            ? resultado.Errores["Estado"] : MensajeErrorEstado;
        MensajeErrorFecha = resultado.Errores.ContainsKey("Fecha")
            ? resultado.Errores["Fecha"] : MensajeErrorFecha;
        MensajeErrorCategoria = resultado.Errores.ContainsKey("Categoria")
            ? resultado.Errores["Categoria"] : MensajeErrorCategoria;
        MensajeErrorComercio = resultado.Errores.ContainsKey("Comercio")
            ? resultado.Errores["Comercio"] : MensajeErrorComercio;

        NotificarCambiosEnErrores();
    }

    private void NotificarCambiosEnErrores()
    {
        OnPropertyChanged(nameof(MostrarErrorMonto));
        OnPropertyChanged(nameof(MostrarErrorDescripcion));
        OnPropertyChanged(nameof(MostrarErrorTarjeta));
        OnPropertyChanged(nameof(MostrarErrorEstado));
        OnPropertyChanged(nameof(MostrarErrorFecha));
        OnPropertyChanged(nameof(MostrarErrorCategoria));
        OnPropertyChanged(nameof(MostrarErrorComercio));
    }
    #endregion

    #region Cancelar
    [RelayCommand]
    private void Cancelar()
    {
        LimpiarCampos();
        LimpiarErrores();
        PopupVisible = false;
    }
    #endregion

    #region Limpiar
    private void LimpiarCampos()
    {
        IdSeleccionado = null;
        MontoSeleccionado = null;
        DescripcionSeleccionada = null;
        ComercioSeleccionado = null;
        EstadoSeleccionado = null;
        FechaSeleccionada = DateTime.Now;
        TarjetaSeleccionada = null;
        CategoriaPredicha = null;
        ListaCategoriasPredichas?.Clear();
    }

    private void LimpiarErrores()
    {
        MensajeErrorMonto = null;
        MensajeErrorDescripcion = null;
        MensajeErrorTarjeta = null;
        MensajeErrorEstado = null;
        MensajeErrorFecha = null;
        MensajeErrorCategoria = null;
        MensajeErrorComercio = null;

        NotificarCambiosEnErrores();
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