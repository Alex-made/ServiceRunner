using System;
using System.Collections.Generic;
using System.Diagnostics;
using ServiceRunner.Models;

namespace ServiceRunner.Services
{
	//информация логируется: лог для безошибочно запущенных сервисов, лог для уже запущенных сервисов, лог для сервисов, не запущенных вследствие ошибки (ошибка также добавлена в лог)
	public class ServiceRunManager : IServiceRunManager
	{
		//TODO пробелемы:
		//1. Если запустил сервис из запускателя, а закрыл сам вручную, то собтие Exited не прилетает. Нужно мониторить запущенные процессы в отдельном потоке - сделать встроенную службу мониторинга 1 раз в секунду (настраиваемое в конфиге значение)
		//2. Служба мониторинга должна, видимо, через dispatcher передавать данные в Services, т.к. она будет работаь в другом потоке.

		public IList<Service> RunServices(IList<Service> services)
		{
			//пробегаемся по коллекции сервисов
			foreach (var service in services)
			{
				switch (service.Status)
				{
					//если сервис запущен, то добавляем запись в том, что он запущен в свойство Error сервиса и не трогаем его.
					case ServiceStatuses.Running:
						service.Error = new ServiceError
						{
							Description = "Сервис уже запущен"
						};
						break;
					//если сервис не запущен, то для него создать процесс и запустить этот процесс. Изменить статус этого сервиса на running.
					case ServiceStatuses.Stopped:
						var proc = new Process();
						proc.StartInfo.WorkingDirectory = service.WorkingDirectory;
						proc.StartInfo.FileName = service.AbsoluteFileName;
						if (proc.Start())
						{
							service.Status = ServiceStatuses.Running;
						}
						//подписываемся на событие выхода процесса из работы. Работает только, если вызвать .Kill процесса из интерфейса, а не вручную закрыть окно.
						proc.Exited += ProcessExitedEventHandler;
						break;
				}
			}

			//по окончании цикла вернуть коллекцию запущенных сервисов: с пустым Error для безошибочно запущенных сервисов, список не запущенных сервисов (ошибки запуска добавлены в свойство типа Error и в лог)
			return services;
		}

		private static void ProcessExitedEventHandler(object? sender, EventArgs e)
		{
			var process = sender as Process;
			//Console.WriteLine(process.ExitCode);
		}
	}
}
