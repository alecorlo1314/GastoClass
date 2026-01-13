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

    private void Editar_Clicked(object sender, EventArgs e)
    {
        mipopup.IsOpen = true;
    }

    private void Eliminar_Clicked(object sender, EventArgs e)
    {

    }
}