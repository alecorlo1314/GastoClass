
using CommunityToolkit.Mvvm.ComponentModel;
using GastoClass.Aplicacion.CasosUso;
using GastoClass.Dominio.Model;
using System.Collections.ObjectModel;

namespace GastoClass.Presentacion.ViewModel
{
    [QueryProperty(nameof(TarjetaCredito), nameof(TarjetaCredito))]
    public partial class DetallesTarjetaCreditoViewModel : ObservableObject
    {
        private readonly ServicioTarjetaCredito _servicioTarjetaCredito;

        [ObservableProperty]
        private ObservableCollection<Gasto>? listaUltimosTresMovimientos = new();

        [ObservableProperty]
        private TarjetaCredito? tarjetaCredito;

        [ObservableProperty]
        private int? idTarjetaCredito;

        public DetallesTarjetaCreditoViewModel(ServicioTarjetaCredito servicioTarjetaCredito)
        {
            _servicioTarjetaCredito = servicioTarjetaCredito;
        }

        private async Task CargarUltimosTresMovimientosAsync()
        {
            try
            {
                if(TarjetaCredito == null) return;
                var movimientos = await _servicioTarjetaCredito.ObtenerUltimosTresGastosPorTarjetaCreditoAsync(IdTarjetaCredito);
                ListaUltimosTresMovimientos = new ObservableCollection<Gasto>(movimientos!);
            }
            catch (Exception ex)
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync("Error", "No se pudieron cargar los movimientos de la tarjeta "+ex.Message, "OK");
            }
        }
        partial void OnTarjetaCreditoChanged(TarjetaCredito? value)
        {
            idTarjetaCredito = value?.Id;
            _ = CargarUltimosTresMovimientosAsync();
        }
    }
}
