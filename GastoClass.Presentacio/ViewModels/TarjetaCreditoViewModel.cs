using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GastoClass.Aplicacion.DTOs;
using GastoClass.Aplicacion.Gastos.Commands;
using GastoClass.Aplicacion.UseCase.TarjetaCreditoCasoUso;
using GastoClass.Dominio.Entidades;
using GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;
using MediatR;
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

    #region Inyeccion de Dependencias
    private readonly IMediator _mediator;

    #endregion

    #region Propiedades UI
    /// <summary>
    /// Color Hexadecimal 1, para el lineargradient
    /// </summary>
    [ObservableProperty]
    private string? colorHexa1;

    /// <summary>
    /// Color Hexadecimal 2, para el lineargradient
    /// </summary>
    [ObservableProperty]
    private string? colorHexa2;

    /// <summary>
    /// Contiene el color del borde
    /// </summary>
    [ObservableProperty]
    private string? colorBorde;

    /// <summary>
    /// Contiene el tipo de tarjeta en el sfcombobox
    /// </summary>
    [ObservableProperty]
    private TipoMoneda tipoMonedaSeleccionada;

    /// <summary>
    /// Contiene el nombre de la tajeta de credito
    /// </summary>
    [ObservableProperty]
    private string? nombreTarjeta;

    /// <summary>
    /// Contiene los ultimos cuatro digitos
    /// </summary>
    [ObservableProperty]
    private int? ultimosCuatroDigitos;

    /// <summary>
    /// Contiene la fecha de vencimiento
    /// </summary>
    [ObservableProperty]
    private DateTime? fechaVencimiento;

    /// <summary>
    /// Contiene los datos del limite de credito
    /// </summary>
    [ObservableProperty]
    private decimal? limiteCredito;

    /// <summary>
    /// Contiene el tipo de moneda USA(Dolar) o CRC(Colones)
    /// </summary>
    [ObservableProperty]
    private TipoMoneda? tipoMoneda;

    /// <summary>
    /// Contiene el dia de corte
    /// </summary>
    [ObservableProperty]
    private int? diaCorte;

    /// <summary>
    /// Contiene el dia de pago
    /// </summary>
    [ObservableProperty]
    private int? diaPago;

    /// <summary>
    /// Contiene el nombre del banco
    /// </summary>
    [ObservableProperty]
    private string? nombreBanco;

    /// <summary>
    /// Contiene el color del texto
    /// </summary>
    [ObservableProperty]
    private string? colorTextoTarjeta;

    /// <summary>
    /// Contiene el icono del tipo de tarjeta
    /// </summary>
    [ObservableProperty]
    private string? iconoTipoTarjeta;

    /// <summary>
    /// Contiene el icono del chip
    /// </summary>
    [ObservableProperty]
    private string? iconoChip;

    [ObservableProperty]
    private TipoTarjeta? tipoTarjetaSeleccionada;
    #endregion

    #region Propiedades Logica
    [ObservableProperty]
    private bool? borderVisible;

    [ObservableProperty]
    private bool? tituloTarjetasVisible;

    [ObservableProperty]
    private bool? tituloGraficoVisible;

    #endregion

    #region Listas Observables
    /// <summary>
    /// Contiene la lista de tipos de tarjeta
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<TipoTarjeta>? listaTipoTarjeta = new();

    /// <summary>
    /// Contiene la lista de tipos de moneda
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<TipoMoneda>? listaTipoMoneda = new();

    /// <summary>
    /// Contiene la lista de tarjetas de credito
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<TarjetaCreditoDto>? listaTarjetasCredito = new();

    /// <summary>
    /// Contiene la lista de total de gastos por tarjeta
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<TotalGastoPorTarjetaDto>? listaTotalGastosPorTarjeta = new();

    #endregion

    #region Estados
    [ObservableProperty]
    private bool isBusy;

    #endregion

    #region Mensajes Error
    [ObservableProperty]
    private string? tipoTarjetaError;

    [ObservableProperty]
    private string? nombreTarjetaError;

    [ObservableProperty]
    private string? ultimosCuatroDigitosError;

    [ObservableProperty]
    private string? limiteCreditoError;

    [ObservableProperty]
    private string? tipoMonedaError;

    [ObservableProperty]
    private string? diaCorteError;

    [ObservableProperty]
    private string? diaPagoError;

    [ObservableProperty]
    private string? nombreBancoError;

    #endregion

    #region Contructor
    public TarjetaCreditoViewModel(EliminarTarjetaCasoUso eliminarTarjetaCasoUso,
        ObtenerTarjetaCreditoCasoUso obtenerTarjetaCredito,
        ActualizarNombreTarjetaCasoUso actualizarNombreTarjetaCasoUso,
        AgregarTarjetaCreditoCasoUso agregarTarjetaCreditoCasoUso, IMediator mediator)
    {
        _eliminarTarjetaCasoUso = eliminarTarjetaCasoUso;
        _obtenerTarjetaCreditoCasoUso = obtenerTarjetaCredito;
        _actualizarNombreTarjetaCasoUso = actualizarNombreTarjetaCasoUso;
        _agregarTarjetaCreditoCasoUso = agregarTarjetaCreditoCasoUso;
        _mediator = mediator;

        //Inicializamos la lista de tipos de tarjetas
        ListaTipoTarjeta = new ObservableCollection<TipoTarjeta>
            {
                new("Visa"),
                new("MASTERCARD"),
                new("AMEX")
            };
        //Inicializamos la lista de tipos de moneda
        ListaTipoMoneda = new ObservableCollection<TipoMoneda>
            {
                new("CRC"),
                new("USD")
            };
    }

    #endregion

    #region Comando Cargar
    /// <summary>
    /// Commando se encarga de obtener las tarjetas
    /// </summary>
    /// <returns></returns>
    private async Task CargarAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            ListaTarjetasCredito?.Clear();
            var tarjetas = await _obtenerTarjetaCreditoCasoUso.Ejecutar();

            ListaTarjetasCredito = new ObservableCollection<TarjetaCreditoDto>(tarjetas!);
            BorderVisible = tarjetas?.Count == 0 || tarjetas == null ? false : true;
            TituloTarjetasVisible = tarjetas?.Count == 0 || tarjetas == null ? false : true;
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
    /// <summary>
    /// Commando se encarga de obtener los gastos por tarjeta
    /// </summary>
    /// <returns></returns>
    private async Task CargarGastosPorTarjetaCreditoAsync()
    {
        try
        {
            var gastoPorTarjeta = await _servicioTarjetaCredito.ObtenerGastosPorTarjetasCreditoAsync();
            ListaTotalGastosPorTarjeta = new ObservableCollection<TotalGastoPorTarjetaDto>(gastoPorTarjeta!);
            TituloGraficoVisible = ListaTotalGastosPorTarjeta?.Count == 0 || ListaTotalGastosPorTarjeta == null ? false : true;
        }
        catch (Exception ex)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync("Error", ex.Message, "OK");
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
            if (ColorHexa1 == ColorHexa2)
            {
                ColorTextoTarjeta = Color.FromArgb("343C6A").ToHex();
                ColorBorde = Color.FromArgb("DFEAF2").ToHex();
                IconoTipoTarjeta = $"icono_{TipoTarjetaSeleccionada?.Valor?.ToString().ToLower()}_gris.png";
                IconoChip = "icono_chip_gris.png";
            }
            else
            {
                ColorTextoTarjeta = Color.FromArgb("F0F7FF").ToHex();
                ColorBorde = "Transparent";
                IconoTipoTarjeta = $"icono_{TipoTarjetaSeleccionada?.Valor?.ToString().ToLower()}_blanco.png";
                IconoChip = "icono_chip_blanco.png";
            }
            //Inicializamos un objeto PreferenciaTarjeta
            PreferenciasTarjetaCreditoDto preferenciasTarjeta = new()
            {
                ColorHex1 = ColorHexa1,
                ColorHex2 = ColorHexa2,
                ColorBorde = ColorBorde,
                ColorTexto = ColorTextoTarjeta,
                IconoTipoTarjeta = IconoTipoTarjeta,
                IconoChip = IconoChip
            };
            //Inicializamos un objeto TarjetaCredito
            var resultado = await _mediator.Send(new AgragarTarjetaCreditoCommand
            {
                TipoTarjeta = TipoTarjetaSeleccionada?.Valor,
                Nombre = NombreTarjeta,
                UltimosCuatroDigitos = UltimosCuatroDigitos,
                MesVencimiento = FechaVencimiento!.Value.Month,
                AnioVencimiento = FechaVencimiento!.Value.Year,
                LimiteCredito = LimiteCredito,
                Balance = 0,
                CreditoDisponible = LimiteCredito,
                Moneda = TipoMonedaSeleccionada.Tipo,
                DiaCorte = DiaCorte,
                DiaPago = DiaPago,
                NombreBanco = NombreBanco,
                idPreferencia = preferenciasTarjeta.Id,
                PreferenciaTarjeta = preferenciasTarjeta
            });

            if (!resultado.EsValido)
            {
                TipoTarjetaError = resultado.Errores.GetValueOrDefault("TipoTarjeta");
                NombreTarjetaError = resultado.Errores.GetValueOrDefault("NombreTarjeta");
                UltimosCuatroDigitosError = resultado.Errores.GetValueOrDefault("UltimosCuatroDigitos");
                LimiteCreditoError = resultado.Errores.GetValueOrDefault("LimiteCredito");
                TipoMonedaError = resultado.Errores.GetValueOrDefault("Moneda");
                DiaCorteError = resultado.Errores.GetValueOrDefault("DiaCorte");
                DiaPagoError = resultado.Errores.GetValueOrDefault("DiaPago");
                NombreBancoError = resultado.Errores.GetValueOrDefault("NombreBanco");
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
        FechaVencimiento = DateTime.Now;
    }
    
    private void LimpiarMensajesError()
    {
        TipoTarjetaError = null;
       NombreTarjetaError = null;
        LimiteCreditoError = null;
        TipoMonedaError = null;
        DiaCorteError = null;
        DiaPagoError = null;
        NombreBancoError = null;
    }
    #endregion
}
