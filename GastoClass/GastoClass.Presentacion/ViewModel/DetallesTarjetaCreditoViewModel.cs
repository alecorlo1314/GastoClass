using CommunityToolkit.Mvvm.ComponentModel;
using GastoClass.Aplicacion.CasosUso;
using GastoClass.Aplicacion.DTOs;
using GastoClass.GastoClass.Aplicacion.Tarjeta.Consultas;
using GastoClass.GastoClass.Aplicacion.Tarjeta.DTOs;
using MediatR;
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
        private readonly ServicioTarjetaCredito _servicioTarjetaCredito;

        IMediator _mediator;

        #endregion

        #region Propiedades de Navegación

        /// <summary>
        /// Tarjeta de crédito recibida desde la pantalla anterior mediante QueryProperty.
        /// Al establecerse, dispara la carga automática de datos relacionados.
        /// </summary>
        [ObservableProperty] private DetallesTarjetaDto? tarjetaCredito;

        /// <summary>
        /// ID de la tarjeta de crédito actual.
        /// Se utiliza para consultar movimientos y gastos asociados.
        /// </summary>
        [ObservableProperty] private int? idTarjetaCredito;

        #endregion

        #region Colecciones de Movimientos

        /// <summary>
        /// Colección de los últimos 3 movimientos/gastos de la tarjeta de crédito.
        /// Se actualiza automáticamente cuando cambia la tarjeta seleccionada.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<UltimosTresMovimientosDto>? listaUltimosTresMovimientos = new();

        #endregion

        #region Colecciones de Gastos por Categoría (Últimos 7 Días)

        /// <summary>
        /// Gastos de la categoría "Alimentación" en los últimos 7 días.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDto>? datosAlimentacion = new();

        /// <summary>
        /// Gastos de la categoría "Transporte" en los últimos 7 días.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDto>? datosTransporte = new();

        /// <summary>
        /// Gastos de la categoría "Entretenimiento" en los últimos 7 días.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDto>? datosEntretenimiento = new();

        /// <summary>
        /// Gastos de la categoría "Servicios" en los últimos 7 días.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDto>? datosServicios = new();

        /// <summary>
        /// Gastos de la categoría "Ropa" en los últimos 7 días.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDto>? datosRopa = new();

        /// <summary>
        /// Gastos de la categoría "Deportes" en los últimos 7 días.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDto>? datosDeportes = new();

        /// <summary>
        /// Gastos de la categoría "Viajes" en los últimos 7 días.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDto>? datosViajes = new();

        /// <summary>
        /// Gastos de la categoría "Tecnología" en los últimos 7 días.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDto>? datosTecnologia = new();

        /// <summary>
        /// Gastos de la categoría "Educación" en los últimos 7 días.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDto>? datosEducacion = new();

        /// <summary>
        /// Gastos de la categoría "Salud" en los últimos 7 días.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDto>? datosSalud = new();

        /// <summary>
        /// Gastos de la categoría "Mascotas" en los últimos 7 días.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDto>? datosMascotas = new();

        /// <summary>
        /// Gastos de la categoría "Hogar" en los últimos 7 días.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDto>? datosHogar = new();

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
        public DetallesTarjetaCreditoViewModel(ServicioTarjetaCredito servicioTarjetaCredito, IMediator mediator)
        {
            _mediator = mediator;
            _servicioTarjetaCredito = servicioTarjetaCredito;
        }

        #endregion

        #region Métodos de Carga de Datos

        /// <summary>
        /// Carga los últimos 3 movimientos/gastos de la tarjeta de crédito actual.
        /// Maneja errores mostrando un alert al usuario.
        /// </summary>
        /// <returns>Tarea asíncrona.</returns>
        private async Task CargarUltimosTresMovimientosAsync()
        {
            try
            {
                if (IdTarjetaCredito == null) return;

                var movimientos = await _mediator.Send(new ObtenerUltimosTresMovimientosConsulta(IdTarjetaCredito.Value));

                ListaUltimosTresMovimientos = movimientos != null
                    ? new ObservableCollection<UltimosTresMovimientosDto>(movimientos)
                    : new ObservableCollection<UltimosTresMovimientosDto>();
            }
            catch (Exception ex)
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync(
                    "Error",
                    $"No se pudieron cargar los movimientos de la tarjeta: {ex.Message}",
                    "OK");
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
        partial void OnTarjetaCreditoChanged(DetallesTarjetaDto? value)
        {
            if (value == null) return;

            IdTarjetaCredito = value.IdTarjeta;

            // Ejecutar carga de datos de forma asíncrona (fire and forget pattern)
            _ = CargarUltimosTresMovimientosAsync();
            _ = InicializarPropiedadesVisualesAsync(value);
            _ = CargarGastosPorCategoriaAsync(value.IdTarjeta);
        }

        #endregion

        /// <summary>
        /// Inicializa las propiedades visuales de la tarjeta a partir del objeto TarjetaCredito.
        /// Estas propiedades se usan para renderizar la representación visual de la tarjeta.
        /// </summary>
        /// <param name="value">Tarjeta de crédito de la cual extraer las propiedades.</param>
        /// <returns>Tarea asíncrona.</returns>
        private async Task InicializarPropiedadesVisualesAsync(DetallesTarjetaDto? value)
        {
            if (value == null) return ;
            //var prefereciaTarjeta = await _mediator.Send(new ObtenerPreferenciaTarjetaConsulta(value.IdTarjeta));

            BalanceDetalles = value.Balance;
            IconoChipDetalles = value.IconoChip;
            NombreBancoDetalles = value.NombreBanco;
            MesVencimientoDetalles = value.MesVencimiento;
            AnioVencimientoDetalles = value.AnioVencimiento;
            UltimosCuatroDigitos = value.UltimosCuatroDigitos;
            IconoTipoTarjetaDetalles = value.IconoTipoTarjeta;
            ColorHex1Detalles = value.ColorHex1;
            ColorHex2Detalles = value.ColorHex2;
            ColorBordeDetalles = value.ColorBorde;
            ColorTextoDetalles = value.ColorTexto;
        }

        /// <summary>
        /// Carga y categoriza los gastos de los últimos 7 días por categoría.
        /// Cada categoría se almacena en su colección observable correspondiente
        /// para ser mostrada en gráficos o listas categorizadas.
        /// </summary>
        /// <param name="idTarjeta">ID de la tarjeta de la cual cargar los gastos.</param>
        /// <returns>Tarea asíncrona.</returns>
        private async Task CargarGastosPorCategoriaAsync(int idTarjeta)
        {
            try
            {
                var resultados = await _mediator.Send(new ObtenerGastosPorCategoriaSemanaConsulta(idTarjeta));

                if (resultados == null) return;

                // Limpiar y cargar cada categoría
                ActualizarCategoria(DatosAlimentacion, resultados, "Alimentacion");
                ActualizarCategoria(DatosTransporte, resultados, "Transporte");
                ActualizarCategoria(DatosEntretenimiento, resultados, "Entretenimiento");
                ActualizarCategoria(DatosServicios, resultados, "Servicios");
                ActualizarCategoria(DatosRopa, resultados, "Ropa");
                ActualizarCategoria(DatosDeportes, resultados, "Deportes");
                ActualizarCategoria(DatosViajes, resultados, "Viajes");
                ActualizarCategoria(DatosTecnologia, resultados, "Tecnologia");
                ActualizarCategoria(DatosEducacion, resultados, "Educacion");
                ActualizarCategoria(DatosSalud, resultados, "Salud");
                ActualizarCategoria(DatosMascotas, resultados, "Mascotas");
                ActualizarCategoria(DatosHogar, resultados, "Hogar");
            }
            catch (Exception ex)
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync(
                    "Error",
                    $"No se pudieron cargar los gastos por categoría: {ex.Message}",
                    "OK");
            }
        }

        #endregion

        #region Métodos Auxiliares

        /// <summary>
        /// Actualiza una colección de categoría específica filtrando los resultados por nombre de categoría.
        /// Limpia la colección existente y la repuebla con los datos filtrados.
        /// </summary>
        /// <param name="coleccion">valor a la colección observable a actualizar.</param>
        /// <param name="resultados">Lista completa de gastos categorizados.</param>
        /// <param name="nombreCategoria">Nombre de la categoría a filtrar.</param>
        private void ActualizarCategoria(
            ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDto>? coleccion,
            IEnumerable<GastoCategoriaUltimosSieteDiasTarjetaDto> resultados,
            string nombreCategoria)
        {
            if (coleccion == null) return;

            coleccion.Clear();
            foreach (var item in resultados.Where(g => g.Categoria == nombreCategoria))
            {
                coleccion.Add(item);
            }
        }

        #endregion
    }
}
