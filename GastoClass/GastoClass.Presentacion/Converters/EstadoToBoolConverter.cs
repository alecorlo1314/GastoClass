using System.Globalization;

namespace GastoClass.Presentacion.Converters;

public class EstadoToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        string estadoActual = value?.ToString()!;
        string estadoBoton = parameter?.ToString()!;
        return estadoActual == estadoBoton;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isChecked = (bool)value!;
        if (isChecked)
            return parameter?.ToString();
        return null;
    }
}
