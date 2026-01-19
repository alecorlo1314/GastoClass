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
        #endregion

        #region ListasObservables
        [ObservableProperty]
        private ObservableCollection<TipoTarjeta>? listaTipoTarjeta = new();
        #endregion

        #region Propiedades Logicas
        [ObservableProperty]
        private bool? masDetallesEsAccesible = false;
        #endregion

        #region Mensajes
        [ObservableProperty]
        private string? mensajeRequerimientoUltimosCuatroDigitos;

        [ObservableProperty]
        private string? mensajeRequerimientoTipoTarjeta;

        [ObservableProperty]
        private string? mensajeRequerimientoNombreTarjeta;

        [ObservableProperty]
        private string? mensajeRequerimientoFechaVencimiento;
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
        }
        #endregion

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

        #region Metodo Observables
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
            masDetallesEsAccesible = true;
        }
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
    }
    #endregion
}
