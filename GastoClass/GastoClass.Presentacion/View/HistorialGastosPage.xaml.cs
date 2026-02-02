using GastoClass.Presentacion.ViewModel;

namespace GastoClass.Presentacion.View;

public partial class HistorialGastosPage : ContentPage
{
	public HistorialGastosPage(HistorialGastosViewModel historialGastosViewModel)
	{
		InitializeComponent();
        //Inyeccion de Dependencias
        BindingContext = historialGastosViewModel;

    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (BindingContext is HistorialGastosViewModel vm)
        {
            vm.Dispose();
        }
    }

    private void Editar_Clicked(object sender, EventArgs e)
    {
        mipopup.IsOpen = true;
    }

    private void Eliminar_Clicked(object sender, EventArgs e)
    {

    }
}