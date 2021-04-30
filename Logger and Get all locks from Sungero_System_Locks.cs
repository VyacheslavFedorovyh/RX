using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace finex.Subject.Module.RecordManagementUI.Client
{
	partial class ModuleFunctions
	{
		var doc;
		// логирование ошибок
		Logger.DebugFormat("fx_DEBUG: document {0}", doc);
	}

	/// <summary>
	/// Получить все блокировки из Sungero_System_Locks
	/// </summary>
	/// <returns>List string. String = Login Id|User Name|EntityId|Entity TypeGuid|LockTime</returns>
	[Remote]
	public static List<string> GetLocksUsers()
	{
		var locksList = new List<string> {};
		
		using (var command = Sungero.Core.SQL.GetCurrentConnection().CreateCommand())
		{
			command.CommandText = Queries.Module.GetLocksUsers;
			using (var reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					var data = string.Format("{0}|{1}|{2}|{3}|{4}", reader.GetValue(0).ToString(), reader.GetValue(1).ToString(), reader.GetValue(2).ToString(), reader.GetValue(3).ToString(), reader.GetValue(4).ToString());
					locksList.Add(data);
				}
			}
		}
		
		return locksList;
	}
	
	/// <summary>
	/// Удалить блокировку из Sungero_System_Locks
	/// </summary>
	[Remote]
	public static bool DeleteLock(int entityId, int loginId, string entityTypeGuid)
	{
		bool execute = true;
		
		var text = string.Format("DELETE FROM dbo.Sungero_System_Locks WHERE EntityId = {0} AND Login = {1} AND EntityTypeGuid = '{2}'", entityId, loginId, entityTypeGuid);
		try
		{
			using (var command = Sungero.Core.SQL.GetCurrentConnection().CreateCommand())
			{
				command.CommandText = text;
				var resultT = command.ExecuteReader();
			}
		}
		catch (Exception ex)
		{
			execute = false;
			Logger.DebugFormat("fxERROR_SQL_DELETE: Во время удаления блокировки произошла ошибка: {0}", ex.Message);
		}
		return execute;
	}
}
