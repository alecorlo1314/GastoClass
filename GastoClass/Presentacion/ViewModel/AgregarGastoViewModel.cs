using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GastoClass.Dominio.Interfacez;
using GastoClass.Dominio.Model;

namespace GastoClass.Presentacion.ViewModel;

public partial class AgregarGastoViewModel : ObservableObject
{
    #region Datos de Gastos
    //Instancia
    public Gasto _gasto { get; set; } = new Gasto();

    //Caracteristicas
    [ObservableProperty]
    public decimal _monto;
    [ObservableProperty]
    public DateTime _fecha;
    [ObservableProperty]
    public string? _descripcion;
    [ObservableProperty]
    public string? _categoria;
    #endregion

    //Asignar accion para cerrar con resultado
    public Func<string?, Task>? solicitudCerrarAsync { get; set; }

    //Inyeccion de dependencias
    private readonly IServicioGastos _servicioGastos;
    public AgregarGastoViewModel(IServicioGastos servicioGasto)
    {
        //Inyeccion de dependencias
        _servicioGastos = servicioGasto;
    }
    //comando para guardar gasto
    [RelayCommand]
    public async Task GuardarGasto()
    {
        // Validaciones
        if (string.IsNullOrWhiteSpace(Monto.ToString()))
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync(
                "Error",
                "Ingresa un monto válido",
                "OK");
            return;
        }

        if (Categoria == null)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync(
                "Error",
                "Selecciona una categoría",
                "OK");
            return;
        }
        // Crear objeto de gasto
        var gasto = new Gasto
        {
            Monto = Monto,
            Categoria = Categoria,
            Descripcion = Descripcion,
            Fecha = Fecha
        };
        
        //Guardar Movimiento
        var resultado = await _servicioGastos.GuardarGastoAsync(gasto);

        if(resultado == 1 && solicitudCerrarAsync is not null)
        {
            await solicitudCerrarAsync(null);
        }
    }
}