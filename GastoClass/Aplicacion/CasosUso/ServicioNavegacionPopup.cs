using CommunityToolkit.Maui.Extensions;
using GastoClass.Dominio.Interfacez;
using GastoClass.Presentacion.View;

namespace GastoClass.Aplicacion.CasosUso
{
    public class ServicioNavegacionPopup : IServicioNavegacionPopup
    {
        private readonly IServiceProvider _serviceProvider;
        public ServicioNavegacionPopup(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task MostrarPopupAgregarGasto()
        {
            var popup = _serviceProvider.GetRequiredService<AgregarGastoPopup>();

            await Application.Current.MainPage.ShowPopupAsync(popup);
        }
    }
}
