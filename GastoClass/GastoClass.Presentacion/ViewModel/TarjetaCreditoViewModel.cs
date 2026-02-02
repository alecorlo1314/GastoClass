using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GastoClass.Aplicacion.CasosUso;
using GastoClass.Aplicacion.DTOs;
using GastoClass.Aplicacion.Tarjeta.Commands;
using GastoClass.Aplicacion.Utilidades;
using GastoClass.Dominio.Model;
using GastoClass.GastoClass.Aplicacion.Tarjeta.Consultas;
using GastoClass.GastoClass.Aplicacion.Tarjeta.DTOs;
using GastoClass.Presentacion.View;
using MediatR;
using System.Collections.ObjectModel;

namespace GastoClass.Presentacion.ViewModel
{
    public partial class TarjetaCreditoViewModel : ObservableObject
    {
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
        [ObservableProperty]
        private ObservableCollection<DetallesTarjetaDto>? listaTarjetasCredito = new();
        [ObservableProperty]
        private ObservableCollection<GastoTarjetaDto>? listaTotalGastosPorTarjeta = new();


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

        #region Estados de carga
        [ObservableProperty]
        private bool? estaOcupado = false;

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
        [ObservableProperty] private bool? popupAgregaTarjetaEstaAbiero;

        private bool esValidoTipoTarjeta = false;
        private bool esValidoNombreTarjeta = false;
        private bool esValidoUltimosCuatroDigitos = false;
        private bool esValidoFechaVencimiento = false;
        private bool esValidoLimiteCredito = false;
        private bool esValidoTipoMoneda = false;
        private bool esValidoDiaCorte = false;
        private bool esValidoDiaPago = false;
        private bool esValidoNombreBanco = false;

        [ObservableProperty] private bool? borderVisible;

        //Estado se utilizan para mostrar cuando hay registros de tarjetas
        [ObservableProperty] private bool? tituloTarjetasVisible;
        [ObservableProperty] private bool? tituloGraficoVisible;

        /// <summary>
        /// Si se selecciona un color de tarjeta este se pondra en true
        /// </summary>
        [ObservableProperty] private bool? borderFueSeleccionado = false;
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
        [ObservableProperty]
        private string? mensajeRequerimientoLimiteCredito;
        [ObservableProperty]
        private string? mensajeRequerimientoTipoMoneda;
        [ObservableProperty]
        private string? mensajeRequerimientoDiaPago;
        [ObservableProperty]
        private string? mensajeRequerimientoDiaCorte;
        [ObservableProperty]
        private string? mensajeRequerimientoNombreBanco;
        #endregion

        #region Constructor
        public TarjetaCreditoViewModel(IMediator mediator)
        {
            _mediator = mediator;
        }
        #endregion

        #region Inicializar Datos
        public async Task InicializarAsync()
        {
            try
            {
                //Importante
                _ = CargarTarjetasCreditoAsync();
                _ = CargarGastosPorTarjetaCreditoAsync();

                //No tan Importante
                _ = CargarListaTiposTarjetasCredito();
                _ = InicializarTiposMonedas();
            }
            catch (Exception ex)
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync
                ("Error",
                "No se pudo inicializar la cargar" + ex.Message,
                "Ok");
            }
        }

        #endregion

