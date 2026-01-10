using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GastoClass.Dominio.Interfacez;
using GastoClass.Dominio.Model;
using System.Collections.ObjectModel;

namespace GastoClass.Presentacion.ViewModel;

//Se conectara a los casos de uso del dominio para obtener datos
//Obtendra datos del modelo y los preparara para la vista
//Se usaran metodos para obener datos relacionados a lo siguiente:
// - Gastos totales del mes
// - Cantidad de transacciones en este mes
// - Categoria con mayor gasto
// - Grafica de gastos por categoria
// - Gastos recientes (los ultimos 5 gastos)
public partial class DashboardViewModel : ObservableObject
{
    //Inyeccion de Dependencias
    private readonly IServicioNavegacionPopup _servicioNavegacionPopup;

    //Listas
    public ObservableCollection<Categorias> Categorias { get; set; }
    public DashboardViewModel(IServicioNavegacionPopup servicioNavegacionPopup)
    {
        //Inyeccion de dependencias
        _servicioNavegacionPopup = servicioNavegacionPopup;

        Categorias = new ObservableCollection<Categorias>();
        Categorias.Add(new Categorias() { Nombre = "Facebook", Id = 0 });
        Categorias.Add(new Categorias() { Nombre = "Google Plus", Id = 1 });
    }

    //Metodo carga asincrona inicial
    //Metodo para obtener gastos totales del mes
    //Metodo para obtener cantidad de transacciones en este mes
    //Metodo para obtener categoria con mayor gasto
    //Metodo para obtener los ultimos 5 gastos

    //Comando Abrir ventana de formulario de gastos
    [RelayCommand]
    public async Task PaginaAgregarGasto()
    {
        //Resultado del formulario
        await _servicioNavegacionPopup.MostrarPopupAgregarGasto();
    }
}
