
using CommunityToolkit.Mvvm.ComponentModel;
using GastoClass.Aplicacion.CasosUso;
using GastoClass.Aplicacion.DTOs;
using GastoClass.Dominio.Model;
using Syncfusion.Maui.DataSource.Extensions;
using System.Collections.ObjectModel;

namespace GastoClass.Presentacion.ViewModel
{
    /// <summary>
    /// ViewModel para la pantalla de detalles de tarjeta de crédito.
    /// Maneja la visualización de información de la tarjeta, últimos movimientos
    /// y gastos por categoría en los últimos 7 días.
    /// </summary>
    [QueryProperty(nameof(TarjetaCredito), nameof(TarjetaCredito))]
    public partial class DetallesTarjetaCreditoViewModel : ObservableObject
    {
        #region Servicios e Inyección de Dependencias

        /// <summary>
        /// Servicio para operaciones relacionadas con tarjetas de crédito.
        /// Proporciona acceso a datos de gastos, movimientos y categorías.
        /// </summary>
        private readonly ServicioTarjetaCredito _servicioTarjetaCredito;

        #endregion

        #region Listas
        /// <summary>
        /// Lista de movimientos de la tarjeta
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<UltimoTresMovimientoDTOs>? listaUltimosTresMovimientos = new();

        [ObservableProperty]
        public ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDTO>? datosAlimentacion = new();
        [ObservableProperty]
        public ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDTO>? datosTransporte = new();
        [ObservableProperty]
        public ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDTO>? datosEntretenimiento = new();
        [ObservableProperty]
        public ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDTO>? datosServicios = new();
        [ObservableProperty]
        public ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDTO>? datosRopa = new();
        [ObservableProperty]
        public ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDTO>? datosDeportes = new();
        [ObservableProperty]
        public ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDTO>? datosViajes = new();
        [ObservableProperty]
        public ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDTO>? datosTecnologia = new();
        [ObservableProperty]
        public ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDTO>? datosEducacion = new();
        [ObservableProperty]
        public ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDTO>? datosSalud = new();
        [ObservableProperty]
        public ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDTO>? datosMascotas = new();
        [ObservableProperty]
        public ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDTO>? datosHogar = new();
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

        /// <summary>
        /// Carga los ultimos 3 movimientos de la tarjeta
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Actualiza las propiedades para mostrar en la figura de la tarjeta
        /// </summary>
        /// <param name="value"></param>
        partial void OnTarjetaCreditoChanged(TarjetaCredito? value)
        {
            //Inicializar el id tarjeta
            IdTarjetaCredito = value?.Id;
            _ = CargarUltimosTresMovimientosAsync();
            _ = InicializarPropiedades(value);
            _ = CargarGastosPorTarjetaCreditoAsync(value!.Id);
        }

        /// <summary>
        /// Carga los gastos por categoria de la tarjeta
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task CargarGastosPorTarjetaCreditoAsync(int id)
        {
            var resultados = await _servicioTarjetaCredito.GastosPorCategoriaSemanaAsync(id);

            DatosAlimentacion?.Clear();
            DatosAlimentacion = new(
                from gasto in resultados
                where gasto.Categoria == "Alimentacion"
                select gasto);

            DatosTransporte?.Clear();
            DatosTransporte = new(
                from gasto in resultados
                where gasto.Categoria == "Transporte"
                select gasto);

            DatosEntretenimiento?.Clear();
            DatosEntretenimiento = new(
                from gasto in resultados
                where gasto.Categoria == "Entretenimiento"
                select gasto);

            DatosServicios?.Clear();
            DatosServicios = new(
                from gasto in resultados
                where gasto.Categoria == "Servicios"
                select gasto);

            DatosRopa?.Clear();
            DatosRopa = new(
                from gasto in resultados
                where gasto.Categoria == "Ropa"
                select gasto);

            DatosDeportes?.Clear();
            DatosDeportes = new(
                from gasto in resultados
                where gasto.Categoria == "Deportes"
                select gasto);

            DatosViajes?.Clear();
            DatosViajes = new(
                from gasto in resultados
                where gasto.Categoria == "Viajes"
                select gasto);

            DatosTecnologia?.Clear();
            DatosTecnologia = new(
                from gasto in resultados
                where gasto.Categoria == "Tecnologia"
                select gasto);

            DatosEducacion?.Clear();
            DatosEducacion = new(
                from gasto in resultados
                where gasto.Categoria == "Educacion"
                select gasto);

            DatosSalud?.Clear();
            DatosSalud = new(
                from gasto in resultados
                where gasto.Categoria == "Salud"
                select gasto);

            DatosMascotas?.Clear();
            DatosMascotas = new(
                from gasto in resultados
                where gasto.Categoria == "Mascotas"
                select gasto);

            DatosHogar?.Clear();
            DatosHogar = new(
                from gasto in resultados
                where gasto.Categoria == "Hogar"
                select gasto);
        }


        /// <summary>
        /// Inicializa las propiedades para mostrar en la figura de la tarjeta
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private async Task InicializarPropiedades(TarjetaCredito? value)
        {
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
