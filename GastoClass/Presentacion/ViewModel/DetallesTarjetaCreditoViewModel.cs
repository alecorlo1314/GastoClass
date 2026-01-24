
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

        #region Propiedades de Navegación

        /// <summary>
        /// Tarjeta de crédito recibida desde la pantalla anterior mediante QueryProperty.
        /// Al establecerse, dispara la carga automática de datos relacionados.
        /// </summary>
        [ObservableProperty]
        private TarjetaCredito? tarjetaCredito;

        /// <summary>
        /// ID de la tarjeta de crédito actual.
        /// Se utiliza para consultar movimientos y gastos asociados.
        /// </summary>
        [ObservableProperty]
        private int? idTarjetaCredito;

        #endregion

        #region Colecciones de Movimientos

        /// <summary>
        /// Colección de los últimos 3 movimientos/gastos de la tarjeta de crédito.
        /// Se actualiza automáticamente cuando cambia la tarjeta seleccionada.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<UltimoTresMovimientoDTOs>? listaUltimosTresMovimientos = new();

        #endregion

        #region Colecciones de Gastos por Categoría (Últimos 7 Días)

        /// <summary>
        /// Gastos de la categoría "Alimentación" en los últimos 7 días.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDTO>? datosAlimentacion = new();

        /// <summary>
        /// Gastos de la categoría "Transporte" en los últimos 7 días.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDTO>? datosTransporte = new();

        /// <summary>
        /// Gastos de la categoría "Entretenimiento" en los últimos 7 días.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDTO>? datosEntretenimiento = new();

        /// <summary>
        /// Gastos de la categoría "Servicios" en los últimos 7 días.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDTO>? datosServicios = new();

        /// <summary>
        /// Gastos de la categoría "Ropa" en los últimos 7 días.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDTO>? datosRopa = new();

        /// <summary>
        /// Gastos de la categoría "Deportes" en los últimos 7 días.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDTO>? datosDeportes = new();

        /// <summary>
        /// Gastos de la categoría "Viajes" en los últimos 7 días.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDTO>? datosViajes = new();

        /// <summary>
        /// Gastos de la categoría "Tecnología" en los últimos 7 días.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDTO>? datosTecnologia = new();

        /// <summary>
        /// Gastos de la categoría "Educación" en los últimos 7 días.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDTO>? datosEducacion = new();

        /// <summary>
        /// Gastos de la categoría "Salud" en los últimos 7 días.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDTO>? datosSalud = new();

        /// <summary>
        /// Gastos de la categoría "Mascotas" en los últimos 7 días.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDTO>? datosMascotas = new();

        /// <summary>
        /// Gastos de la categoría "Hogar" en los últimos 7 días.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDTO>? datosHogar = new();

        #endregion

        #region Propiedades de Visualización de Tarjeta

        /// <summary>
        /// Balance actual de la tarjeta de crédito.
        /// </summary>
        [ObservableProperty]
        private decimal? balanceDetalles;

        /// <summary>
        /// Ruta o nombre del icono del chip de la tarjeta.
        /// </summary>
        [ObservableProperty]
        private string? iconoChipDetalles;

        /// <summary>
        /// Nombre del banco emisor de la tarjeta.
        /// </summary>
        [ObservableProperty]
        private string? nombreBancoDetalles;

        /// <summary>
        /// Mes de vencimiento de la tarjeta (1-12).
        /// </summary>
        [ObservableProperty]
        private int? mesVencimientoDetalles;

        /// <summary>
        /// Año de vencimiento de la tarjeta (formato completo, ej: 2025).
        /// </summary>
        [ObservableProperty]
        private int? anioVencimientoDetalles;

        /// <summary>
        /// Últimos 4 dígitos del número de tarjeta para identificación.
        /// </summary>
        [ObservableProperty]
        private int? ultimosCuatroDigitos;

        /// <summary>
        /// Ruta o nombre del icono del tipo de tarjeta (Visa, Mastercard, etc.).
        /// </summary>
        [ObservableProperty]
        private string? iconoTipoTarjetaDetalles;

        /// <summary>
        /// Color primario en formato hexadecimal para el degradado de la tarjeta.
        /// </summary>
        [ObservableProperty]
        private string? colorHex1Detalles;

        /// <summary>
        /// Color secundario en formato hexadecimal para el degradado de la tarjeta.
        /// </summary>
        [ObservableProperty]
        private string? colorHex2Detalles;

        /// <summary>
        /// Color del borde de la tarjeta en formato hexadecimal.
        /// </summary>
        [ObservableProperty]
        private string? colorBordeDetalles;

        /// <summary>
        /// Color del texto en la tarjeta en formato hexadecimal.
        /// </summary>
        [ObservableProperty]
        private string? colorTextoDetalles;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor del ViewModel.
        /// </summary>
        /// <param name="servicioTarjetaCredito">Servicio de tarjetas de crédito inyectado.</param>
        public DetallesTarjetaCreditoViewModel(ServicioTarjetaCredito servicioTarjetaCredito)
        {
            _servicioTarjetaCredito = servicioTarjetaCredito;
        }

        #endregion

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
        #region Métodos de Cambio de Propiedades (Property Changed)

        /// <summary>
        /// Se ejecuta automáticamente cuando la propiedad TarjetaCredito cambia.
        /// Inicializa el ID de la tarjeta y dispara la carga de datos relacionados:
        /// - Últimos 3 movimientos
        /// - Propiedades visuales de la tarjeta
        /// - Gastos por categoría de los últimos 7 días
        /// </summary>
        /// <param name="value">Nueva tarjeta de crédito seleccionada.</param>
        partial void OnTarjetaCreditoChanged(TarjetaCredito? value)
        {
            if (value == null) return;

            IdTarjetaCredito = value.Id;

            // Ejecutar carga de datos de forma asíncrona (fire and forget pattern)
            _ = CargarUltimosTresMovimientosAsync();
            _ = InicializarPropiedadesVisualesAsync(value);
            _ = CargarGastosPorCategoriaAsync(value.Id);
        }

        #endregion

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
