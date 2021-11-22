using System.Windows;
using Prism.Ioc;
using Prism.Unity;
using ServiceRunner.Services;
using ServiceRunner.Views;

namespace ServiceRunner
{
	public partial class App : PrismApplication
	{
		/// <summary>
		/// Used to register types with the container that will be used by your application.
		/// </summary>
		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.Register<IServiceRunManager, ServiceRunManager>();
			containerRegistry.Register<IServicesRepository, ServicesRepository>();
		}

		/// <summary>Creates the shell or main window of the application.</summary>
		/// <returns>The shell of the application.</returns>
		protected override Window CreateShell()
		{
			return Container.Resolve<MainWindow>();
		}
	}
}
