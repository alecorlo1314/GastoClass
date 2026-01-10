using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GastoClass.Dominio.Interfacez;
using GastoClass.Model;
using System.Security.Cryptography.X509Certificates;

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
        if (string.IsNullOrWhiteSpace(_monto.ToString()))
        {
            await Application.Current.Windows[0].Page.DisplayAlertAsync(
                "Error",
                "Ingresa un monto válido",
                "OK");
            return;
        }

        if (_categoria == null)
        {
            await Application.Current.Windows[0].Page.DisplayAlertAsync(
                "Error",
                "Selecciona una categoría",
                "OK");
            return;
        }
        // Crear objeto de gasto
        var gasto = new Gasto
        {
            Monto = _monto,
            Categoria = _categoria,
            Descripcion = _descripcion,
            Fecha = _fecha
        };
        
        //Guardar Movimiento
        var resultado = await _servicioGastos.GuardarGastoAsync(gasto);

        if(resultado == 1 && solicitudCerrarAsync is not null)
        {
            await solicitudCerrarAsync(null);
        }
    }
}