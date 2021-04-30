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
		/// Создать задачу по процессу "Согласование договорарных документов". (Договор)
		/// </summary>
		/// <param name="document">Документ.</param>
		/// <returns>Задача по процессу "Согласование договорных документов".</returns>
		[Remote(PackResultEntityEagerly = true), Public]
		public static ITask CreateTaskC(finex.Subject.IContract document)
		{
			// Создание задачи
			var task = Tasks.Create();
			// Вложение документа в задачу
			task.AttachmentGroup.All.Add(document);
			// Заполнение темы
			task.Subject = "Согласование " + document.Name;
			// Заполнение автора
			task.AuthorFin = Sungero.Company.Employees.Current;
			// Заполнение подписанта
			task.Signer = document.OurSignatory;
			// Заполнение обязательных согласующих по категории
			//task.
			foreach (var employee in document.DocumentGroup.Employeefinex.OrderBy(d => d.Id))
			{
				var employeeList = Functions.Task.GetUsersFromGroups(employee.ChildProperty);
				foreach (var groupEmployee in employeeList)
				{
					task.Obligatorily.AddNew().Obligatorily = groupEmployee;
				}
			}
			return task;
		}
		
		/// <summary>
		/// Создать задачу по процессу "Согласование договорных документов". (Доп. соглашение)
		/// </summary>
		/// <param name="document">Документ.</param>
		/// <returns>Задача по процессу "Согласование договорных документов".</returns>
		[Remote(PackResultEntityEagerly = true), Public]
		public static ITask CreateTaskS(finex.Subject.ISupAgreement document)
		{
			// Создание задачи
			var task = Tasks.Create();
			// Вложение документа в задачу
			task.AttachmentGroup.All.Add(document);
			// Заполнение темы
			task.Subject = "Согласование " + document.Name;
			// Заполнение автора
			task.AuthorFin = Sungero.Company.Employees.Current;
			// Заполнение подписанта
			task.Signer = document.OurSignatory;
			// Заполнение обязательных согласующих по категории
			foreach (var employee in document.DocumentGroup.Employeefinex.OrderBy(d => d.Id))
			{
				var employeeList = Functions.Task.GetUsersFromGroups(employee.ChildProperty);
				foreach (var groupEmployee in employeeList)
				{
					task.Obligatorily.AddNew().Obligatorily = groupEmployee;
				}
			}
			return task;
		}
	}
}