using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GastoClass.Aplicacion.CasosUso;
using GastoClass.Dominio.Model;
using GastoClass.GastoClass.Aplicacion.HistorialGasto.Consultas;
using GastoClass.GastoClass.Aplicacion.HistorialGasto.DTOs;
using MediatR;
using System.Collections.ObjectModel;

namespace GastoClass.Presentacion.ViewModel;
public partial class HistorialGastosViewModel : ObservableObject
{
    #region Dependencias
    private readonly ServicioGastos _gastoService;
    private readonly PredictionApiService _servicioPrediccionCategoriaML;

    IMediator _mediator;

    #endregion

    #region Colecciones observables
    [ObservableProperty]
    private ObservableCollection<GastoHistorialDto>? listaGastos = new();
    [ObservableProperty]
    private ObservableCollection<GastoHistorialDto>? listaGastosFiltrados = new();
    [ObservableProperty]
    private ObservableCollection<CategoriasRecomendadas> listaCategoriaFinal = new();
    [ObservableProperty]
    private ObservableCollection<ResultadoPrediccion> listaCategoriasSugeridasML = new();

    #endregion

    #region Properidades de Estado UI
    [ObservableProperty]
    private bool isBusy;
    [ObservableProperty]
    private bool actualizadoConExito;
    [ObservableProperty]
    private bool mostarPopupEliminacion;
    [ObservableProperty]
    private string? mensajeEliminacionGasto;
    #endregion

    #region Propiedades auxiliares
    [ObservableProperty]
    private Gasto? eliminarGastoObjeto;
    #endregion

    #region Propiedades de búsqueda
    [ObservableProperty]
    private string? textoBusqueda;

    #endregion

    #region Propiedades de edición
    [ObservableProperty] private int idSeleccionado;
    [ObservableProperty] private decimal montoSeleccionado;
    [ObservableProperty] private string? descripcionSeleccionada;
    [ObservableProperty] private int idTarjetaSeleccionada;
    [ObservableProperty] private string? categoriaSeleccionada;
    [ObservableProperty] private CategoriasRecomendadas? categoriaSeleccionadaObjeto;
    [ObservableProperty] private DateTime fechaSeleccionada;

    #endregion

    #region Control de estado interno
    private string? _descripcionOriginal;
    private bool _modoEdicion;
    private bool _descripcionCambiadaPorUsuario;
    private CancellationTokenSource? _cts;
    #endregion

    #region Constructor
    public HistorialGastosViewModel(IMediator mediator,
        ServicioGastos gastoService,
        PredictionApiService servicioPrediccionCategoriaML)
    {
        _gastoService = gastoService;
        _servicioPrediccionCategoriaML = servicioPrediccionCategoriaML;
        _mediator = mediator;

        _ = CargarListaMovimientos();
    }

    #endregion