        #region Carga inicial de tarjetas de crédito
        public async Task CargarTarjetasCreditoAsync()
        {
            try
            {
                var tarjetas = await _mediator.Send(new ObtenerTarjetaCreditoConsulta());

                ListaTarjetasCredito = new ObservableCollection<DetallesTarjetaDto>(tarjetas!);
                BorderVisible = tarjetas?.Count == 0 || tarjetas == null ? false : true;
                TituloTarjetasVisible = tarjetas?.Count == 0 || tarjetas == null ? false : true;
            }
            catch (Exception ex)
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync("Error", ex.Message, "OK");
            }
        }

        #endregion

        #region Carga inicial de gastos por tarjeta
        private async Task CargarGastosPorTarjetaCreditoAsync()
        {
            try
            {
                var gastoPorTarjeta = await _mediator.Send(new ObtenerGastoPorTarjetaCreditoConsulta());
                ListaTotalGastosPorTarjeta = new ObservableCollection<GastoTarjetaDto>(gastoPorTarjeta!);
                TituloGraficoVisible = ListaTotalGastosPorTarjeta?.Count == 0 || ListaTotalGastosPorTarjeta == null ? false : true;
            }
            catch (Exception ex)
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync("Error", ex.Message, "OK");
            }
        }
        #endregion

        #region Inicializar Tipos de Tarjetas de Credito
        private async Task CargarListaTiposTarjetasCredito()
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

        #region Inicializar Tipos de Monedas

        private async Task InicializarTiposMonedas()
        {
            ListaTipoMoneda = new ObservableCollection<TipoMoneda>
            {
                new(){ Moneda = "CRC"},
                new(){ Moneda = "USD"}
            };
        }

        #endregion

        #region Commando Seleccionar Color Tarjeta
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

        #endregion

        #region Comando Finalizar Registro
        [RelayCommand(CanExecute = nameof(FinalizarRegistroAccesible))]
        private async Task FinalizarRegistroTarjetaCredito()
        {
            if(BorderFueSeleccionado!.Value) return;

            try
            {
                EstaOcupado = true;

                if (ColorHexa1 == ColorHexa2)
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

                var agregarTarjetaCommand = new AgragarTarjetaCreditoCommand
                {
                    Tipo = this.TipoTarjeta?.Tipo,
                    Nombre = this.NombreTarjeta,
                    UltimosCuatroDigitos = this.UltimosCuatroDigitos!.Value,
                    MesVencimiento = this.FechaVencimiento!.Value.Month,
                    AnioVencimiento = this.FechaVencimiento!.Value.Year,
                    LimiteCredito = this.LimiteCredito!.Value,
                    TipoMoneda = this.TipoMoneda?.Moneda,
                    DiaCorte = this.DiaCorte!.Value,
                    DiaPago = this.DiaPago!.Value,
                    NombreBanco = this.NombreBanco,
                    //Preferencias Visuales
                    ColorHex1 = ColorHexa1,
                    ColorHex2 = ColorHexa2,
                    ColorBorde = ColorBorde,
                    ColorTexto = ColorTextoTarjeta,
                    IconoTipoTarjeta = IconoTipoTarjeta,
                    IconoChip = IconoChip
                };

                var resultado = await _mediator.Send(agregarTarjetaCommand);

                if (resultado.EsValido)
                {
                    await Shell.Current.CurrentPage.DisplayAlertAsync
                        ("Informacion",
                        "La tarjeta se ingreso con exito",
                        "Ok");
                    LimpiarCampos();
                    _ = CargarTarjetasCreditoAsync();
                    PopupAgregaTarjetaEstaAbiero = false;
                    EstaOcupado = false;
                }
                else
                {
                    await Shell.Current.CurrentPage.DisplayAlertAsync
                        ("Error",
                        "No se pudo guardar la tarjeta",
                        "Ok");
                    EstaOcupado = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync("Error", ex.Message, "OK");
            }finally
            {
                EstaOcupado = false;
            }
        }

        #endregion

        #region Comando eliminar Tarjetas Credito
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
                _ = CargarTarjetasCreditoAsync();
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

        #region Metodos de Cambio Basicos
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
        partial void OnNombreTarjetaChanged(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                esValidoNombreTarjeta = false;
                MensajeRequerimientoNombreTarjeta = "Campo requerido";
                return;
            }
            if (value!.Length <= 2)
            {
                esValidoNombreTarjeta = false;
                MensajeRequerimientoNombreTarjeta = "Minimo 3 caracteres";
                return;
            }
            if (value.Length > 50)
            {
                esValidoNombreTarjeta = false;
                MensajeRequerimientoNombreTarjeta = "Maximo 50 caracteres";
                return;
            }
            MensajeRequerimientoNombreTarjeta = string.Empty;
            esValidoNombreTarjeta = true;
        }
        partial void OnUltimosCuatroDigitosChanged(int? value)
        {
            if (string.IsNullOrWhiteSpace(value.ToString()))
            {
                esValidoUltimosCuatroDigitos = false;
                MensajeRequerimientoUltimosCuatroDigitos = "Campo es requerido";
                return;
            }
            if (value?.ToString().Length != 4)
            {
                esValidoUltimosCuatroDigitos = false;
                MensajeRequerimientoUltimosCuatroDigitos = "Tienen que ser 4 digitos";
                return;
            }
            MensajeRequerimientoUltimosCuatroDigitos = string.Empty;
            esValidoUltimosCuatroDigitos = true;
        }
        partial void OnFechaVencimientoChanged(DateTime? value)
        {
            if (string.IsNullOrWhiteSpace(value.ToString()) || string.IsNullOrEmpty(value.ToString()))
            {
                esValidoFechaVencimiento = false;
                MensajeRequerimientoFechaVencimiento = "Campo es requerido";
                return;
            }
            //De dominio - Corregir 
            if (value < DateTime.Now)
            {
                esValidoFechaVencimiento = false;
                MensajeRequerimientoFechaVencimiento = "La fecha de vencimiento no puede ser menor al dia de hoy";
                return;
            }
            MensajeRequerimientoFechaVencimiento = string.Empty;
            esValidoFechaVencimiento = true;
        }

        #endregion

        #region Metodos de Cambio Finales
        partial void OnLimiteCreditoChanged(decimal? value)
        {
            if (string.IsNullOrWhiteSpace(value.ToString()) || string.IsNullOrEmpty(value.ToString()))
            {
                MensajeRequerimientoLimiteCredito = "Campo es requerido";
                esValidoLimiteCredito = false;
                ActualizarComandoFinalizar();
                return;
            }
            if(!int.TryParse(value.ToString(), out _))
            {
                MensajeRequerimientoLimiteCredito = "Solo se permiten numeros";
                esValidoLimiteCredito = false;
                ActualizarComandoFinalizar();
                return;
            }
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
        partial void OnDiaCorteChanged(int? value)
        {
            if (value == null)
            {
                esValidoDiaCorte = false;
                MensajeRequerimientoDiaCorte = "Campo requerido";
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
        partial void OnNombreBancoChanged(string? value)
        {
            if(string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(value))
            {
                esValidoNombreBanco = false;
                MensajeRequerimientoNombreBanco = "Campo requerido";
                ActualizarComandoFinalizar();
                return;
            }
            if (value.Length <= 2)
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

        #endregion

        #region Metodo aprobar registro formulario
        private void ActualizarComandoFinalizar()
        {
            FinalizarRegistroTarjetaCreditoCommand.NotifyCanExecuteChanged();
        }

        #endregion

        #region Metodo Limpieza
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

        #region Navegacion
        [RelayCommand]
        private async Task TarjetaSeleccionada(TarjetaCredito tarjeta)
        {
            if (tarjeta == null) return;

            // Navegar a la página de detalles pasando la tarjeta
            await Shell.Current.GoToAsync(nameof(DetallesTarjetaCreditoPage),
                new Dictionary<string, object>
                {
                    ["TarjetaCredito"] = tarjeta 
                });
        }
        #endregion
    }
}
