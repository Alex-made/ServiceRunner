using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Prism.Commands;
using Prism.Mvvm;
using ServiceRunner.Models;
using ServiceRunner.Services;

namespace ServiceRunner.ViewModels
{
	//TODO Вопрос: 
	//1. Где лучше инициализирвоать программу данными? Прямо в VM главного окна?
	//2. Ничего, если я передаю ссылку на коллекцию другим объектам и они ее изменяют?
	//Может лучше сделать так, чтобы подключенные в VM сервисы возвращали данные и мы обновляли коллекцию непосредственно в нашей VM?


	/// <summary>
	/// Представляет модель представления главного окна.
	/// </summary>
	public class MainWindowViewModel : BindableBase
	{
		#region Data
		#region Fields
		private DelegateCommand<IList<Service>> _addToSelectedServicesCommand;
		private DelegateCommand<object> _runServicesCommand;
		private DelegateCommand<object> _stopServicesCommand;

		private Service _selectedService;
		private List<Service> _selectedServices;
		private readonly IServiceRunManager _serviceRunManager;
		private readonly ObservableCollection<Service> _services;
		private readonly IServicesRepository _servicesRepository;
		#endregion
		#endregion

		#region .ctor
		public MainWindowViewModel(IServiceRunManager serviceRunManager, IServicesRepository servicesRepository)
		{
			if (serviceRunManager != null)
			{
				_serviceRunManager = serviceRunManager;
			}

			if (servicesRepository != null)
			{
				_servicesRepository = servicesRepository;
			}

			var services = _servicesRepository.GetServices();
			//TODO зарегистрировать в контейнере
			var repeater = new RepeaterService();
			repeater.Run(services,
				CheckServices, 2000);

			Services = _servicesRepository.GetServiceObservableCollection(services);
		}
		#endregion

		private void CheckServices(IList<Service> services)
		{
			foreach (var service in services)
			{
				//получить имя из полного пути 
				var serviceName = Service.GetNameWithoutExtension(service.AbsoluteFileName);
				var processes = Process.GetProcessesByName(serviceName);

				//если процессы нашлись, значит они запущены.
				service.Status = processes.Any() ? ServiceStatuses.Running : ServiceStatuses.Stopped;
			}
		}

		#region Properties
		public DelegateCommand<IList<Service>> AddToSelectedServicesCommand =>
			_addToSelectedServicesCommand ??= new DelegateCommand<IList<Service>>(AddToSelectedServicesCommandExecute);

		public DelegateCommand<object> RunServicesCommand => _runServicesCommand ??= new DelegateCommand<object>(RunServicesCommandExecute);

		public DelegateCommand<object> StopServicesCommand => _stopServicesCommand ??= new DelegateCommand<object>(StopServicesCommandExecute);

		public Service SelectedService
		{
			get => _selectedService;
			set => SetProperty(ref _selectedService, value);
		}

		public List<Service> SelectedServices
		{
			get => _selectedServices;
			set => SetProperty(ref _selectedServices, value);
		}

		public ObservableCollection<Service> Services
		{
			get => _services;
			private init => SetProperty(ref _services, value);
		}
		#endregion

		#region Private
		private void AddToSelectedServicesCommandExecute(IList<Service> selectedService)
		{
		}

		private void RunServicesCommandExecute(object selectedItems)
		{
			//TODO костыль!
			var selectedServices = (IList) selectedItems;
			if (selectedServices.Count == 0)
			{
				return;
			}

			var servicesCollection = selectedServices.Cast<Service>();

			_serviceRunManager.RunServices(servicesCollection.ToList());
		}

		private void StopServicesCommandExecute(object selectedItems)
		{
			//TODO костыль!
			var selectedServices = (IList)selectedItems;
			if (selectedServices.Count == 0)
			{
				return;
			}

			var servicesCollection = selectedServices.Cast<Service>();

			_serviceRunManager.StopServices(servicesCollection.ToList());
		}
		#endregion

		//TODO инжектить менеджер настроек. при повторном включении он должен отметить уже выбранные сервисы
	}
}
