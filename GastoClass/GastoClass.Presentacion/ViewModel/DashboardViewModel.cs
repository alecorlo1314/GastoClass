using CommunityToolkit.Mvvm.ComponentModel;
using GastoClass.GastoClass.Aplicacion.Dashboard.Consultas.GastosPorCategoria;
using GastoClass.GastoClass.Aplicacion.Dashboard.Consultas.ResumenMes;
using GastoClass.GastoClass.Aplicacion.Dashboard.Consultas.UltimosCincoGastos;
using GastoClass.GastoClass.Aplicacion.Dashboard.DTOs;
using MediatR;
using System.Collections.ObjectModel;

namespace GastoClass.Presentacion.ViewModel;

/// <summary>
/// ViewModel principal del Dashboard
/// Gestiona la visualización de gastos, predicciones ML y operaciones CRUD
/// </summary>
public partial class DashboardViewModel : ObservableObject
{
    #region Inyección de Dependencias
    private readonly IMediator? _mediator;


    #endregion

    public AgregarGastoViewModel AgregarGastoVM { get; }

    #region Propiedades para Control de Predicción ML
    // Token de cancelación para predicciones en curso
    private CancellationTokenSource? _cts;

    #endregion

    #region Propiedades Observables

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
    [ObservableProperty]
    private ObservableCollection<GastoPorCategoriaDto> gastoPorCategoriasMes = new();
    [ObservableProperty]
    private ObservableCollection<UltimoCincoGastosDto>? ultimosCincoMovimientos = new();

    #endregion

    #region Constructor
    public DashboardViewModel(IMediator mediator, AgregarGastoViewModel agregarGastoVM)
    {
        _mediator = mediator;

        _ = InicializarDatosAsync();
        // Cargar datos iniciales del dashboard
        AgregarGastoVM = agregarGastoVM;

        agregarGastoVM.GastoAgregado += OnGastoAgregado;
    }

    #endregion

    #region Event Handlers
    private async void OnGastoAgregado()
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await RefrescarDashboardAsync();
        });
    }

    #endregion

    #region Métodos Públicos
    public async Task RefrescarDashboardAsync()
    {
        await Task.WhenAll(
            CargarTransaccionesGastoTotal(),
            CargarGastosPorCategoria(),
            ObtenerUltimos5GastosAsync()
        );
    }
    #endregion

    #region Inicializar Datos
    public async Task InicializarDatosAsync()
    {
        try
        {
            await CargarTransaccionesGastoTotal();
            await CargarGastosPorCategoria();
            await ObtenerUltimos5GastosAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    #endregion

    #region Métodos de Carga de Datos 
    private async Task CargarTransaccionesGastoTotal()
    {
        try
        {
            var consulta = await _mediator!.Send(new ObtenerResumenMesConsulta(DateTime.Now.Month, DateTime.Now.Year));

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                GastoTotalMes = consulta.TotalGastado;
                CantidadTransacciones = consulta.CantidadTransacciones;
                MostrarMensajeCantidadTransacciones();
            });
        }
        catch (Exception ex)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync("Error", ex.Message, "OK");
            });
        }
    }

    private void MostrarMensajeCantidadTransacciones()
    {
        MensajeCantidadTransacciones = CantidadTransacciones switch
        {
            0 => "No se han registrado transacciones este mes.",
            1 => "Basado en 1 transacción de este mes.",
            _ => $"Basado en {CantidadTransacciones} transacciones de este mes."
        };
    }

    private async Task CargarGastosPorCategoria()
    {
        try
        {
            var gastosPorCategoria = await _mediator!.Send(
                new ObtenerGastosPorCategoriaConsulta(DateTime.Now.Month, DateTime.Now.Year));

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                GastoPorCategoriasMes.Clear();

                if (gastosPorCategoria != null)
                {
                    foreach (var gasto in gastosPorCategoria)
                    {
                        GastoPorCategoriasMes.Add(gasto);
                    }
                }
            });
        }
        catch (Exception ex)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync("Error", ex.Message, "OK");
            });
        }
    }


    private async Task ObtenerUltimos5GastosAsync()
    {
        try
        {
            var ultimosCincoGastos = await _mediator!.Send(new ObtenerUltimosTresGastosConsulta());

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                UltimosCincoMovimientos?.Clear();

                if (ultimosCincoGastos != null)
                {
                    UltimosCincoMovimientos = new ObservableCollection<UltimoCincoGastosDto>(ultimosCincoGastos);
                }
            });
        }
        catch (Exception ex)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync("Error", ex.Message, "OK");
            });
        }
    }
    #endregion

    #region IDisposable
    public void Dispose()
    {
        if (AgregarGastoVM != null)
        {
            AgregarGastoVM.GastoAgregado -= OnGastoAgregado;
        }
    }
    #endregion
}
