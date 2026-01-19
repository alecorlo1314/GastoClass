namespace GastoClass.Presentacion.View;

public partial class TargetaCreditoPage : ContentPage
{
	public TargetaCreditoPage()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		mipopup.IsOpen = true;
    }

    private void BotonCancelar(object sender, EventArgs e)
    {

    }
}