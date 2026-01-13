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
        private readonly PredictionApiService _servicioPrediccionCategoriaML;

        #region Listas Observables
        [ObservableProperty]
        private ObservableCollection<Gasto>? listaGastos = new(); //Para la lista de gastos
        [ObservableProperty]
        private ObservableCollection<Gasto>? listaGastosFiltrados = new(); //Para la busqueda
        [ObservableProperty]
        private ObservableCollection<CategoriasRecomendadas> listaCategoriaFinal = new(); //Lista de Categorias que se quedara
        [ObservableProperty]
        private ObservableCollection<ResultadoPrediccion> listaCategoriasSugeridasML = new();
        #endregion
        //Variables 
        [ObservableProperty]
        private string? textoBusqueda;

        //Variables de edicion 
        [ObservableProperty]
        private decimal montoSeleccionado;
        [ObservableProperty]
        private string? descripcionSeleccionada;
        [ObservableProperty]
        private string? categoriaSeleccionada;
        [ObservableProperty]
        private DateTime fechaSeleccionada;

        private string? _descripcionOriginal;
        private bool _modoEdicion;
        private bool _descripcionCambiadaPorUsuario;

        //Tiempo 
        private CancellationTokenSource? _cts;
        //Clases 
        private CategoriasRecomendadas categoriaRecomendadaML;
        public HistorialGastosViewModel(ServicioGastos gastoService, PredictionApiService servicioPrediccionCategoriaML)
        {
            //Inyeccion de Dependencias
            _gastoService = gastoService;
            _servicioPrediccionCategoriaML = servicioPrediccionCategoriaML;
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
        [RelayCommand]
        private void EditarGasto(Gasto gasto)
        {
            _modoEdicion = true;

            _descripcionOriginal = gasto.Descripcion;
            _descripcionCambiadaPorUsuario = false;

            MontoSeleccionado = gasto.Monto;
            DescripcionSeleccionada = gasto.Descripcion;
            FechaSeleccionada = gasto.Fecha;
            CategoriaSeleccionada = gasto.Categoria;

            ListaCategoriasSugeridasML?.Clear();
            ListaCategoriaFinal?.Clear();
        }

        partial void OnDescripcionSeleccionadaChanged(string? value)
        {
            if (!_modoEdicion)
                return;

            if (!EsDescripcionValida(value))
                return;

            if (value == _descripcionOriginal)
            {
                ListaCategoriasSugeridasML?.Clear();
                return;
            }

            _descripcionCambiadaPorUsuario = true;

            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            _ = Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(500, _cts.Token);

                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        await CargarCategoriasSugeridasMLAsync();
                    });
                }
                catch (TaskCanceledException) { }
            });
        }

        private async Task CargarCategoriasSugeridasMLAsync()
        {
            if (!_descripcionCambiadaPorUsuario)
                return;

            if (!EsDescripcionValida(DescripcionSeleccionada))
                return;

            try
            {
                var prediction = await _servicioPrediccionCategoriaML
                    .PredictAsync(DescripcionSeleccionada!);

                if (prediction == null)
                    return;

                ListaCategoriasSugeridasML?.Clear();
                ListaCategoriasSugeridasML?.Add(prediction);

                CategoriaSeleccionada = prediction.Categoria;

                ListaCategoriaFinal = new ObservableCollection<CategoriasRecomendadas>
        {
            new()
            {
                DescripcionCategoriaRecomendada = prediction.Categoria,
                ScoreCategoriaRecomendada = prediction.Confidencial
            }
        };
            }
            catch (Exception ex)
            {
                await Shell.Current.CurrentPage
                    .DisplayAlertAsync("Error ML", ex.Message, "OK");
            }
        }

        private bool EsDescripcionValida(string? texto)
=> !string.IsNullOrWhiteSpace(texto) && texto.Length >= 3;
    }
}
