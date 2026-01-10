using CommunityToolkit.Maui.Views;
using GastoClass.Presentacion.ViewModel;

namespace GastoClass.Presentacion.View;

public partial class AgregarGastoPopup : Popup
{
    public AgregarGastoPopup(AgregarGastoViewModel agregarGastoViewModel)
    {
        InitializeComponent();
        BindingContext = agregarGastoViewModel;
        DatePickerFecha.Date = DateTime.Now;

        //Conexion al callback del vm con el cierre real del Popup async
        //
    }

    private void OnCerrarClicked(object sender, EventArgs e)
    {
          //Cerrar Popup
          CloseAsync();
    }

    private void OnCancelarClicked(object sender, EventArgs e)
    {
        //Cerrar Popup
        CloseAsync();
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        // Cerrar y devolver resultado
        await CloseAsync();
    }
}