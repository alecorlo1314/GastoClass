using GastoClass.Presentacion.ViewModel;

namespace GastoClass.Presentacion.View;

public partial class DetallesTarjetaCreditoPage : ContentPage
{
    public DetallesTarjetaCreditoPage(DetallesTarjetaCreditoViewModel detallesTarjetaCreditoViewModel)
	{
        InitializeComponent();
        BindingContext = detallesTarjetaCreditoViewModel;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}