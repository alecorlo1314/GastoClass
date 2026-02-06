using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GastoClass.Aplicacion.Interfaces;
using GastoClass.GastoClass.Aplicacion.Dashboard.Consultas.TarjetasCreditoComboBox;
using GastoClass.GastoClass.Aplicacion.Dashboard.DTOs;
using GastoClass.GastoClass.Aplicacion.Gasto.Commands.AgregarGasto;
using GastoClass.GastoClass.Aplicacion.Servicios.DTOs;
using GastoClass.GastoClass.Presentacion.Mensajes;
using MediatR;
using System.Collections.ObjectModel;

namespace GastoClass.Presentacion.ViewModel;

public partial class AgregarGastoViewModel : ObservableObject, IDisposable
{
    #region Inyeccion de Dependencias
    private readonly IMediator? _mediator;
    private readonly IPrediccionCategoriaServicio? _prediccionCategoriaServicio;
    #endregion

    #region Propiedades del Formulario
    [ObservableProperty] private string? descripcion;
    [ObservableProperty] private decimal? monto;
    [ObservableProperty] private string? comercio;
    [ObservableProperty] private DateTime fecha = DateTime.Now;
    [ObservableProperty] private TarjetaCreditoComboBoxDto? tarjetaSeleccionada;
    [ObservableProperty] private string? nombreTarjeta;
    [ObservableProperty] private string? numeroTarjeta;
    [ObservableProperty] private string? estadoSeleccionado = "Pendiente";
    #endregion

    #region Listas Observables
    [ObservableProperty] private CategoriaPredichaDto? categoriaPredicha;
    [ObservableProperty] private ObservableCollection<CategoriaPredichaDto>? listaCategoriasPredichas = new();
    [ObservableProperty] private ObservableCollection<TarjetaCreditoComboBoxDto>? listaTarjetasCredito = new();
    #endregion

    #region Propiedades de procesos
    // Token de cancelación para predicciones en curso
    private CancellationTokenSource? _cts;

    [ObservableProperty] private bool estaOcupado = false;
    #endregion

    #region Mensajes de Error Formulario
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
    public AgregarGastoViewModel(IMediator mediator, IPrediccionCategoriaServicio prediccionCategoriaServicio)
    {
        _mediator = mediator;
        _prediccionCategoriaServicio = prediccionCategoriaServicio;
    }
    #endregion

    #region Inicializar Datos 
    public async Task InicializarDatosAsync()
    {
        await CargarDatosTarjetasComboBoxAsync();
    }

    private async Task CargarDatosTarjetasComboBoxAsync()
    {
        //Se necesita icono, nombre y ultimos 4 digitos
        var tarjetas = await _mediator!.Send(new ObtenerTarjetasCreditoComboBoxConsulta());
        ListaTarjetasCredito = new ObservableCollection<TarjetaCreditoComboBoxDto>(tarjetas);
    }

    #endregion

    #region Validaciones Descripcion
    private void ValidarDescripcion(string? descripcion)
    {
        if (string.IsNullOrWhiteSpace(descripcion) || string.IsNullOrEmpty(descripcion)) MensajeErrorDescripcion = "Descripcion es requerida";
        else if (descripcion.Length > 200) MensajeErrorDescripcion = "No puede ser mayor a 200 caracteres";
        else if (descripcion!.Length < 3) MensajeErrorDescripcion = "No puede ser menor a 3 caracteres";
        else MensajeErrorDescripcion = null;

        OnPropertyChanged(nameof(MostrarErrorDescripcion));
    }

    #endregion

    #region Validaciones Monto
    partial void OnMontoChanged(decimal? value) => ValidarMonto(value);

    private void ValidarMonto(decimal? monto)
    {
        if (string.IsNullOrWhiteSpace(monto.ToString()) || string.IsNullOrEmpty(monto.ToString())) MensajeErrorMonto = "Monto es requerido";
        else if (monto < 0) MensajeErrorMonto = "No puede ser menor a 0";
        else MensajeErrorMonto = null;

        OnPropertyChanged(nameof(MostrarErrorMonto));
    }

    #endregion

    #region Validaciones Categoria
    partial void OnCategoriaPredichaChanged(CategoriaPredichaDto? value) => ValidacionCategoria(value);
    private void ValidacionCategoria(CategoriaPredichaDto? categoria)
    {
        if (categoria == null) MensajeErrorCategoria = "Categoria es requerida";
        else MensajeErrorCategoria = null;

        OnPropertyChanged(nameof(MostrarErrorCategoria));
    }
    #endregion

    #region Valdaciones Comercio
    partial void OnComercioChanged(string? value) => ValidarComercio(value);
    private void ValidarComercio(string? comercio)
    {
        if (string.IsNullOrWhiteSpace(comercio) || string.IsNullOrEmpty(comercio)) MensajeErrorComercio = "Comercio es requerido";
        else if (comercio!.Length > 100) MensajeErrorComercio = "No puede ser mayor a 100 caracteres";
        else if (comercio.Length < 3) MensajeErrorComercio = "No puede ser menor a 3 caracteres";
        else MensajeErrorComercio = null;

        OnPropertyChanged(nameof(MostrarErrorComercio));
    }
    #endregion

