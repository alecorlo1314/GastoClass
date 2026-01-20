using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GastoClass.Aplicacion.CasosUso;
using GastoClass.Aplicacion.DTOs;
using GastoClass.Aplicacion.Utilidades;
using GastoClass.Dominio.Model;
using System.Collections.ObjectModel;

namespace GastoClass.Presentacion.ViewModel
{
    public partial class TarjetaCreditoViewModel : ObservableObject
    {
        #region Inyeccion de Dependencias
        private readonly ServicioTarjetaCredito _servicioTarjetaCredito;
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
        [NotifyPropertyChangedFor(nameof(MasDetallesEsAccesible))]
        private TipoTarjeta? tipoTarjeta;

        /// <summary>
        /// Contiene el nombre de la tajeta de credito
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MasDetallesEsAccesible))]
        private string? nombreTarjeta;

        /// <summary>
        /// Contiene los ultimos cuatro digitos
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MasDetallesEsAccesible))]
        private int? ultimosCuatroDigitos;

        /// <summary>
        /// Contiene la fecha de vencimiento
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MasDetallesEsAccesible))]
        private DateTime? fechaVencimiento;

        /// <summary>
        /// Contiene los datos del limite de credito
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FinalizarRegistroAccesible))]
        private decimal? limiteCredito;

        /// <summary>
        /// Contiene el tipo de moneda USA(Dolar) o CRC(Colones)
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FinalizarRegistroAccesible))]
        private TipoMoneda? tipoMoneda;

        /// <summary>
        /// Contiene el dia de corte
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FinalizarRegistroAccesible))]
        private int? diaCorte;

        /// <summary>
        /// Contiene el dia de pago
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FinalizarRegistroAccesible))]
        private int? diaPago;

        /// <summary>
        /// Contiene el nombre del banco
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FinalizarRegistroAccesible))]
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
        private ObservableCollection<TarjetaCredito>? listaTarjetasCredito = new();

        /// <summary>
        /// Contiene la lista de total de gastos por tarjeta
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<TotalGastoPorTarjeta>? listaTotalGastosPorTarjeta = new();
        #endregion

        #region Propiedades Logicas
        /// <summary>
        /// Indica si el botón de ver más detalles es accesible
        /// Se habilita cuando los primeros 4 campos son válidos
        /// </summary>
        public bool MasDetallesEsAccesible =>
            esValidoTipoTarjeta &&
            esValidoNombreTarjeta &&
            esValidoUltimosCuatroDigitos &&
            esValidoFechaVencimiento;

        /// <summary>
        /// Indica si el botón de finalizar el registro es accesible
        /// Se habilita cuando TODOS los campos son válidos
        /// </summary>
        public bool FinalizarRegistroAccesible =>
            MasDetallesEsAccesible &&
            esValidoLimiteCredito &&
            esValidoTipoMoneda &&
            esValidoDiaCorte &&
            esValidoDiaPago &&
            esValidoNombreBanco;

        /// <summary>
        /// Indica si el popup de agregar tarjeta esta abierto
        /// </summary>
        [ObservableProperty]
        private bool? popupAgregaTarjetaEstaAbiero;

        /// <summary>
        /// Indica si el tipo de tarjeta es válido
        /// </summary>
        private bool esValidoTipoTarjeta = false;

        /// <summary>
        /// Indica si el nombre de tarjeta es válido
        /// </summary>
        private bool esValidoNombreTarjeta = false;

        /// <summary>
        /// Indica si los últimos cuatro dígitos son válidos
        /// </summary>
        private bool esValidoUltimosCuatroDigitos = false;

        /// <summary>
        /// Indica si la fecha de vencimiento es válida
        /// </summary>
        private bool esValidoFechaVencimiento = false;

        private bool esValidoLimiteCredito = false;
        private bool esValidoTipoMoneda = false;
        private bool esValidoDiaCorte = false;
        private bool esValidoDiaPago = false;
        private bool esValidoNombreBanco = false;
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
        public TarjetaCreditoViewModel(ServicioTarjetaCredito servicioTarjetaCredito)
        {
            //Inyeccion de dependencias
            _servicioTarjetaCredito = servicioTarjetaCredito;

            _ = CargarTarjetasCredito();
            _ = CargarGastosPorTarjetaCreditoAsync();
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

        #region Carga Inicial
        /// <summary>
        /// Carga las tarjetas de credito
        /// </summary>
        /// <returns></returns>
        private async Task CargarTarjetasCredito()
        {
            try
            {
                var tarjetas = await _servicioTarjetaCredito.ObtenerTarjetasCreditoAsync();
                ListaTarjetasCredito = new ObservableCollection<TarjetaCredito>(tarjetas!);
            }
            catch (Exception ex)
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync("Error", ex.Message, "OK");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task CargarGastosPorTarjetaCreditoAsync()
        {
            try
            {
                var gastoPorTarjeta = await _servicioTarjetaCredito.ObtenerGastosPorTarjetasCreditoAsync();
                ListaTotalGastosPorTarjeta = new ObservableCollection<TotalGastoPorTarjeta>(gastoPorTarjeta!);
            }
            catch (Exception ex)
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync("Error", ex.Message, "OK");
            }
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
                ColorBorde = solid.Color.ToHex();
            }
            // para previsualizar cómo se verá la tarjeta
            if (ColorHexa1 == ColorHexa2 && ColorHexa1 == "#FFFFFF")
            {
                // Tarjeta blanca: texto oscuro, iconos grises, borde visible
                ColorTextoTarjeta = Color.FromArgb("343C6A").ToHex();
                IconoTipoTarjeta = $"icono_{TipoTarjeta?.Tipo?.ToString().ToLower() ?? "mastercard"}_gris.png";
                IconoChip = "icono_chip_gris.png";
            }
            else
            {
                // Tarjetas de color: texto claro, iconos blancos, borde según selección
                ColorTextoTarjeta = Color.FromArgb("F0F7FF").ToHex();
                IconoTipoTarjeta = $"icono_{TipoTarjeta?.Tipo?.ToString().ToLower() ?? "mastercard"}_blanco.png";
                IconoChip = "icono_chip_blanco.png";
            }

        }
        /// <summary>
        /// Se ejecuta al tocar el boton de finalizar el registro
        /// </summary>
        /// <returns></returns>
        [RelayCommand(CanExecute = nameof(FinalizarRegistroAccesible))]
        private async Task FinalizarRegistroTarjetaCredito()
        {
            if(ColorHexa1 == ColorHexa2)
            {
                ColorTextoTarjeta = Color.FromArgb("343C6A").ToHex();
                ColorBorde = Color.FromArgb("DFEAF2").ToHex();
                IconoTipoTarjeta = $"icono_{TipoTarjeta?.Tipo?.ToString().ToLower()}_gris.png";
                IconoChip = "icono_chip_gris.png";
            }
            else
            {
                ColorTextoTarjeta = Color.FromArgb("F0F7FF").ToHex();
                ColorBorde = "Transparent";
                IconoTipoTarjeta = $"icono_{TipoTarjeta?.Tipo?.ToString().ToLower()}_blanco.png";
                IconoChip = "icono_chip_blanco.png";
            }
            //Inicializamos un objeto PreferenciaTarjeta
            PreferenciaTarjeta preferenciaTarjeta = new()
                {
                    ColorHex1 = ColorHexa1,
                    ColorHex2 = ColorHexa2,
                    ColorBorde = ColorBorde,
                    ColorTexto = ColorTextoTarjeta,
                    IconoTipoTarjeta = IconoTipoTarjeta,
                    IconoChip = IconoChip
                };
            //Inicializamos un objeto TarjetaCredito
            TarjetaCredito tarjetaCredito = new ()
            {
                TipoTarjeta = this.TipoTarjeta?.Tipo,
                NombreTarjeta = this.NombreTarjeta,
                UltimosCuatroDigitos = this.UltimosCuatroDigitos,
                MesVencimiento = this.FechaVencimiento!.Value.Month,
                AnioVencimiento = this.FechaVencimiento!.Value.Year,
                LimiteCredito = this.LimiteCredito,
                Balance = 0,
                CreditoDisponible = this.LimiteCredito,
                Moneda = this.TipoMoneda?.Moneda,
                DiaCorte = this.DiaCorte,
                DiaPago = this.DiaPago, 
                NombreBanco = this.NombreBanco,
                IdPreferenciaTarjeta = preferenciaTarjeta.Id,
                PreferenciaTarjeta = preferenciaTarjeta
            };

            //Realizamos la llamada al servicio de agregar tarjeta
            var resultado = await _servicioTarjetaCredito.AgregarTarjetaCreditoAsync(tarjetaCredito);

            if(resultado == 1)
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync
                    ("Informacion", 
                    "La tarjeta se ingreso con exito", 
                    "Ok");
                LimpiarCampos();
                _ = CargarTarjetasCredito();
                PopupAgregaTarjetaEstaAbiero = false;
            }
            else
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync
                    ("Error", 
                    "No se pudo guardar la tarjeta", 
                    "Ok");
                return;
            }
        }

        [RelayCommand]
        private async Task EliminarTarjetasCredito()
        {
            var resultado = await _servicioTarjetaCredito.EliminarTarjetasCreditoAsync();
            if (resultado >= 1)
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync
                    ("Informacion",
                    "Tarjeta eliminadas con exito",
                    "Ok");
                _ = CargarTarjetasCredito();
            }
            else
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync
                    ("Error",
                    "No se pudo eliminar las tarjetas",
                    "Ok");
                return;
            }
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
                MensajeRequerimientoTipoTarjeta = "Debes seleccionar un tipo de tarjeta";
                esValidoTipoTarjeta = false;
                return;
            }
            MensajeRequerimientoTipoTarjeta = string.Empty;
            esValidoTipoTarjeta = true;
        }

        /// <summary>
        /// Realiza la validacion para el nombre de la tarjeta
        /// </summary>
        /// <param name="value"></param>
        partial void OnNombreTarjetaChanged(string? value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length <= 2)
            {
                esValidoNombreTarjeta = false;
                MensajeRequerimientoNombreTarjeta = "Tiene que tener mas de 2 caracteres";
                return;
            }
            MensajeRequerimientoNombreTarjeta = string.Empty;
            esValidoNombreTarjeta = true;
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
                esValidoUltimosCuatroDigitos = false;
                MensajeRequerimientoUltimosCuatroDigitos = "Tienen que ser 4 digitos";
                return;
            }
            MensajeRequerimientoUltimosCuatroDigitos = string.Empty;
            esValidoUltimosCuatroDigitos = true;
        }

        /// <summary>
        /// Realiza la validacion para la fecha de vencimiento
        /// </summary>
        /// <param name="value"></param>
        partial void OnFechaVencimientoChanged(DateTime? value)
        {
            if (value < DateTime.Now)
            {
                esValidoFechaVencimiento = false;
                MensajeRequerimientoFechaVencimiento = "La fecha de vencimiento no puede ser menor al dia de hoy";
                return;
            }
            MensajeRequerimientoFechaVencimiento = string.Empty;
            esValidoFechaVencimiento = true;
        }

        /// <summary>
        /// Realiza la validacion para el limite de credito
        /// </summary>
        /// <param name="value"></param>
        partial void OnLimiteCreditoChanged(decimal? value)
        {
            if (value == 0)
            {
                MensajeRequerimientoLimiteCredito = "No puede ser 0";
                esValidoLimiteCredito = false;
                ActualizarComandoFinalizar();
                return;
            }
            if (value < 0)
            {
                MensajeRequerimientoLimiteCredito = "No puede ser negativo";
                esValidoLimiteCredito = false;
                ActualizarComandoFinalizar();
                return;
            }
            MensajeRequerimientoLimiteCredito = string.Empty;
            esValidoLimiteCredito = true;
            ActualizarComandoFinalizar();
        }

        /// <summary>
        /// Realiza la validacion para el tipo de moneda
        /// </summary>
        /// <param name="value"></param>
        partial void OnTipoMonedaChanged(TipoMoneda? value)
        {
            if (value == null)
            {
                MensajeRequerimientoTipoMoneda = "Debes seleccionar un tipo de moneda";
                esValidoTipoMoneda = false;
                ActualizarComandoFinalizar();
                return;
            }
            MensajeRequerimientoTipoMoneda = string.Empty;
            esValidoTipoMoneda = true;
            ActualizarComandoFinalizar();
        }

        /// <summary>
        /// Realiza la validacion para el dia de corte
        /// </summary>
        /// <param name="value"></param>
        partial void OnDiaCorteChanged(int? value)
        {
            if (value == null)
            {
                esValidoDiaCorte = false;
                MensajeRequerimientoDiaCorte = "Debes seleccionar un dia";
                ActualizarComandoFinalizar();
                return;
            }
            if (value < 1 || value > 31)
            {
                esValidoDiaCorte = false;
                MensajeRequerimientoDiaCorte = "El dia debe estar entre 1 y 31";
                ActualizarComandoFinalizar();
                return;
            }
            esValidoDiaCorte = true;
            MensajeRequerimientoDiaCorte = string.Empty;
            ActualizarComandoFinalizar();
        }

        /// <summary>
        /// Realiza la validacion para el dia de pago
        /// </summary>
        /// <param name="value"></param>
        partial void OnDiaPagoChanged(int? value)
        {
            if (value == null)
            {
                esValidoDiaPago = false;
                MensajeRequerimientoDiaPago = "Debes seleccionar un dia";
                ActualizarComandoFinalizar();
                return;
            }
            if (value < 1 || value > 31)
            {
                esValidoDiaPago = false;
                MensajeRequerimientoDiaPago = "El dia debe estar entre 1 y 31";
                ActualizarComandoFinalizar();
                return;
            }
            esValidoDiaPago = true;
            MensajeRequerimientoDiaPago = string.Empty;
            ActualizarComandoFinalizar();
        }

        /// <summary>
        /// Realiza la validacion para el nombre del banco
        /// </summary>
        /// <param name="value"></param>
        partial void OnNombreBancoChanged(string? value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length <= 2)
            {
                esValidoNombreBanco = false;
                MensajeRequerimientoNombreBanco = "Tiene que tener mas de 2 caracteres";
                ActualizarComandoFinalizar();
                return;
            }
            MensajeRequerimientoNombreBanco = string.Empty;
            esValidoNombreBanco = true;
            ActualizarComandoFinalizar();
        }

        /// <summary>
        /// Actualiza el estado del comando FinalizarRegistro
        /// </summary>
        private void ActualizarComandoFinalizar()
        {
            FinalizarRegistroTarjetaCreditoCommand.NotifyCanExecuteChanged();
        }
        #endregion

        #region Metodo Limpieza
        /// <summary>
        /// Se encarga de limpiar las entradas despues de guardar la tarjeta de credito con exito
        /// </summary>
        private async void LimpiarCampos()
        {
            TipoTarjeta = null;
            NombreTarjeta = null;
            UltimosCuatroDigitos = null;
            FechaVencimiento = DateTime.Now;
            LimiteCredito = null;
            TipoTarjeta = new(){Tipo= "MasterCard" };
            TipoMoneda = new(){Moneda = "CRC"};
            DiaCorte = null;
            DiaPago = null;
            NombreBanco = null;
        }
        #endregion
    }
}
