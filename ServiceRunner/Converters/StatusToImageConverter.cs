using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ServiceRunner.Models;

namespace ServiceRunner
{
	public class StatusToImageConverter : IValueConverter
	{
		/// <summary>Converts a value.</summary>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				//MY TODO вернуть картинку "ошибка статуса")
				return null;
			}
			var status = (ServiceStatuses) value;
			if (status == ServiceStatuses.Running)
			{
				//TODO передалть на img.UriSource = new Uri("/Images/green-circle.png", UriKind.Relative);
				return "C:\\Users\\ab.smirnov\\source\\repos\\ServiceRunner\\ServiceRunner\\Images\\green-circle.png";
			}
			//TODO передалть на img.UriSource = new Uri("/Images/red-circle.png", UriKind.Relative);
			return "C:\\Users\\ab.smirnov\\source\\repos\\ServiceRunner\\ServiceRunner\\Images\\red-circle.png";
		}

		/// <summary>Converts a value.</summary>
		/// <param name="value">The value that is produced by the binding target.</param>
		/// <param name="targetType">The type to convert to.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
	}
}