    #region Validaciones Fecha
    partial void OnFechaChanged(DateTime value) => ValidarFecha(value);
    private void ValidarFecha(DateTime fecha)
    {
        if (fecha!.Year < 2024) MensajeErrorFecha = "No puede ser menor al 2024";
        else if (fecha > DateTime.Now) MensajeErrorFecha = "No puede ser mayor a la fecha actual";
        else MensajeErrorFecha = null;

        OnPropertyChanged(nameof(MostrarErrorFecha));
    }

    #endregion

    #region Validaciones Tarjeta
    partial void OnTarjetaSeleccionadaChanged(TarjetaCreditoComboBoxDto? value) => ValidarTarjeta(value);
    private void ValidarTarjeta(TarjetaCreditoComboBoxDto? tarjeta)
    {
        if (tarjeta == null) MensajeErrorTarjeta = "Tarjeta es requerida";
        else MensajeErrorTarjeta = null;

        OnPropertyChanged(nameof(MostrarErrorTarjeta));
    }
    #endregion

    #region OnDescripcionChanged
    partial void OnDescripcionChanged(string? value)
    {
        ValidarDescripcion(value);
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
            }
        }, cancellationToken);
    }
    #endregion

    #region Validaciones Estado
    partial void OnEstadoSeleccionadoChanged(string? value)
    {
        ValidarEstadoSeleccionado(value);
    }
    private void ValidarEstadoSeleccionado(string? estado)
    {
        if (string.IsNullOrWhiteSpace(estado) || string.IsNullOrEmpty(estado)) MensajeErrorEstado = "Estado es requerido";
        else MensajeErrorEstado = null;

        OnPropertyChanged(nameof(MostrarErrorEstado));
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
        catch (Exception)
        {
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
            EstaOcupado = true;

            //validar antes de guardar
            bool hayErrores = false;


            ValidarFecha(Fecha);
            ValidarMonto(Monto);
            ValidarDescripcion(Descripcion);
            ValidacionCategoria(CategoriaPredicha);
            ValidarComercio(Comercio);
            ValidarEstadoSeleccionado(EstadoSeleccionado);
            ValidarTarjeta(TarjetaSeleccionada);
            hayErrores = MostrarErrorFecha || MostrarErrorMonto || MostrarErrorDescripcion ||
                         MostrarErrorCategoria || MostrarErrorComercio || MostrarErrorEstado ||
                         MostrarErrorTarjeta;

            if(hayErrores)
            {
                //Retornar si hay errores de validación
                return;
            }

            var command = new AgregarGastoCommand
            {
                MontoCommand = Monto!.Value,
                FechaCommand = Fecha,
                TarjetaIdCommand = TarjetaSeleccionada!.Id,
                DescripcionCommand = Descripcion,
                CategoriaCommand = CategoriaPredicha?.CategoriaPrincipal,
                ComercioCommand = Comercio,                 
                EstadoCommand = EstadoSeleccionado, 
                NombreImagenCommand = $"icono_{CategoriaPredicha?.CategoriaPrincipal?.ToLower()}.png"
            };

            var resultado = await _mediator!.Send(command);

            if (resultado.Popup != null)
            {
                await Shell.Current.DisplayAlertAsync(
                    resultado.Popup.Titulo,
                    resultado.Popup.Mensaje,
                    "Aceptar");
                return;
            }
            // Limpiar espacios
            LimpiarCampos();
            LimpiarErrores();

            //Notificar a DashboardViewModel y refrescar datos nuevos
            WeakReferenceMessenger.Default.Send(new GastoAgregadoMessage());
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "Aceptar");
        }
        finally
        {
            EstaOcupado = false;
        }
    }
    #endregion

    #region Limpiar Campos
    private void LimpiarCampos()
    {
        Monto = null;
        Descripcion = null;
        CategoriaPredicha = null;
        EstadoSeleccionado = "Pendiente";
        ListaCategoriasPredichas?.Clear();
    }
    #endregion

    #region Limpiar Errores
    private void LimpiarErrores()
    {
        MensajeErrorMonto = null;
        MensajeErrorDescripcion = null;
        MensajeErrorTarjeta = null;
        MensajeErrorEstado = null;
        MensajeErrorFecha = null;
        MensajeErrorCategoria = null;
        MensajeErrorComercio = null;

        OnPropertyChanged(nameof(MostrarErrorMonto));
        OnPropertyChanged(nameof(MostrarErrorDescripcion));
        OnPropertyChanged(nameof(MostrarErrorTarjeta));
        OnPropertyChanged(nameof(MostrarErrorEstado));
        OnPropertyChanged(nameof(MostrarErrorFecha));
        OnPropertyChanged(nameof(MostrarErrorCategoria));
        OnPropertyChanged(nameof(MostrarErrorComercio));
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