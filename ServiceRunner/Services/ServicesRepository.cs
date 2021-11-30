using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ServiceRunner.Models;

namespace ServiceRunner.Services
{
	public class ServicesRepository : IServicesRepository
	{
		#region IServicesRepository members
		/// <summary>
		/// Возвращает коллекцию доступных для запуска сервисов.
		/// </summary>
		public IList<Service> GetServices()
		{
			//TODO первая версия. будет захардкожен путь до папки с прогами. В следующей версии сделать, чтобы можно было указать путь пользователю и запомнить настройки.
			//настройки будем инжектить в этот сервис. через настройки будем брать путь к файлам. можно сделать просто json настройки, а не пользовательские xml.
			//лучше сделать кнопку "+", при которой будет OpenDialog и там добавлять нужную папку. Один раз добавил нужные папки, они отобразились в списке с чекбоксами. Далее, сервисы загружаем в список из папок, отмеяенных чекбоксами.

			var availableServices = new List<Service>();

			//пробежаться по каталогу
			var directories = Directory.EnumerateDirectories("C:\\Users\\ab.smirnov\\Downloads\\ServiceRunnerTest");
			foreach (var directory in directories)
			{
				if (directory.Contains("Northis"))
				{
					var northisFiles = Directory.GetFiles(directory);
					//получить пути к файлам exe
					var exeFileFullPaths = northisFiles.Where(file => file.EndsWith(".exe"));
					//если получил имя, то для каждого exe добавить в коллекцию Services (имя, путь, статус)
					foreach (var exeFileFullPath in exeFileFullPaths)
					{
						AddToServicesCollection(exeFileFullPath, availableServices);
					}
				}
			}

			return availableServices;
		}
		#endregion

		#region Private
		private void AddToServicesCollection(string exeFileFullPath, IList<Service> availableServices)
		{
			//получить имя из полного пути 
			var serviceName = Service.GetNameWithoutExtension(exeFileFullPath);
			var processes = Process.GetProcessesByName(serviceName);

			//если процессы нашлись, значит они запущены.
			if (processes.Any())
			{
				foreach (var process in processes)
				{
					availableServices.Add(new Service
					{
						Name = process.ProcessName + ".exe",
						WorkingDirectory = Path.GetDirectoryName(process.MainModule.FileName),
						Status = ServiceStatuses.Running,
						Version = Path.GetFileName(Path.GetDirectoryName(process.MainModule.FileName)) //TODO можно сделать, чтобы version проставлялся автоматически в конструкторе Service из имени WorkingDirectory
					});
				}
			}
			else
			{
				availableServices.Add(new Service
				{
					Name = serviceName + ".exe",
					WorkingDirectory = Path.GetDirectoryName(exeFileFullPath),
					Status = ServiceStatuses.Stopped,
					Version = Path.GetFileName(Path.GetDirectoryName(exeFileFullPath))
				});
			}
		}
		#endregion
	}
}
