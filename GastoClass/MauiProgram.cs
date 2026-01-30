using CommunityToolkit.Maui;
using GastoClass.Aplicacion.CasosUso;
using GastoClass.Aplicacion.Interfaces;
using GastoClass.Aplicacion.Servicios;
using GastoClass.Aplicacion.Servicios.Consultas.CategoriaPredicha;
using GastoClass.Dominio.Interfaces;
using GastoClass.Dominio.Interfacez;
using GastoClass.GastoClass.Aplicacion.Dashboard.Consultas.GastosPorCategoria;
using GastoClass.GastoClass.Aplicacion.Dashboard.Consultas.ResumenMes;
using GastoClass.GastoClass.Aplicacion.Dashboard.Consultas.UltimosCincoGastos;
using GastoClass.GastoClass.Aplicacion.HistorialGasto.Consultas;
using GastoClass.GastoClass.Dominio.Interfaces;
using GastoClass.Infraestructura.Repositorios;
using GastoClass.Presentacion.View;
using GastoClass.Presentacion.ViewModel;
using Infraestructura.Persistencia.ContextoDB;
using Infraestructura.Persistencia.Repositorios;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using Syncfusion.Maui.Toolkit.Hosting;
using System.Globalization;

namespace GastoClass
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Inter_18pt-Medium.ttf", "InterMedium");
                    fonts.AddFont("Inter_18pt-SemiBold.ttf", "InterSemiBold");
                    fonts.AddFont("Inter_24pt-Regular.ttf", "InterRegular");
                    fonts.AddFont("Poppins-SemiBold.ttf", "PoppinsSemiBold");
                    fonts.AddFont("Poppins-Bold.ttf", "PoppinsBold");
                    fonts.AddFont("Poppins-Medium.ttf", "PoppinsMedium");
                    fonts.AddFont("Poppins-Regular.ttf", "PoppinsRegular");

                });
            //Usa para poner las fechas en español, ejemplo (01 Ene 2026)
            var culture = new CultureInfo("es-ES"); 
            CultureInfo.DefaultThreadCurrentCulture = culture; 
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            builder.ConfigureSyncfusionToolkit();

            //vistas
            builder.Services.AddTransient<DashboardPage>();
            builder.Services.AddTransient<HistorialGastosPage>();
            builder.Services.AddTransient<MLDetallesPage>();
            builder.Services.AddTransient<ConfiguracionesPage>();
            builder.Services.AddTransient<TargetaCreditoPage>();
            builder.Services.AddTransient<DetallesTarjetaCreditoPage>();
            //ViewModels
            builder.Services.AddTransient<AgregarGastoViewModel>();
            builder.Services.AddTransient<ActualizarGastoViewModel>();
            builder.Services.AddTransient<EliminarGastoViewModel>();
            builder.Services.AddTransient<ConfiguracionesViewModel>();
            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<HistorialGastosViewModel>();
            builder.Services.AddTransient<MLDetallesViewModel>();
            builder.Services.AddTransient<TarjetaCreditoViewModel>();
            builder.Services.AddTransient<DetallesTarjetaCreditoViewModel>();

            //Repositorios y servicios
            builder.Services.AddSingleton<IServicioGastos, DatosGastos>();
            builder.Services.AddSingleton<IServicioTarjetaCredito, DatosTarjetasCredito>();
            builder.Services.AddScoped<IRepositorioGasto, RepositorioGasto>();
            builder.Services.AddSingleton<IPrediccionCategoriaServicio, PrediccionCategoriaServicio>();
            builder.Services.AddSingleton<IRepositorioTarjetaCredito, RepositorioTarjetaCredito>();
            builder.Services.AddSingleton<IRepositorioPreferenciaTarjeta, RepositorioPreferenciasTarjetaCredito>();
            builder.Services.AddSingleton<PrediccionCategoriaServicio>();
            builder.Services.AddSingleton<AppContextoDatos>();
            builder.Services.AddSingleton<ServicioGastos>();
            builder.Services.AddSingleton<ServicioTarjetaCredito>();
            builder.Services.AddSingleton<AppDbContext>();

            // Registrar MediatR indicando el assembly donde están tus Handlers
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(ObtenerResumenMesConsulta).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(ObtenerGastosPorCategoriaConsulta).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(ObtenerUltimosTresGastosConsulta).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(ObtenerGastosConsulta).Assembly);
            });

            // HTTP Client para el servicio de predicción
            builder.Services.AddSingleton(sp =>
            {
                return new HttpClient
                {
                    BaseAddress = new Uri("https://localhost:56288")
                };
            });
            builder.Services.AddSingleton<PredictionApiService>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
