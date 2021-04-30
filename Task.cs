using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace finex.Subject.Module.RecordManagementUI.Client
{
	partial class ModuleFunctions
	{
		/// <summary>
		/// Диалог для выбора папки
		/// </summary>
		public virtual void ImportDocLetter()
		{
			Functions.Module.Remote.DocumentRegistersGetAll();
		}
	}
}


namespace finex.Subject.Module.RecordManagementUI.Server
{
	partial class ModuleFunctions
	{

		/// <summary>
		/// Получение всех журналов регистрации
		/// </summary>
		[Remote, Public]
		public IQueryable<Sungero.Docflow.IDocumentRegister> DocumentRegistersGetAll()
		{
			return Sungero.Docflow.DocumentRegisters.GetAll();
		}
	}
}