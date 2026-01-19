using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GastoClass.Aplicacion.Utilidades;
using System.Collections.ObjectModel;

namespace GastoClass.Presentacion.ViewModel
{
    public partial class TarjetaCreditoViewModel : ObservableObject
    {
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
        /// Contiene el tipo de tarjeta en el sfcombobox
        /// </summary>
        [ObservableProperty]
        private TipoTarjeta? tipoTarjeta;

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

        [ObservableProperty]
        private string? nombreBanco;
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
        #endregion

        #region Propiedades Logicas
        /// <summary>
        /// Indica si el bono de ver mas detalles es accesible
        /// </summary>
        [ObservableProperty]
        private bool? masDetallesEsAccesible = false;

        /// <summary>
        /// Indica si el bono de finalizar el registro es accesible
        /// </summary>
        [ObservableProperty]
        private bool? finalizarRegistroAccesible = false;
        #endregion

        #region Mensajes
        /// <summary>
        /// Contiene el mensaje de requerimiento de ultimos cuatro digitos
        /// </summary>
        [ObservableProperty]
        private string? mensajeRequerimientoUltimosCuatroDigitos;

        /// <summary>
        /// Contiene el mensaje de requerimiento de tipo de tarjeta
        /// </summary>
        [ObservableProperty]
        private string? mensajeRequerimientoTipoTarjeta;

        /// <summary>
        /// Contiene el mensaje de requerimiento de nombre de tarjeta
        /// </summary>
        [ObservableProperty]
        private string? mensajeRequerimientoNombreTarjeta;

        /// <summary>
        /// Contiene el mensaje de requerimiento de fecha de vencimiento
        /// </summary>
        [ObservableProperty]
        private string? mensajeRequerimientoFechaVencimiento;

        /// <summary>
        /// Contiene el mensaje de requerimiento de limite de credito
        /// </summary>
        [ObservableProperty]
        private string? mensajeRequerimientoLimiteCredito;

        /// <summary>
        /// Contiene el mensaje de requerimiento de tipo de moneda
        /// </summary>
        [ObservableProperty]
        private string? mensajeRequerimientoTipoMoneda;

        /// <summary>
        /// Contiene el mensaje de requerimiento de dia de pago
        /// </summary>
        [ObservableProperty]
        private string? mensajeRequerimientoDiaPago;

        /// <summary>
        /// Contiene el mensaje de requerimiento de dia de corte
        /// </summary>
        [ObservableProperty]
        private string? mensajeRequerimientoDiaCorte;

        /// <summary>
        /// Contiene el mensaje de requerimiento de nombre del banco
        /// </summary>
        [ObservableProperty]
        private string? mensajeRequerimientoNombreBanco;
        #endregion

        #region Constructor
        public TarjetaCreditoViewModel()
        {
            //Inicializamos la lista de tipos de tarjetas
            ListaTipoTarjeta = new ObservableCollection<TipoTarjeta>
            {
                new(){ Tipo = "MasterCard"},
                new(){ Tipo = "Visa"},
                new() { Tipo = "Amex"}
            };
            //Inicializamos la lista de tipos de moneda
            ListaTipoMoneda = new ObservableCollection<TipoMoneda>
            {
                new(){ Moneda = "CRC"},
                new(){ Moneda = "USD"}
            };
        }
        #endregion

        #region Commandos
        /// <summary>
        /// Se ejecuta al tocar los bordes de colores para guardar los colores en las propiedades Hexadecimales
        /// </summary>
        /// <param name="borderSeleccionado"></param>
        /// <returns></returns>
        [RelayCommand]
        private async Task ColorTarjeta(Border borderSeleccionado)
        {
            if (borderSeleccionado.Background is LinearGradientBrush gradiente)
            {
                //obtener colores hexadecimal del borde seleccionado
                var coloresHexa = gradiente.GradientStops.Select(gs => gs.Color.ToHex()).ToList();
                ColorHexa1 = coloresHexa[0];
                ColorHexa2 = coloresHexa[1];
            }
            if (borderSeleccionado.Stroke is SolidColorBrush solid)
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync("Color", $"Color Border: {solid.Color.ToHex()}, ColorHex1={ColorHexa1}, ColorHex2={ColorHexa2}", "OK");
            }
        }
        [RelayCommand]
        private async Task FinalizarRegistroTarjetaCredito()
        {
            //Inicializamos un objeto TarjetaCredito
        }
        #endregion

