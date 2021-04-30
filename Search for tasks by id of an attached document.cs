using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace finex.Entity.Server
{
	public class ModuleFunctions
	{
		/// <summary>
		/// Поиск задач
		/// </summary>
		/// <param name="id">Id Документа</param>
		/// <returns></returns>
		[Remote(PackResultEntityEagerly = true), Public]
		public static IQueryable<ITask> GetAllTask(int id)
		{ 
			return finex.Entity.Tasks.GetAll().Where(a=> a.AttachmentDetails.Any(d => d.AttachmentId == id));
		}
	}
}