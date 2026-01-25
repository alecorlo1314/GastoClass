using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GastoClass.Aplicacion.CasosUso;
using GastoClass.Aplicacion.DTOs;
using GastoClass.Aplicacion.Excepciones;
using GastoClass.Aplicacion.UseCase;
using GastoClass.Dominio.Excepciones;
using GastoClass.Dominio.ValueObjects;
using System.Collections.ObjectModel;

namespace GastoClass.Presentacion.Views;

public partial class TarjetaCreditoViewModel : ObservableObject
{
    #region Casos de Uso 
    private readonly EliminarTarjetaCasoUso _eliminarTarjetaCasoUso;
    private readonly ObtenerTarjetaCreditoCasoUso _obtenerTarjetaCreditoCasoUso;
    private readonly ActualizarNombreTarjetaCasoUso _actualizarNombreTarjetaCasoUso;
    private readonly AgregarTarjetaCreditoCasoUso _agregarTarjetaCreditoCasoUso;

    #endregion

    #region Listas
    [ObservableProperty]
    private ObservableCollection<TarjetaCreditoDto> _tarjetasCredito = new();

    #endregion

    #region Estados
    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? mensajeError;

    #endregion

    #region Desde las Entradas
    [ObservableProperty]
    private string? tipoTarjetaSeleccionada;

    [ObservableProperty]
    private string? nombreTarjeta;

    [ObservableProperty]
    private string? ultimosCuatroDigitos;

    [ObservableProperty]
    private int mesExpiracion;

    [ObservableProperty]
    private int anioExpiracion;

    #endregion

    #region Contructor
    public TarjetaCreditoViewModel(EliminarTarjetaCasoUso eliminarTarjetaCasoUso,
        ObtenerTarjetaCreditoCasoUso obtenerTarjetaCredito,
        ActualizarNombreTarjetaCasoUso actualizarNombreTarjetaCasoUso,
        AgregarTarjetaCreditoCasoUso agregarTarjetaCreditoCasoUso)
    {
        _eliminarTarjetaCasoUso = eliminarTarjetaCasoUso;
        _obtenerTarjetaCredito = obtenerTarjetaCredito;
        _actualizarNombreTarjetaCasoUso = actualizarNombreTarjetaCasoUso;
        _agregarTarjetaCreditoCasoUso = agregarTarjetaCreditoCasoUso;
    }

    #endregion

    #region Comandos
    /// <summary>
    /// Commando se encarga de obtener las tarjetas
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    private async Task CargarAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            MensajeError = null;

            TarjetasCredito.Clear();
            var tarjetas = await _obtenerTarjetaCreditoCasoUso.Ejecutar();

            foreach (var tarjeta in tarjetas!)
                TarjetasCredito.Add(tarjeta);
        }
        catch (Exception ex)
        {
            MensajeError = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Commando se encarga de agregar una tarjeta
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    private async Task AgregarAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            MensajeError = null;

            await _agregarTarjetaCreditoCasoUso.Ejecutar(
                TipoTarjetaSeleccionada,
                NombreTarjeta,
                UltimosCuatroDigitos,
                MesExpiracion,
                AnioExpiracion);

            LimpiarFormulario();
            await CargarAsync();
        }
        catch (ExcepcionValidacionCasoUso ex)
        {
            MensajeError = ex.Message;
        }
        catch (ExcepcionDominio ex)
        {
            MensajeError = ex.Message;
        }
        catch (Exception)
        {
            MensajeError = "Ocurrió un error inesperado";
        }
        finally
        {
            IsBusy = false;
        }
    }
    #endregion

    #region Metodo Auxiliares
    private void LimpiarFormulario()
    {
        TipoTarjetaSeleccionada = null;
        NombreTarjeta = null;
        UltimosCuatroDigitos = null;
        MesExpiracion = DateTime.Now.Month;
        AnioExpiracion = DateTime.Now.Year;
    }

    #endregion
}
