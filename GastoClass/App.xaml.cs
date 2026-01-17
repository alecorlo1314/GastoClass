

namespace GastoClass
{
    public partial class App : Application
    {
        public App()
        {
            //Register Syncfusion<sup>®</sup> license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JGaF5cX2FCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdlWX5fcHVRRGVcU0B+V0ZWYEs=");

            InitializeComponent();
        }

        /// <summary>
        /// Indicar el tamano inicial para pantallas windows
        /// https://learn.microsoft.com/es-es/dotnet/maui/user-interface/controls/window?view=net-maui-10.0
        /// </summary>
        /// <param name="activationState"></param>
        /// <returns></returns>
        protected override Window CreateWindow(IActivationState? activationState) =>
            new Window(new AppShell())
            {
                Width = 1200,
                Height = 800,
                X = 100,
                Y = 100, 
            };
    }
}