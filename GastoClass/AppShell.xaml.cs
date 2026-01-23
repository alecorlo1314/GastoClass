using GastoClass.Presentacion.View;

namespace GastoClass
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            //Registro de Paginas
            Routing.RegisterRoute(nameof(DetallesTarjetaCreditoPage), typeof(DetallesTarjetaCreditoPage));
        }
    }
}
