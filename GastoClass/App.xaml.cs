

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

        //Indicar el tamano inicial para pantallas windows
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