using System.Globalization;

namespace GastoClass.Presentacion.Converts
{
    public class SfComboBoxSelectionChangedConverter : IValueConverter
    {
        public object? Convert(object? value, Type typeTarget, object? parametros, CultureInfo culture)
        {
            if (value is Syncfusion.Maui.Inputs.SelectionChangedEventArgs args)
            {
                return args.AddedItems?.FirstOrDefault();
            }
            return null;
        }
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
