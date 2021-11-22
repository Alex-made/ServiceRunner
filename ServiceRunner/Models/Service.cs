using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using ServiceRunner.Annotations;

namespace ServiceRunner.Models
{
	public class Service : INotifyPropertyChanged
	{
		#region Delegates and events
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region .ctor
		public Service()
		{
			Error = new ServiceError();
			Id = Guid.NewGuid();
		}
		#endregion

		#region Properties
		//TODO переделать на приватное поле
		public ServiceError Error
		{
			get;
			set;
		}

		//TODO выпилить. не нужен.
		public Guid Id
		{
			get;
		}

		/// <summary>
		/// Возвращает имя файла с расширением.
		/// </summary>
		public string Name
		{
			get;
			set;
		}

		private ServiceStatuses _status;

		public ServiceStatuses Status
		{
			get => _status;
			set
			{
				_status = value;
				OnPropertyChanged();
			}
		}

		public string Version
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает абсолютный путь до папки с сервисом.
		/// </summary>
		public string WorkingDirectory
		{
			get;
			set;
		}

		public string AbsoluteFileName =>
			//TODO сделать красиво + сделать приватные поля и конструктор
			WorkingDirectory + "\\" + Name;
		#endregion

		#region Public
		/// <summary>
		/// Возвращает имя файла без расширения.
		/// </summary>
		public string GetNameWithoutExtension() => GetNameWithoutExtension(Name);

		/// <summary>
		/// Возвращает имя файла без расширения.
		/// </summary>
		/// <param name="filePathIncludingFileExtension">Путь до файла, вклачая имя файла с расширением.</param>
		public static string GetNameWithoutExtension(string filePathIncludingFileExtension) => Path.GetFileNameWithoutExtension(filePathIncludingFileExtension);

		/// <summary>
		/// Устанавливает описание ошибки при работе с сервисом.
		/// </summary>
		/// <param name="description"></param>
		public void SetError(string description)
		{
			Error.Description = description;
		}
		#endregion

		#region Overridable
		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}
