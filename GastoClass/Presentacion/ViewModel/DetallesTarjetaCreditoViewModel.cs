
using CommunityToolkit.Mvvm.ComponentModel;
using GastoClass.Aplicacion.CasosUso;
using GastoClass.Aplicacion.DTOs;
using GastoClass.Dominio.Model;
using System.Collections.ObjectModel;

namespace GastoClass.Presentacion.ViewModel
{
    [QueryProperty(nameof(TarjetaCredito), nameof(TarjetaCredito))]
    public partial class DetallesTarjetaCreditoViewModel : ObservableObject
    {
        #region Inyeccion de dependencias
        /// <summary>
        /// Inyeccion de dependencias de ServicioTarjetaCredito
        /// </summary>
        private readonly ServicioTarjetaCredito _servicioTarjetaCredito;
        #endregion

        #region Listas
        /// <summary>
        /// Lista de movimientos de la tarjeta
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<UltimoTresMovimientoDTOs>? listaUltimosTresMovimientos = new();
        #endregion

        #region Propiedades Objetos
        /// <summary>
        /// Objeto Tarjeta Credito, recibido desde la pantalla anterior
        /// </summary>
        [ObservableProperty]
        private TarjetaCredito? tarjetaCredito;
        #endregion

        #region Propiedades Primitivas
        /// <summary>
        /// Contiene el id de la tarjeta detallada para consultar los movimientos
        /// </summary>
        [ObservableProperty]
        private int? idTarjetaCredito;
        #endregion

        #region Propiedades para mostrar en la figura de la tarjeta
        [ObservableProperty]
        private decimal? balanceDetalles;
        [ObservableProperty]
        private string? iconoChipDetalles;
        [ObservableProperty]
        private string? nombreBancoDetalles;
        [ObservableProperty]
        private int? mesVencimientoDetalles;
        [ObservableProperty]
        private int? anioVencimientoDetalles;
        [ObservableProperty]
        private int? ultimosCuatroDigitos;
        [ObservableProperty]
        private string? iconoTipoTarjetaDetalles;
        [ObservableProperty]
        private string? colorHex1Detalles;
        [ObservableProperty]
        private string? colorHex2Detalles;
        [ObservableProperty]
        private string? colorBordeDetalles;
        [ObservableProperty]
        private string? colorTextoDetalles;
        #endregion

        public DetallesTarjetaCreditoViewModel(ServicioTarjetaCredito servicioTarjetaCredito)
        {
            _servicioTarjetaCredito = servicioTarjetaCredito;
        }

        private async Task CargarUltimosTresMovimientosAsync()
        {
            try
            {
                var movimientos = await _servicioTarjetaCredito.ObtenerUltimosTresGastosPorTarjetaCreditoAsync(IdTarjetaCredito);
                ListaUltimosTresMovimientos = new ObservableCollection<UltimoTresMovimientoDTOs>(movimientos!);
            }
            catch (Exception ex)
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync("Error", "No se pudieron cargar los movimientos de la tarjeta "+ex.Message, "OK");
            }
         }
        partial void OnTarjetaCreditoChanged(TarjetaCredito? value)
        {

            //Inicializar el id tarjeta
            IdTarjetaCredito = value?.Id;
            _ = CargarUltimosTresMovimientosAsync();

            //Inicializar Tarjeta de Credito
            BalanceDetalles = value?.Balance;
            IconoChipDetalles = value?.PreferenciaTarjeta?.IconoChip;
            NombreBancoDetalles = value?.NombreBanco;
            MesVencimientoDetalles = value?.MesVencimiento;
            AnioVencimientoDetalles = value?.AnioVencimiento;
            UltimosCuatroDigitos = value?.UltimosCuatroDigitos;
            IconoTipoTarjetaDetalles = value?.PreferenciaTarjeta?.IconoTipoTarjeta;
            ColorHex1Detalles = value?.PreferenciaTarjeta?.ColorHex1;
            ColorHex2Detalles = value?.PreferenciaTarjeta?.ColorHex2;
            ColorBordeDetalles = value?.PreferenciaTarjeta?.ColorBorde;
            ColorTextoDetalles = value?.PreferenciaTarjeta?.ColorTexto;
        }
    }
}
