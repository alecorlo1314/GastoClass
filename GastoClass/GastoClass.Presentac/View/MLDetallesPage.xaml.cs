using GastoClass.Presentacion.ViewModel;

namespace GastoClass.Presentacion.View;

public partial class MLDetallesPage : ContentPage
{
	public MLDetallesPage(MLDetallesViewModel mLDetallesViewModel)
	{
		InitializeComponent();
		//Inyeccion de dependencias
		BindingContext = mLDetallesViewModel;
	}
}