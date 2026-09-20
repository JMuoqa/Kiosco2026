using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Kiosco2026.Clases.TextBlock
{
    public class OcultarTextoLargo : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string texto = value as string;
            if (string.IsNullOrEmpty(texto)) return string.Empty;

            int maxChars = int.Parse(parameter.ToString());
            return texto.Length > maxChars
                ? texto.Substring(0, maxChars) + "..."
                : texto;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value; // no se necesita convertir de vuelta
        }
    }
}
