using System.Collections.Generic;
using ServiceRunner.Models;

namespace ServiceRunner.Services
{
	public interface IServiceRunManager
	{
		/// <summary>
		/// Запускает сервисы.
		/// </summary>
		/// <param name="services">Коллекция сервисов, которые необходимо запустить.</param>
		/// <returns>Коллекцию, содержащую успешно и не успешно запущенные сервисы.</returns>
		/// TODO возможно, не нужно ничего возвращать
		IList<Service> RunServices(IList<Service> services);

		/// <summary>
		/// Останавливает сервисы.
		/// </summary>
		/// <param name="services">Коллекция сервисов, которые необходимо остановить.</param>
		/// <returns>Коллекцию, содержащую успешно и не успешно остановленные сервисы.</returns>
		/// TODO возможно, не нужно ничего возвращать
		IList<Service> StopServices(IList<Service> services);
	}
}