        #region Metodo Observables
        /// <summary>
        /// Realiza la validacion para el sfComboBox 
        /// </summary>
        /// <param name="value"></param>
        partial void OnTipoTarjetaChanged(TipoTarjeta? value)
        {
            if (value == null)
            {
                //bloquear el bono de ver mas detalles antes de sacar el popup de agregar tarjeta
                MensajeRequerimientoTipoTarjeta = "Debes seleccionar un tipo de tarjeta";
                MasDetallesEsAccesible = false;
                return;
            }
            MensajeRequerimientoTipoTarjeta = string.Empty;
            MasDetallesEsAccesible = true;
        }
        /// <summary>
        /// Realiza la validacion para el nombre de la tarjeta
        /// </summary>
        /// <param name="value"></param>
        partial void OnNombreTarjetaChanged(string? value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length <= 2)
            {
                //bloquear el bono de ver mas detalles antes de sacar el popup de agregar tarjeta
                MasDetallesEsAccesible = false;
                MensajeRequerimientoNombreTarjeta = "Tiene que tener mas de 2 caracteres";
                return;
            }
            MensajeRequerimientoNombreTarjeta = string.Empty;
            MasDetallesEsAccesible = true;
        }
        /// <summary>
        /// Realiza la validacion para los ultimos cuatro digitos
        /// </summary>
        /// <param name="value"></param>
        partial void OnUltimosCuatroDigitosChanged(int? value)
        {
            //Validacion de longitud
            if (value?.ToString().Length != 4 || string.IsNullOrWhiteSpace(value.ToString()))
            {
                //bloquear el bono de ver mas detalles antes de sacar el popup de agregar tarjeta
                MasDetallesEsAccesible = false;
                MensajeRequerimientoUltimosCuatroDigitos = "Tienen que ser 4 digitos";
                return;
            }
            MensajeRequerimientoUltimosCuatroDigitos = string.Empty;
            MasDetallesEsAccesible = true;
        }
        /// <summary>
        /// Realiza la validacion para la fecha de vencimiento
        /// </summary>
        /// <param name="value"></param>
        partial void OnFechaVencimientoChanged(DateTime? value)
        {
            if (value < DateTime.Now)
            {
                //bloquear el bono de ver mas detalles antes de sacar el popup de agregar tarjeta
                MasDetallesEsAccesible = false;
                MensajeRequerimientoFechaVencimiento = "La fecha de vencimiento no puede ser menor al dia de hoy";
                return;
            }
            MasDetallesEsAccesible = true;
            MensajeRequerimientoFechaVencimiento = string.Empty;
        }
        /// <summary>
        /// Realiza la validacion para el limite de credito
        /// </summary>
        /// <param name="value"></param>
        partial void OnLimiteCreditoChanged(decimal? value)
        {
            if (value == 0)
            {
                finalizarRegistroAccesible = false;
                MensajeRequerimientoLimiteCredito = "No puede ser 0";
                return;
            }
            if (value < 0)
            {
                finalizarRegistroAccesible = false;
                MensajeRequerimientoLimiteCredito = "No puede ser negativo";
                return;
            }
            finalizarRegistroAccesible = true;
            MensajeRequerimientoLimiteCredito = string.Empty;
        }
        /// <summary>
        /// Realiza la validacion para el tipo de moneda
        /// </summary>
        /// <param name="value"></param>
        partial void OnTipoMonedaChanged(TipoMoneda? value)
        {
            if (value == null)
            {
                //bloquear el bono de ver mas detalles antes de sacar el popup de agregar tarjeta
                MensajeRequerimientoTipoMoneda = "Debes seleccionar un tipo de moneda";
                finalizarRegistroAccesible = false;
                return;
            }
            MensajeRequerimientoTipoMoneda = string.Empty;
            finalizarRegistroAccesible = true;
        }
        /// <summary>
        /// Realiza la validacion para el dia de corte
        /// </summary>
        /// <param name="value"></param>
        partial void OnDiaCorteChanged(int? value)
        {
            if(value == null)
            {
                //bloquear el bono de ver mas detalles antes de sacar el popup de agregar tarjeta
                finalizarRegistroAccesible = false;
                MensajeRequerimientoDiaCorte = "Debes seleccionar un dia";
                return;
            }
            if(value < 1 || value > 31)
            {
                //bloquear el bono de ver mas detalles antes de sacar el popup de agregar tarjeta
                finalizarRegistroAccesible = false;
                MensajeRequerimientoDiaCorte = "El dia debe estar entre 1 y 31";
                return;
            }
            finalizarRegistroAccesible = true;
            MensajeRequerimientoDiaCorte = string.Empty;
        }
        /// <summary>
        /// Realiza la validacion para el dia de pago
        /// </summary>
        /// <param name="value"></param>
        partial void OnDiaPagoChanged(int? value)
        {
            if (value == null)
            {
                //bloquear el bono de ver mas detalles antes de sacar el popup de agregar tarjeta
                finalizarRegistroAccesible = false;
                MensajeRequerimientoDiaPago = "Debes seleccionar un dia";
                return;
            }
            if (value < 1 || value > 31)
            {
                //bloquear el bono de ver mas detalles antes de sacar el popup de agregar tarjeta
                finalizarRegistroAccesible = false;
                MensajeRequerimientoDiaPago = "El dia debe estar entre 1 y 31";
                return;
            }
            finalizarRegistroAccesible = true;
            MensajeRequerimientoDiaPago = string.Empty;
        }
        /// <summary>
        /// Realiza la validacion para el nombre del banco
        /// </summary>
        /// <param name="value"></param>
        partial void OnNombreBancoChanged(string? value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length <= 2)
            {
                //bloquear el bono de ver mas detalles antes de sacar el popup de agregar tarjeta
                finalizarRegistroAccesible = false;
                MensajeRequerimientoNombreBanco = "Tiene que tener mas de 2 caracteres";
                return;
            }
            MensajeRequerimientoNombreBanco = string.Empty;
            finalizarRegistroAccesible = true;
        }
    }
    #endregion
}
