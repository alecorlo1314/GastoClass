using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GastoClass.Aplicacion.CasosUso;
using GastoClass.Aplicacion.Common;
using GastoClass.Aplicacion.DTOs;
using GastoClass.Aplicacion.Excepciones;
using GastoClass.Aplicacion.UseCase;
using GastoClass.Dominio.Excepciones;
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

    #endregion

    #region Mensajes Error
    [ObservableProperty]
    private string? nombreTarjetaError;

    [ObservableProperty]
    private string? ultimosCuatroDigitosError;

    [ObservableProperty]
    private string? fechaExpiracionError;

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
        _obtenerTarjetaCreditoCasoUso = obtenerTarjetaCredito;
        _actualizarNombreTarjetaCasoUso = actualizarNombreTarjetaCasoUso;
        _agregarTarjetaCreditoCasoUso = agregarTarjetaCreditoCasoUso;
    }

    #endregion

    #region Comando Cargar
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

            TarjetasCredito.Clear();
            var tarjetas = await _obtenerTarjetaCreditoCasoUso.Ejecutar();

            foreach (var tarjeta in tarjetas!)
                TarjetasCredito.Add(tarjeta);
        }
        catch (Exception ex)
        {
           await Shell.Current.DisplayAlertAsync(
               "Error", 
               "Ocurrio un error al obtener las tarjetas: " + ex.Message,
               "Aceptar");
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion

    #region Comando Agregar Tarjeta
    /// <summary>
    /// Commando se encarga de agregar una tarjeta
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    private async Task AgregarAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        LimpiarMensajesError();
        try
        {
            TarjetaCreditoDto tarjetaCredito = new()
            {
                Tipo = TipoTarjetaSeleccionada,
                Nombre = NombreTarjeta,
                UltimosCuatroDigitos = UltimosCuatroDigitos,
                MesVencimiento = MesExpiracion,
                AnioVencimiento = AnioExpiracion
            };

            var resultado = await _agregarTarjetaCreditoCasoUso.Ejecutar(tarjetaCredito);

            if (!resultado.EsValido)
            {
                NombreTarjetaError = resultado.Errores.GetValueOrDefault("NombreTarjeta");
                UltimosCuatroDigitosError = resultado.Errores.GetValueOrDefault("UltimosCuatroDigitos");
                FechaExpiracionError = resultado.Errores.GetValueOrDefault("Vencimiento");
                return;
            }
            await Shell.Current.DisplayAlertAsync("Exito", "Tarjeta Agregada", "Aceptar");
            LimpiarFormulario();
            await CargarAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", "Ocurrio un error al agregar la tarjeta", "Aceptar");
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
    
    private void LimpiarMensajesError()
    {
        NombreTarjetaError = null;
        UltimosCuatroDigitosError = null;
        FechaExpiracionError = null;
    }
    #endregion
}
