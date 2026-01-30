using GastoClass.Presentacion.ViewModel;

namespace GastoClass.Presentacion.View;

public partial class DashboardPage : ContentPage
{
	public DashboardPage(DashboardViewModel dashboardViewModel)
	{
		InitializeComponent();
        BindingContext = dashboardViewModel;
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        mipopup.Show();
    }

    private void OnCancelarClicked(object sender, EventArgs e)
    {
        mipopup.IsOpen = false;
    }
}