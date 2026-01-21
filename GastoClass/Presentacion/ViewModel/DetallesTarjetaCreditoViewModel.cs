
using CommunityToolkit.Mvvm.ComponentModel;
using GastoClass.Dominio.Model;

namespace GastoClass.Presentacion.ViewModel
{
    [QueryProperty("TarjetaCredito", "TarjetaCredito")]
    public partial class DetallesTarjetaCreditoViewModel : ObservableObject
    {
        [ObservableProperty]
        private TarjetaCredito? tarjetaCredito;

        public DetallesTarjetaCreditoViewModel()
        {

        }
    }
}
