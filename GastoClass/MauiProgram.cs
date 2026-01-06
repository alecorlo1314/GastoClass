using Microsoft.Extensions.Logging;
using GastoClass.Infraestructura.Repositorios;
using GastoClass.View;
using GastoClass.Presentacion.View;
using GastoClass.ViewModel;
using GastoClass.Aplicacion.CasosUso;
using GastoClass.Dominio.Interfacez;

namespace GastoClass
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            //vistas
            builder.Services.AddTransient<AgregarGastoPage>();
            builder.Services.AddTransient<ConfiguracionesPage>();
            builder.Services.AddTransient<DashboardPage>();
            builder.Services.AddTransient<HistorialGastosPage>();
            builder.Services.AddTransient<MLDetallesPage>();
            //ViewModels
            builder.Services.AddTransient<AgregarGastoViewModel>();
            builder.Services.AddTransient<ConfiguracionesViewModel>();
            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<HistorialGastosViewModel>();
            builder.Services.AddTransient<MLDetallesViewModel>();

            //Repositorios y servicios
            builder.Services.AddSingleton<DatosGastos>();
            builder.Services.AddSingleton<RepositorioBaseDatos>();
            builder.Services.AddSingleton<IServicioGastos, DatosGastos>();
            builder.Services.AddSingleton<ServicioGastos>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
