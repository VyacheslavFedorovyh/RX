using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Sungero.Domain.Initialization;

namespace finex.Entity.Server
{
	public class ModuleJobs
	{

		/// <summary>
		/// Проверка и запуск переодичсеких заданий
		/// </summary>
		public virtual void FonPeriodicTask()
		{
		
			// Можно функцией определить прошла ли дата
			// Получаю все сущности
			var error = PeriodicDocuments.GetAll()
				.Where(b => b.Status.Value.ToString() == "InWork") // Проверка статус "B работе" 
				.Where(b => Calendar.Between(b.NextDate.Value.Date, b.FirstDate.Value.Date, Calendar.Today.Date)); // Проверить, находится ли дата в указанном периоде
																												   // Возвращает true/false признак того что дата входит в период

			if (error != null)
			{
				// Перебираем error по n (количество записей в error)
				foreach (IPeriodicDocument n in error)
				{
					// Создание и отправка уведомления администратору
					PeriodicTask.Functions.PeriodicDocument.ErrorNot(n);
				}
			}
		}

	}
}

