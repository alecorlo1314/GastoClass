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
    public AgregarGastoViewModel AgregarGastoVM { get; }

    #endregion

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

        // Inyectar el ViewModel de Agregar Gasto
        AgregarGastoVM = agregarGastoVM;
        //Notificar al ViewModel de Agregar Gasto el evento GastoAgregado
        agregarGastoVM.GastoAgregado += OnGastoAgregado;
    }

    #endregion

    #region Event Handlers
    private void OnGastoAgregado()
    {
        _ = RefrescarDashboardAsync();
    }

    #endregion

    #region Métodos Públicos
    public async Task RefrescarDashboardAsync()
    {
        await Task.WhenAll(
            CargarResumenMesAsync(),
            CargarGastosPorCategoria(),
            ObtenerUltimos5GastosAsync()
        );
    }
    #endregion

    #region Inicializar Datos
    public async Task InicializarDatosAsync()
    {
        await Task.WhenAll(
            CargarResumenMesAsync(),
            CargarGastosPorCategoria(),
            ObtenerUltimos5GastosAsync());
    }

    #endregion

    #region Metodo cargar el resumen del mes
    private async Task CargarResumenMesAsync()
    {
        var consulta = await _mediator!.Send(new ObtenerResumenMesConsulta(DateTime.Now.Month, DateTime.Now.Year));

        GastoTotalMes = consulta.TotalGastado;
        CantidadTransacciones = consulta.CantidadTransacciones;
        MostrarMensajeCantidadTransacciones();
    }

    #endregion

    #region Metodo para mostrar el mensaje de cantidad de transacciones
    private void MostrarMensajeCantidadTransacciones()
    {
        MensajeCantidadTransacciones = CantidadTransacciones switch
        {
            0 => "No se han registrado transacciones este mes.",
            1 => "Basado en 1 transacción de este mes.",
            _ => $"Basado en {CantidadTransacciones} transacciones de este mes."
        };
    }

    #endregion

    #region Metodo para cargar los gastos por categoria
    private async Task CargarGastosPorCategoria()
    {
        try
        {
            var gastosPorCategoria = await _mediator!.Send(
                new ObtenerGastosPorCategoriaConsulta(DateTime.Now.Month, DateTime.Now.Year));

            GastoPorCategoriasMes.Clear();

            if (gastosPorCategoria != null)
            {
                foreach (var gasto in gastosPorCategoria)
                {
                    GastoPorCategoriasMes.Add(gasto);
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    #region Metodo para cargar los ultimos 5 gastos
    private async Task ObtenerUltimos5GastosAsync()
    {
        var resultado = await _mediator!
            .Send(new ObtenerUltimosCincoGastosConsulta());

        if (!resultado.EsValido)
        {
            return;
        }

        UltimosCincoMovimientos =
            new ObservableCollection<UltimoCincoGastosDto>(resultado.Datos!);
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