    #region Carga de datos
    private async Task CargarListaMovimientos()
    {
        try
        {
            IsBusy = true;
            var consulta = await _mediator.Send(new ObtenerGastosConsulta());
            if(consulta == null)
                return;

            ListaGastos?.Clear();

            foreach (var gasto in consulta)
                ListaGastos?.Add(gasto);

            ListaGastosFiltrados = new ObservableCollection<GastoHistorialDto>(ListaGastos!);
        }
        catch (Exception)
        {
            // Manejo de errores (logging o notificación)
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion

    #region Búsqueda y filtrado
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
                ListaGastosFiltrados = new ObservableCollection<GastoHistorialDto>(ListaGastos!);
                return;
            }

            textoBusqueda = textoBusqueda.ToLower();

            var resultado = ListaGastos?.Where(g =>
                g.Descripcion!.ToLower().Contains(textoBusqueda) ||
                g.Id!.ToString().Contains(textoBusqueda) ||
                g.Categoria!.ToLower().Contains(textoBusqueda) ||
                g.Monto.ToString().Contains(textoBusqueda));

            ListaGastosFiltrados = new ObservableCollection<GastoHistorialDto>(resultado!);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    #endregion

    #region Edición de gastos
    [RelayCommand]
    private void AbrirPopupEdicionGasto(Gasto gasto)
    {
        //Se activa el modo edicion
        _modoEdicion = true;
        //Guardamos la descipcion original
        _descripcionOriginal = gasto.Descripcion;
        //Aun el usuario no ha editado la descripcion
        _descripcionCambiadaPorUsuario = false;

        //Guardamos los datos del Objeto Gasto para que se muestren en el popup
        IdSeleccionado = gasto.Id;
        MontoSeleccionado = gasto.Monto;
        DescripcionSeleccionada = gasto.Descripcion;
        IdTarjetaSeleccionada = gasto.TarjetaId;
        FechaSeleccionada = gasto.Fecha;
        CategoriaSeleccionada = gasto.Categoria;

        //Limpiamos las lista de sugerencias ML y la lista definitiva
        CategoriaSeleccionadaObjeto = null;
        ListaCategoriasSugeridasML?.Clear();
        ListaCategoriaFinal?.Clear();
    }

    /// <summary>
    /// Se ejecuta cuando cambia la descripción del gasto en edición
    /// </summary>
    partial void OnDescripcionSeleccionadaChanged(string? value)
    {
        //Si el modo edicion esta en false pasar
        if (!_modoEdicion)
            return;
        //Si la descripcion es invalida no se pasa
        if (!EsDescripcionValida(value))
            return;
        //Si el valor es igual al valor original, se limpia la lista de categorias sugeridas
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

    #region Eliminacion de gasto
    [RelayCommand]
    private async Task AbrirPopupEliminarGasto(Gasto eliminarGasto)
    {
        try
        {
            //mostrar popup para la eliminacion del gasto
             MensajeEliminacionGasto = MetodoMensajeEliminacionGasto(eliminarGasto.Descripcion);
            //cancelar o borra
            MostarPopupEliminacion = true;
            //Guardar temporalmente el gasto a eliminar
            EliminarGastoObjeto = eliminarGasto;
        }
        catch (Exception ex)
        {
            Shell.Current?.CurrentPage.DisplayAlertAsync("Error",$"Se produjo un error al intentar eliminar el gasto: {ex.Message}","Ok");
        }
    }

    [RelayCommand]
    private async Task EliminarGasto()
    {
        try
        {
            if (EliminarGastoObjeto == null) return;
            //si es borrar se manda el gastos al servicio de gastos
            var resultado = await _gastoService.EliminarGastoAsync(EliminarGastoObjeto);
            //si se guardo correctamente devolvera 1 y mostrar "El gastos de elimino correctamente y se cierra el popup de eliminacion al darle ok"
            if (resultado == 1)
            {
                Shell.Current?.CurrentPage.DisplayAlertAsync("Informacion", "Gasto eliminado correctamente!", "OK");
                MostarPopupEliminacion = false;
                IsBusy = true;
                _ = CargarListaMovimientos();
                EliminarGastoObjeto = null;
            }
            else
            {
                //si no se guardo devolvera un 0 y mostrar un popup segun sea el caso
                Shell.Current?.CurrentPage.DisplayAlertAsync("Informacion", "No se pudo eliminar el gasto", "OK");
            }

            //si se guardo, al finalizar se cargara la nueva lista sin el gasto que se elimino
        }catch (Exception ex)
        {
            Shell.Current?.CurrentPage.DisplayAlertAsync("Error", $"Se produjo un error al intentar eliminar el gasto: {ex.Message}", "Ok");
        }
        finally
        {
            IsBusy = false;
            MostarPopupEliminacion = false;
        }
    }

    [RelayCommand]
    private async Task CancelarEliminarGasto()
    {
        try
        {
            //cerra popup
            MostarPopupEliminacion = false;
            //inicializar el objeto gasto a eliminar
            EliminarGastoObjeto = null;
        }catch(Exception ex)
        {
            Shell.Current?.CurrentPage.DisplayAlertAsync("Error", $"Se produjo un error al intentar cancelar el gasto: {ex.Message}", "Ok");
        }
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

            // Actualiza el STRING (para la BD)
            CategoriaSeleccionada = prediction.Categoria;

            // Limpiar y llenar la lista
            ListaCategoriaFinal?.Clear();

            //Pasar los scores de la prediccion a la lista final
            foreach (var (key,value) in prediction.scoreDict)
            {
                ListaCategoriaFinal?.Add(new CategoriasRecomendadas
                {
                    DescripcionCategoriaRecomendada = key,
                    ScoreCategoriaRecomendada = value
                });
            }
            // ordernar lista de puntos, desdendentemente por puntos
            var ordenada = ListaCategoriaFinal
                ?.OrderByDescending(s => s.ScoreCategoriaRecomendada)
                .ToList();
            //Actualizar la lista final con los datos ordenados por scores
            ListaCategoriaFinal = new ObservableCollection<CategoriasRecomendadas>(ordenada!);

            CategoriaSeleccionadaObjeto = ListaCategoriaFinal?.FirstOrDefault();
        }
        catch (Exception ex)
        {
            await Shell.Current.CurrentPage
                .DisplayAlertAsync("Error ML", ex.Message, "OK");
        }
    }

    #endregion

    #region Actualizar Gasto
    [RelayCommand]
    private async Task ActualizarGasto()
    {
        try
        {
            //Validaciones
            if (string.IsNullOrWhiteSpace(IdSeleccionado.ToString()) || !EsDescripcionValida(DescripcionSeleccionada)
                || EsMontoValido(MontoSeleccionado) || CategoriaSeleccionada == null) return;
            //inicializar objeto Gasto
            Gasto gasto = new Gasto
            {
                Id = IdSeleccionado,
                Descripcion = DescripcionSeleccionada,
                Monto = MontoSeleccionado,
                TarjetaId = IdTarjetaSeleccionada,
                Fecha = FechaSeleccionada,
                Categoria = CategoriaSeleccionada
                
            };

            IsBusy = true;
            //Guardar en la BD
            var resultado = await _gastoService.GuardarGastoAsync(gasto);

            if (resultado == 1)
            {
                Shell.Current.CurrentPage?.DisplayAlertAsync("Prueba", $"Se guardo {gasto.Descripcion} con categoria {gasto.Categoria}", "Ok");
                //Cerrar Popup
                ActualizadoConExito = false;
                // Carga inicial del historial de gastos
                _ = CargarListaMovimientos();
            }    
        }
        catch (Exception ex)
        {
            Shell.Current.CurrentPage?.DisplayAlertAsync("Prueba", $"Se produjo un error al intentar actualizar el gastos: {ex.Message}", "Ok");
        }
        finally
        {
            IsBusy = false;
        }
    }
    #endregion

    #region Seleccion Usuario 
    /// <summary>
    /// Se ejecuta cuando el usuario selecciona del ComboBox manualmente
    /// </summary>
    partial void OnCategoriaSeleccionadaObjetoChanged(CategoriasRecomendadas? value)
    {
        if (value == null) return;

        // Actualiza el string que se guarda en la BD
        CategoriaSeleccionada = value.DescripcionCategoriaRecomendada;
    }
    #endregion

    #region Validaciones

    /// <summary>
    /// Valida que la descripción tenga un formato mínimo válido
    /// </summary>
    private bool EsDescripcionValida(string? texto)
        => !string.IsNullOrWhiteSpace(texto) && texto.Length >= 3;
    private bool EsMontoValido(decimal monto)
        => !string.IsNullOrWhiteSpace(monto.ToString()) && monto < 0 &&
        decimal.TryParse(monto.ToString(), out _);
    #endregion

    #region Mostrar Mensajes
    private string MetodoMensajeEliminacionGasto(string? descripcion)
    {
        return $"Esta accion no se puede deshacer. Eliminara permanentemente el registro de gasto de '{descripcion}'";
    }
    #endregion 
}
