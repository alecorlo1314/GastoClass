using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        [ObservableProperty]
        private ObservableCollection<Gasto>? listaGastosFiltrados = new();
        #endregion
        //Variables 
        [ObservableProperty]
        private string? textoBusqueda;
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
                ListaGastosFiltrados = new ObservableCollection<Gasto>(ListaGastos!);
            }
            catch (Exception ex)
            {

            }
        }

        partial void OnTextoBusquedaChanged(string? value)
        {
            _ = Filtrar(value);
        }
        private async Task Filtrar(string? textoBusqueda)
       {
            try
            {
                //Paso
                // - verificar el texto no este vacio
                // - Si esta vacio, mostrar toda la lista
                // - Si tiene texto, filtrar la lista por descripcion, categoria o monto
                // - Actualizar la lista observable
                if (string.IsNullOrWhiteSpace(textoBusqueda))
                {
                    ListaGastosFiltrados = new ObservableCollection<Gasto>(ListaGastos!);
                    return;
                }
                textoBusqueda = textoBusqueda.ToLower();
                var resultado = ListaGastos?.Where(g =>
            g.Descripcion!.ToLower().Contains(textoBusqueda) ||
            g.Id!.ToString().Contains(textoBusqueda) ||
            g.Categoria!.ToLower().Contains(textoBusqueda) ||
            g.Monto.ToString().Contains(textoBusqueda));

                ListaGastosFiltrados = new ObservableCollection<Gasto>(resultado!);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
            }
        }
        
    }
}
