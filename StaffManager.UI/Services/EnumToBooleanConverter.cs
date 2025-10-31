using System.Globalization;
using System.Windows.Data;

namespace StaffManager.UI.Services
{
    [ValueConversion(typeof(Enum), typeof(bool))]
    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;
            string checkValue = value.ToString()!;
            string targetValue = parameter.ToString()!;
            return checkValue.Equals(targetValue, StringComparison.InvariantCultureIgnoreCase);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return Binding.DoNothing;
            return Enum.Parse(targetType, parameter.ToString()!);
        }
    }
}
