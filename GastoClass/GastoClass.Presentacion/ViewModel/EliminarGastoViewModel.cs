using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GastoClass.GastoClass.Aplicacion.Gasto.Commands.EliminarGasto;
using GastoClass.GastoClass.Aplicacion.HistorialGasto.DTOs;
using MediatR;

namespace GastoClass.Presentacion.ViewModel;

/// <summary>
/// ViewModel responsable ÚNICAMENTE de eliminar gastos
/// </summary>
public partial class EliminarGastoViewModel : ObservableObject
{
    #region Inyección de Dependencias
    private readonly IMediator _mediator;
    #endregion

    #region Eventos
    /// <summary>
    /// Evento que se dispara cuando se elimina un gasto exitosamente
    /// </summary>
    public event Action? GastoEliminado;
    #endregion

    #region Propiedades
    [ObservableProperty]
    private bool popupVisible;

    [ObservableProperty]
    private string? mensajeConfirmacion;

    [ObservableProperty]
    private bool procesandoEliminacion;

    private GastoHistorialDto? _gastoAEliminar;
    #endregion

    #region Constructor
    public EliminarGastoViewModel(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion

    #region Abrir Confirmación
    /// <summary>
    /// Abre el popup de confirmación de eliminación
    /// </summary>
    public void AbrirConfirmacion(GastoHistorialDto gasto)
    {
        _gastoAEliminar = gasto;
        MensajeConfirmacion = $"Esta acción no se puede deshacer. " +
            $"Eliminará permanentemente el registro de gasto de '{gasto.Descripcion}'";
        PopupVisible = true;
    }
    #endregion

    #region Eliminar Gasto
    [RelayCommand]
    private async Task Eliminar()
    {
        if (_gastoAEliminar == null)
            return;

        try
        {
            ProcesandoEliminacion = true;

            var command = new EliminarGastoCommand(_gastoAEliminar.Id);

            var resultado = await _mediator.Send(command);

            if (!resultado.EsValido)
            {
                var errores = string.Join(", ", resultado.Errores.Values);
                await Shell.Current.CurrentPage.DisplayAlertAsync(
                    "Error",
                    $"No se pudo eliminar el gasto: {errores}",
                    "OK");
               
                return;
            }

            await Shell.Current.CurrentPage.DisplayAlertAsync(
                "Éxito",
                "El gasto se eliminó correctamente",
                "OK");

            PopupVisible = false;
            _gastoAEliminar = null;

            // Notificar que se eliminó el gasto
            GastoEliminado?.Invoke();
        }
        catch (Exception ex)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync(
                "Error",
                $"Error al eliminar el gasto: {ex.Message}",
                "OK");
        }
        finally
        {
            ProcesandoEliminacion = false;
        }
    }
    #endregion

    #region Cancelar
    [RelayCommand]
    private void Cancelar()
    {
        PopupVisible = false;
        _gastoAEliminar = null;
        MensajeConfirmacion = null;
    }
    #endregion
}
