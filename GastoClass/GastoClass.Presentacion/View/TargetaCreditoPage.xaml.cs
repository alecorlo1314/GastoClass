using GastoClass.Presentacion.ViewModel;
namespace GastoClass.Presentacion.View;

public partial class TargetaCreditoPage : ContentPage
{
	public TargetaCreditoPage(TarjetaCreditoViewModel targetaCreditoViewModel)
	{
		InitializeComponent();
        //Inyeccion de Dependencias
        BindingContext = targetaCreditoViewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((TarjetaCreditoViewModel)BindingContext).InicializarAsync();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
		mipopup.IsOpen = true;
    }

    private void BotonCancelar(object sender, EventArgs e)
    {
        mipopup.IsOpen = false;
    }
}