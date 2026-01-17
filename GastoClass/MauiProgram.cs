using Microsoft.Extensions.Logging;
using GastoClass.Infraestructura.Repositorios;
using GastoClass.Presentacion.View;
using GastoClass.Presentacion.ViewModel;
using GastoClass.Aplicacion.CasosUso;
using GastoClass.Dominio.Interfacez;
using CommunityToolkit.Maui;
using Syncfusion.Maui.Toolkit.Hosting;
using Syncfusion.Maui.Core.Hosting;
using Microsoft.Maui.LifecycleEvents;

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
            builder.ConfigureSyncfusionToolkit();

            //vistas
            builder.Services.AddTransient<AgregarGastoPage>();
            builder.Services.AddTransient<DashboardPage>();
            builder.Services.AddTransient<HistorialGastosPage>();
            //ViewModels
            builder.Services.AddTransient<AgregarGastoViewModel>();
            builder.Services.AddTransient<ConfiguracionesViewModel>();
            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<HistorialGastosViewModel>();
            builder.Services.AddTransient<MLDetallesViewModel>();

            //Repositorios y servicios
            builder.Services.AddSingleton<IServicioGastos, DatosGastos>();
            builder.Services.AddSingleton<ServicioGastos>();
            builder.Services.AddSingleton<RepositorioBaseDatos>();


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
