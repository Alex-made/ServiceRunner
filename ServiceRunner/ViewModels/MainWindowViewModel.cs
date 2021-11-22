using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Prism.Commands;
using Prism.Mvvm;
using ServiceRunner.Models;
using ServiceRunner.Services;

namespace ServiceRunner.ViewModels
{
	public class MainWindowViewModel : BindableBase
	{
		#region Data
		#region Fields
		private DelegateCommand<object> _runServicesCommand;
		private DelegateCommand<IList<Service>> _addToSelectedServicesCommand;
		private IServiceRunManager _serviceRunManager;
		private IServicesRepository _servicesRepository;

		private Service _selectedService;
		private List<Service> _selectedServices;
		private ObservableCollection<Service> _services;
		#endregion
		#endregion

		//TODO инжектить менеджер настроек. при повторном включении он должен отметить уже выбранные сервисы
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

			//TODO написать extension(т.к. это не прямой функционал репозитория), возвращающий ObservableCollection
			Services = new ObservableCollection<Service>(_servicesRepository.GetServices());
		}
		#endregion

		#region Properties
		public ObservableCollection<Service> Services
		{
			get => _services;
			private set
			{
				SetProperty(ref _services, value);
			}
		}

		public DelegateCommand<object> RunServicesCommand => _runServicesCommand ??= new DelegateCommand<object>(RunServicesCommandExecute);

		public DelegateCommand<IList<Service>> AddToSelectedServicesCommand => _addToSelectedServicesCommand ??= new DelegateCommand<IList<Service>>(AddToSelectedServicesCommandExecute);

		public Service SelectedService
		{
			get => _selectedService;
			set
			{
				SetProperty(ref _selectedService, value);
			}
		}

		public List<Service> SelectedServices
		{
			get => _selectedServices;
			set
			{
				SetProperty(ref _selectedServices, value);
			}
		}
		#endregion

		#region Private
		private void RunServicesCommandExecute(object selectedItems)
		{
			//TODO костыль!
			var selectedServices = (IList)selectedItems;
			if (selectedServices.Count == 0)
			{
				return;
			}
			var servicesCollection = selectedServices.Cast<Service>();
			
			_serviceRunManager.RunServices(servicesCollection.ToList());
		}

		private void AddToSelectedServicesCommandExecute(IList<Service> selectedService)
		{
		}
		#endregion
	}
}
