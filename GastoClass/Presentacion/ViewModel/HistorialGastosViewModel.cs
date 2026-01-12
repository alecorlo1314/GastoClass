using CommunityToolkit.Mvvm.ComponentModel;
using GastoClass.Aplicacion.CasosUso;
using GastoClass.Dominio.Model;
using System.Collections.ObjectModel;

namespace GastoClass.Presentacion.ViewModel
{
    public partial class HistorialGastosViewModel : ObservableObject
    {
        //Inyeccion de Dependencias
        private readonly ServicioGastos _gastoService;

        #region Listas Observables
        [ObservableProperty]
        private ObservableCollection<Gasto>? listaGastos = new();
        #endregion
        public HistorialGastosViewModel(ServicioGastos gastoService)
        {
            //Inyeccion de Dependencias
            _gastoService = gastoService;
            //Cargar lista de gastos
            //Task.Run(async () => await CargarListaMovimientos());
            _ = CargarListaMovimientos();
        }
        //Cargar lista de gastos desde la base de datos
        private async Task CargarListaMovimientos()
        {
            try
            {
                var consulta = await _gastoService.ObtenerGastosAsync();
                //Limpiamos la lista
                ListaGastos?.Clear();
                //cargar la lista de gastos con la consulta
                foreach (var gasto in consulta)
                {
                    ListaGastos?.Add(gasto);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
