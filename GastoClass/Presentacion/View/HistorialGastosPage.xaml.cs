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
}