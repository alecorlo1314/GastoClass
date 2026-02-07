using CommunityToolkit.Mvvm.ComponentModel;
using GastoClass.Aplicacion.Dashboard.Consultas.ObtenerGastosPorCategoria;
using GastoClass.Aplicacion.Dashboard.Consultas.ObtenerResumenMensual;
using GastoClass.Aplicacion.Dashboard.Consultas.ObtenerUltimosGastos;
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
    /// <summary>
    /// Se usa para enviar comandos y solicitudes al Mediator
    /// </summary>
    private readonly IMediator _mediator;

    #endregion

    #region Propiedades UI
    [ObservableProperty] private decimal totalGastoEsteMes;
    [ObservableProperty] private int totalTransaccionesEsteMes;
    [ObservableProperty] private string? mensajeCantidadTransacciones;

    #endregion

    #region Propiedades Listas
    [ObservableProperty]
    private ObservableCollection<GastoPorCategoriaDto> gastosPorCategoria = new();

    [ObservableProperty]
    private ObservableCollection<GastoUltimosTresDto> ultimosCincoMovimientos = new();

    #endregion

    #region Constructor

    /// <summary>
    /// Constructor del ViewModel
    /// Inicializa servicios, timer y carga datos iniciales del dashboard
    /// </summary>
    public DashboardViewModel(IMediator mediator)
    {
        // Inyección de dependencias
        _mediator = mediator;

        // Cargar Dashboard
        _ = CargarDashboard();
    }

    #endregion

    #region Cargar Dashboard
    private async Task CargarDashboard()
    {
        var resumen = await _mediator.Send(
            new ObtenerResumenMensualConsulta(DateTime.Now.Month, DateTime.Now.Year));

        TotalGastoEsteMes = resumen.TotalGastado;
        TotalTransaccionesEsteMes = resumen.CantidadTransacciones;
        MensajeCantidadTransacciones =
            $"Basado en {TotalTransaccionesEsteMes} transacciones del mes.";

        var categorias = await _mediator.Send(
            new ObtenerGastosPorCategoriaConsulta(DateTime.Now.Month, DateTime.Now.Year));

        GastosPorCategoria =
            new ObservableCollection<GastoPorCategoriaDto>(categorias!);

        var ultimos = await _mediator.Send(
            new ObtenerUltimosTresGastosConsulta());

        UltimosCincoMovimientos =
            new ObservableCollection<GastoUltimosTresDto>(ultimos);
    }

    #endregion

    #region Mostrar Mensaje Cantidad Transacciones
    /// <summary>
    /// Genera mensaje descriptivo basado en la cantidad de transacciones
    /// </summary>
    private void MostrarMensajeCantidadTransacciones()
    {
        if (TotalTransaccionesEsteMes == 0)
        {
            MensajeCantidadTransacciones = "No se han registrado transacciones este mes.";
        }
        if (TotalTransaccionesEsteMes == 1)
        {
            MensajeCantidadTransacciones = "Basado en 1 transaccion de este mes.";
        }
        if (TotalTransaccionesEsteMes > 1)
        {
            MensajeCantidadTransacciones = $"Basado en {TotalTransaccionesEsteMes} transacciones de este mes.";
        }
    }

    #endregion
}