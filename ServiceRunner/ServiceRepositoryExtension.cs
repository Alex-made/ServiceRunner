using System.Collections.Generic;
using System.Collections.ObjectModel;
using ServiceRunner.Models;
using ServiceRunner.Services;

namespace ServiceRunner
{
	/// <summary>
	/// Представляет расширение для <see cref="ServicesRepository"/>.
	/// </summary>
	public static class ServiceRepositoryExtension
	{
		#region Public
		/// <summary>
		/// Возвращает коллекция сервисов типа <see cref="ObservableCollection{T}"/>.
		/// </summary>
		/// <param name="services">Коллекция сервисов.</param>
		public static ObservableCollection<Service> GetServiceObservableCollection(this IServicesRepository repository, IList<Service> services) =>
			new ObservableCollection<Service>(services);
		#endregion
	}
}
