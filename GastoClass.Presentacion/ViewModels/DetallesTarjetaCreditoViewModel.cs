using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GastoClass.Aplicacion.CarpetaGastos.Commands;
using GastoClass.Aplicacion.DetallesCarpeta.Consultas.ObtenerDetallesTarjeta;
using GastoClass.Aplicacion.DetallesCarpeta.Consultas.ObtenerGastoCategoriaUltimoSieteDiasTarjeta;
using GastoClass.Aplicacion.DetallesCarpeta.Consultas.ObtenerUltimosGastos;
using GastoClass.Dominio.Entidades;
using MediatR;
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
        #region Inyeccion de Dependencias
        private readonly IMediator _mediator;

        #endregion

        #region Propiedades de Navegación
        [ObservableProperty]private TarjetaCredito? tarjetaCredito;
        [ObservableProperty]private int? idTarjetaCreditoAcual;

        #endregion

        #region Colecciones de Movimientos
        [ObservableProperty]
        private ObservableCollection<GastoUltimosTresDto>? listaUltimosTresMovimientos = new();

        #endregion

        #region Colecciones de Gastos por Categoría (Últimos 7 Días)
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDto>? datosAlimentacion = new();
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDto>? datosTransporte = new();
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDto>? datosEntretenimiento = new();
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDto>? datosServicios = new();
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDto>? datosRopa = new();
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDto>? datosDeportes = new();
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDto>? datosViajes = new();
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDto>? datosTecnologia = new();
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDto>? datosEducacion = new();
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDto>? datosSalud = new();
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDto>? datosMascotas = new();
        [ObservableProperty]
        private ObservableCollection<GastoCategoriaUltimosSieteDiasTarjetaDto>? datosHogar = new();

        #endregion

        #region Propiedades de Visualización de Tarjeta
        [ObservableProperty]private decimal? balanceDetalles;
        [ObservableProperty]private string? iconoChipDetalles;
        [ObservableProperty]private string? nombreBancoDetalles;
        [ObservableProperty]private int? mesVencimientoDetalles;
        [ObservableProperty]private int? anioVencimientoDetalles;
        [ObservableProperty]private int? ultimosCuatroDigitos;
        [ObservableProperty]private string? iconoTipoTarjetaDetalles;
        [ObservableProperty]private string? colorHex1Detalles;
        [ObservableProperty]private string? colorHex2Detalles;
        [ObservableProperty]private string? colorBordeDetalles;
        [ObservableProperty]private string? colorTextoDetalles;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor del ViewModel.
        /// </summary>
        /// <param name="servicioTarjetaCredito">Servicio de tarjetas de crédito inyectado.</param>
        public DetallesTarjetaCreditoViewModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        #endregion

        #region OnTarjetaCreditoChanged

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

            //Sera el ID de consulta
            idTarjetaCreditoAcual = value.Id;

            //Tareas para cargar
            //- 1 - Ultimos 3 movimientos
            _ = CargarUltimosTresMovimientosAsync();
            //- 2 - Propiedades visuales
            _ = InicializarPropiedadesVisualesAsync();
            _ = CargarGastosPorCategoriaAsync();
        }

        #endregion

        #region Metodo Cargar Ultimos 3 Movimientos

        /// <summary>
        /// Carga los últimos 3 movimientos/gastos de la tarjeta de crédito actual.
        /// Maneja errores mostrando un alert al usuario.
        /// </summary>
        /// <returns>Tarea asíncrona.</returns>
        private async Task CargarUltimosTresMovimientosAsync()
        {
            try
            {
                if (IdTarjetaCreditoAcual == null) return;

                var movimientos = await _mediator.Send(new ObtenerUltimosTresGastosConsulta(IdTarjetaCreditoAcual));

                ListaUltimosTresMovimientos = movimientos != null
                    ? new ObservableCollection<GastoUltimosTresDto>(movimientos)
                    : new ObservableCollection<GastoUltimosTresDto>();
            }
            catch (Exception ex)
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync(
                    "Error",
                    $"No se pudieron cargar los movimientos de la tarjeta: {ex.Message}",
                    "OK");
            }
        }

        #endregion

        #region Metodo Inicializar Propiedades Visuales
        /// <summary>
        /// Inicializa las propiedades visuales de la tarjeta a partir del objeto TarjetaCredito.
        /// Estas propiedades se usan para renderizar la representación visual de la tarjeta.
        /// </summary>
        /// <param name="value">Tarjeta de crédito de la cual extraer las propiedades.</param>
        /// <returns>Tarea asíncrona.</returns>
        private async Task InicializarPropiedadesVisualesAsync() 
        { 
            var detalles = await _mediator.Send(new ObtenerDatosTarjetaCreditoConsulta(IdTarjetaCreditoAcual));

            BalanceDetalles = detalles.BalanceDetalles;
            IconoChipDetalles = detalles.IconoChipDetalles;
            NombreBancoDetalles = detalles.NombreBancoDetalles;
            MesVencimientoDetalles = detalles.MesVencimientoDetalles;
            AnioVencimientoDetalles = detalles.AnioVencimientoDetalles;
            UltimosCuatroDigitos = detalles.UltimosCuatroDigitos;
            IconoTipoTarjetaDetalles = detalles.IconoTipoTarjetaDetalles;
            ColorHex1Detalles = detalles.ColorHex1Detalles;
            ColorHex2Detalles = detalles.ColorHex2Detalles;
            ColorBordeDetalles = detalles.ColorBordeDetalles;
            ColorTextoDetalles = detalles.ColorHex2Detalles;
        }

        #endregion

        #region Metodo Cargar Gastos Por Categoria
        /// <summary>
        /// Carga y categoriza los gastos de los últimos 7 días por categoría.
        /// Cada categoría se almacena en su colección observable correspondiente
        /// para ser mostrada en gráficos o listas categorizadas.
        /// </summary>
        /// <param name="idTarjeta">ID de la tarjeta de la cual cargar los gastos.</param>
        /// <returns>Tarea asíncrona.</returns>
        private async Task CargarGastosPorCategoriaAsync()
        {
            try
            {
                var resultados = await _mediator.Send(new ObtenerGastoCategoriaUltimosSieteDiasTarjetaConsulta(IdTarjetaCreditoAcual));

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

        #region Comando Actualizar Tarjeta
        //[RelayCommand]
        //private async Task ActualizarTarjetaAsync()
        //{
        //    var resultados = await _mediator.Send(new ActualizarTarjetaCreditoCommand
        //    {
        //        IdTarjeta = IdTarjetaCredito!.Value,
        //        NombreTarjeta = TarjetaCredito!.NombreTarjeta.Valor,
        //        NombreBanco = TarjetaCredito.NombreBanco.Valor,
        //        TipoMoneda = TarjetaCredito.TipoMoneda.Tipo,
        //        TipoTarjeta = TarjetaCredito.Tipo.Valor
        //    });

        //    if (!resultados.EsValido)
        //    {
        //        await Shell.Current.CurrentPage.DisplayAlertAsync(
        //            "Atencio","No se pudo guarda la actualizacion","OK");
        //        return;
        //    }
        //    await Shell.Current.CurrentPage.DisplayAlertAsync(
        //            "Informacion", "Tarjeta actualizada con exito!", "OK");
        //}

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
