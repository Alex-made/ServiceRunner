using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
			IterateWithActions(services,
							   service =>
							   {
								   //если сервис запущен, то добавляем запись в том, что он запущен в свойство Error сервиса и не трогаем его.
								   service.SetError("Сервис уже запущен");
							   },
							   service =>
							   {
								   //если сервис не запущен, то для него создать процесс и запустить этот процесс. Изменить статус этого сервиса на running.
								   var proc = new Process();
								   proc.StartInfo.WorkingDirectory = service.WorkingDirectory;
								   proc.StartInfo.FileName = service.AbsoluteFileName;
								   if (proc.Start())
								   {
									   service.Status = ServiceStatuses.Running;
								   }
								   //подписываемся на событие выхода процесса из работы. Работает только, если вызвать .Kill процесса из интерфейса, а не вручную закрыть окно.
								   proc.Exited += ProcessExitedEventHandler;
							   });
			//по окончании цикла вернуть коллекцию запущенных сервисов: с пустым Error для безошибочно запущенных сервисов, список не запущенных сервисов (ошибки запуска добавлены в свойство типа Error и в лог)
			return services;
		}

		/// <summary>
		/// Останавливает сервисы.
		/// </summary>
		/// <param name="services">Коллекция сервисов, которые необходимо остановить.</param>
		/// <returns>Коллекцию, содержащую успешно и не успешно остановленные сервисы.</returns>
		/// TODO возможно, не нужно ничего возвращать
		public IList<Service> StopServices(IList<Service> services)
		{
			//пробегаемся по коллекции сервисов
			IterateWithActions(services,
							   service =>
							   {
								   //если сервис запущен, то получить процесс и остановить его
								   var processes = Process.GetProcessesByName(Service.GetNameWithoutExtension(service.AbsoluteFileName));
								   if (processes.Any())
								   {
									   try
									   {
										   foreach (var process in processes)
										   {
											   process.Kill();
										   }
									   }
									   catch (Exception e)
									   {
										   service.SetError("При остановке сервиса возникла ошибка: " + e.Message);
									   }

									   service.Status = ServiceStatuses.Stopped;
								   }
							   },
							   service =>
							   {
								   //если сервис не запущен, то для него создать ошибку остановки и не трогать статус
								   service.SetError("Сервис уже остановлен");
							   });
			return services;
		}

		/// <summary>
		/// Выполняет действия на коллекции сервисов в зависимости от статуса сервиса.
		/// </summary>
		/// <param name="services">Коллекция сервисов.</param>
		/// <param name="onRunningAction">Действие, выполняющееся, если статус сервиса "Запущен".</param>
		/// <param name="onStoppingAction">Действие, выполняющееся, если статус сервиса "Остановлен".</param>
		private void IterateWithActions(IList<Service> services, Action<Service> onRunningAction, Action<Service> onStoppingAction)
		{
			foreach (var service in services)
			{
				switch (service.Status)
				{
					case ServiceStatuses.Running:
						onRunningAction.Invoke(service);
						break;
					case ServiceStatuses.Stopped:
						onStoppingAction.Invoke(service);
						break;
				}
			}
		}

		private static void ProcessExitedEventHandler(object? sender, EventArgs e)
		{
			var process = sender as Process;
			//Console.WriteLine(process.ExitCode);
		}
	}
}