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

    //private async void comboBox_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    //{
    //    var seleccionactual = e.AddedItems?[0].ToString();
    //    await Shell.Current.CurrentPage.DisplayAlertAsync("Alert", $"Se seleccion el item {seleccionactual}", "Ok");
    //}
}