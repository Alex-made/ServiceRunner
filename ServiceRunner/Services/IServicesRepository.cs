using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceRunner.Models;

namespace ServiceRunner.Services
{
	public interface IServicesRepository
	{
		/// <summary>
		/// Возвращает коллекцию доступных для запуска сервисов.
		/// </summary>
		IList<Service> GetServices();
	}
}
