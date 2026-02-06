using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GastoClass.Aplicacion.HistorialGasto.Consultas;
using GastoClass.Aplicacion.HistorialGasto.DTOs;
using MediatR;
using System.Collections.ObjectModel;

namespace GastoClass.Presentacion.ViewModel;

/// <summary>
/// ViewModel responsable ÚNICAMENTE del listado y búsqueda de gastos
/// </summary>
public partial class HistorialGastosViewModel : ObservableObject
{
    #region Dependencias
    private readonly IMediator _mediator;

    #endregion

    #region Viewmodels 
    public ActualizarGastoViewModel ActualizarGastoVM { get; }
    public EliminarGastoViewModel EliminarGastoVM { get; }

    #endregion

    #region Colecciones Observables
    [ObservableProperty]
    private ObservableCollection<GastoHistorialDto>? listaGastos = new();

    [ObservableProperty]
    private ObservableCollection<GastoHistorialDto>? listaGastosFiltrados = new();
    #endregion

    #region Propiedades de Estado UI
    [ObservableProperty]
    private bool isBusy;
    #endregion

    #region Propiedades de Búsqueda
    [ObservableProperty]
    private string? textoBusqueda;
    #endregion

    #region Constructor
    public HistorialGastosViewModel(
        IMediator mediator,
        ActualizarGastoViewModel actualizarGastoVM,
        EliminarGastoViewModel eliminarGastoVM)
    {
        _mediator = mediator;
        ActualizarGastoVM = actualizarGastoVM;
        EliminarGastoVM = eliminarGastoVM;

        // Suscribirse a eventos de los otros ViewModels
        ActualizarGastoVM.GastoActualizado += OnGastoActualizado;
        EliminarGastoVM.GastoEliminado += OnGastoEliminado;

        // Cargar datos iniciales
        _ = CargarListaMovimientos();
    }
    #endregion

    #region Carga de Datos
    public async Task CargarListaMovimientos()
    {
        try
        {
            IsBusy = true;

            var consulta = await _mediator.Send(new ObtenerGastosConsulta());

            if (consulta == null)
                return;

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                ListaGastos?.Clear();

                foreach (var gasto in consulta)
                    ListaGastos?.Add(gasto);

                ListaGastosFiltrados = new ObservableCollection<GastoHistorialDto>(ListaGastos!);
            });
        }
        catch (Exception ex)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync(
                "Error",
                $"Error al cargar gastos: {ex.Message}",
                "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
    #endregion

    #region Búsqueda y Filtrado
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

    #region Event Handlers
    private async void OnGastoActualizado()
    {
        await CargarListaMovimientos();
    }

    private async void OnGastoEliminado()
    {
        await CargarListaMovimientos();
    }
    #endregion

    #region Comando Abrir Actualizacion
    [RelayCommand]
    private void AbrirEdicion(GastoHistorialDto gasto)
    {
        // Delegar al ViewModel de actualización
        ActualizarGastoVM.CargarGastoParaEdicion(gasto);
    }

    #endregion

    #region Comando Abrir Popup Eliminacion
    [RelayCommand]
    private void AbrirEliminacion(GastoHistorialDto gasto)
    {
        // Delegar al ViewModel de eliminación
        EliminarGastoVM.AbrirConfirmacion(gasto);
    }

    [RelayCommand]
    private void Eliminar()
    {
        IAsyncRelayCommand eliminarCommand = EliminarGastoVM.EliminarCommand;
    }

    #endregion

    #region Cleanup
    public void Dispose()
    {
        ActualizarGastoVM.GastoActualizado -= OnGastoActualizado;
        EliminarGastoVM.GastoEliminado -= OnGastoEliminado;
    }
    #endregion
}