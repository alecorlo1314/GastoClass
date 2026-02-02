using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GastoClass.Aplicacion.Interfaces;
using GastoClass.GastoClass.Aplicacion.Dashboard.Consultas.TarjetasCreditoComboBox;
using GastoClass.GastoClass.Aplicacion.Dashboard.DTOs;
using GastoClass.GastoClass.Aplicacion.Gasto.Commands.AgregarGasto;
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
    [ObservableProperty] private TarjetaCreditoComboBoxDto? tarjetaSeleccionada;
    [ObservableProperty] private string? nombreTarjeta;
    [ObservableProperty] private string? numeroTarjeta;
    #endregion

    #region Listas Observables
    [ObservableProperty] private CategoriaPredichaDto? categoriaPredicha;
    [ObservableProperty] private ObservableCollection<CategoriaPredichaDto>? listaCategoriasPredichas = new();
    [ObservableProperty] private ObservableCollection<TarjetaCreditoComboBoxDto>? listaTarjetasCredito = new();
    #endregion

    #region Propiedades de procesos
    // Token de cancelación para predicciones en curso
    private CancellationTokenSource? _cts;
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

        // Inicializar Datos
        //_ = InicializarDatosAsync();
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

    #region Validaciones Estado
    partial void OnEstadoChanged(string? value) => ValidarEstado(value);
    private void ValidarEstado(string? estado)
    {
        if (string.IsNullOrWhiteSpace(estado) || string.IsNullOrEmpty(estado)) MensajeErrorEstado = "Estado es requerido";
        else MensajeErrorEstado = null;

        OnPropertyChanged(nameof(MostrarErrorEstado));
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

            //validar antes de guardar
            bool hayErrores = false;


            ValidarFecha(Fecha);
            ValidarMonto(Monto);
            ValidarDescripcion(Descripcion);
            ValidacionCategoria(CategoriaPredicha);
            ValidarComercio(Comercio);
            ValidarEstado(Estado);
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
                ComercioCommand = "Comercio",
                EstadoCommand = "Activo",
                NombreImagenCommand = $"icono_{CategoriaPredicha?.CategoriaPrincipal?.ToLower()}.png"
            };

            var resultado = await _mediator!.Send(command);

            if (!resultado.EsValido)
            {
                // Mostrar errores de validación
                MensajeErrorMonto = resultado.Errores.ContainsKey("monto")
                    ? resultado.Errores["monto"] : null;
                MensajeErrorDescripcion = resultado.Errores.ContainsKey("descripcion")
                    ? resultado.Errores["descripcion"] : null;
                MensajeErrorTarjeta = resultado.Errores.ContainsKey("tarjetaId")
                    ? resultado.Errores["tarjetaId"] : null;
                MensajeErrorEstado = resultado.Errores.ContainsKey("estado")
                    ? resultado.Errores["estado"] : null;
                MensajeErrorFecha = resultado.Errores.ContainsKey("fecha")
                    ? resultado.Errores["fecha"] : null;
                MensajeErrorCategoria = resultado.Errores.ContainsKey("categoria")
                    ? resultado.Errores["categoria"] : null;
                MensajeErrorComercio = resultado.Errores.ContainsKey("comercio")
                    ? resultado .Errores["comercio"] : null;

                // Notificar cambios en visibilidad
                OnPropertyChanged(nameof(MostrarErrorMonto));
                OnPropertyChanged(nameof(MostrarErrorDescripcion));
                OnPropertyChanged(nameof(MostrarErrorTarjeta));
                OnPropertyChanged(nameof(MostrarErrorEstado));
                OnPropertyChanged(nameof(MostrarErrorFecha));
                OnPropertyChanged(nameof(MostrarErrorCategoria));
                OnPropertyChanged(nameof(MostrarErrorComercio));

                return;
            }

            // Mostrar mensaje de exito
            await Shell.Current.CurrentPage.DisplayAlertAsync("Exito", "Gasto guardado con exito", "OK");

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
        Fecha = DateTime.Now;
        CategoriaPredicha = null;
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

        OnPropertyChanged(nameof(MostrarErrorMonto));
        OnPropertyChanged(nameof(MostrarErrorDescripcion));
        OnPropertyChanged(nameof(MostrarErrorTarjeta));
        OnPropertyChanged(nameof(MostrarErrorEstado));
        OnPropertyChanged(nameof(MostrarErrorFecha));
        OnPropertyChanged(nameof(MostrarErrorCategoria));
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