using GastoClass.Presentacion.ViewModel;

namespace GastoClass.Presentacion.View;

public partial class DashboardPage : ContentPage
{
    private readonly DashboardViewModel _dashboardViewModel;
    private readonly AgregarGastoViewModel _agregarGastoViewModel;
    public DashboardPage(DashboardViewModel dashboardViewModel, AgregarGastoViewModel agregarGastoViewModel)
	{
		InitializeComponent();

        _dashboardViewModel = dashboardViewModel;
        _agregarGastoViewModel = agregarGastoViewModel;

        //BindingContext principal de la pagina
        BindingContext = _dashboardViewModel;

        //BindingContext del popup agregar gasto
        mipopup.BindingContext = _agregarGastoViewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((DashboardViewModel)BindingContext).InicializarDatosAsync();
    }
    private async void Button_Clicked(object sender, EventArgs e)
    {
        mipopup.Show();
        await _agregarGastoViewModel.InicializarDatosAsync();
    }

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        mipopup.IsOpen = false;

        // Refrescar datos del dashboard cuando se guarda una tarjeta
        await _dashboardViewModel.InicializarDatosAsync();
    }
}