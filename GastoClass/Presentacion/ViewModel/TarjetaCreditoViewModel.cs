using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace GastoClass.Presentacion.ViewModel
{
    public partial class TarjetaCreditoViewModel : ObservableObject
    {
        #region Propiedades UI
        /// <summary>
        /// Color Hexadecimal 1, para el lineargradient
        /// </summary>
        [ObservableProperty]
        private string? colorHexa1;

        /// <summary>
        /// Color Hexadecimal 2, para el lineargradient
        /// </summary>
        [ObservableProperty]
        private string? colorHexa2;
        #endregion
        public TarjetaCreditoViewModel() { }

        [RelayCommand]
        private async Task ColorTarjeta(Border borderSeleccionado)
        {
            if(borderSeleccionado.Background is LinearGradientBrush gradiente)
            {
                //obtener colores hexadecimal del borde seleccionado
                var coloresHexa = gradiente.GradientStops.Select(gs => gs.Color.ToHex()).ToList();
                ColorHexa1 = coloresHexa[0];
                ColorHexa2 = coloresHexa[1];
            }
            if (borderSeleccionado.Stroke is SolidColorBrush solid)
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync("Color", $"Color Border: {solid.Color.ToHex()}, ColorHex1={ColorHexa1}, ColorHex2={ColorHexa2}", "OK");
            }
        }
    }
}
