using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Sungero.Domain.Initialization;

namespace finex.Entity.Server
{
	public partial class ModuleInitializer
	{
		public override void Initializing(Sungero.Domain.ModuleInitializingEventArgs e)
		{
			CreateRole("Руководитель юр. отдела", "Руководитель юр. отдела, роль с одним участником", Constants.Module.Initialize.FinanceArchiveGuid);
			GrantRightsOnFolder(); // Создать типы документов для договоров.
			CreateDocumentTypes(); // Функция инициализации для выдачи прав на "Проектные документы".
			RevokeRightsToObjects();
			GrantRights();
			Docs();
		}

		/// <summary>
		/// Создать тип документа для договоров.
		/// </summary>
		public static void CreateDocumentTypes()
		{
			InitializationLogger.Debug("Init: Create default document types");
			Sungero.Docflow.PublicInitializationFunctions.Module.CreateDocumentType("Проектные документы", DocumentProject.ClassTypeGuid, Sungero.Docflow.DocumentType.DocumentFlow.Inner, true);
		}
		
		/// <summary>
		/// Функция инициализации для выдачи прав на "Проектные документы".
		/// </summary>
		public static void GrantRights()
		{
			InitializationLogger.Debug("Init: Issuance of rights to all");
			var role = Roles.GetAll().FirstOrDefault(r => r.Sid == Constants.Module.Initialize.FinanceArchiveGuid);
			if (role == null)
				return;
			finex.Entity.DocumentProjects.AccessRights.Grant(role, DefaultAccessRightsTypes.Create);
			finex.Entity.DocumentProjects.AccessRights.Save();
		}

		/// <summary>
		/// Удаление прав доступа.
		/// </summary>
		public static void RevokeRightsToObjects()
		{
			var allUsers = Roles.AllUsers;
			if (allUsers != null)
			{
				Sungero.Contracts.IncomingInvoices.AccessRights.Revoke(allUsers, DefaultAccessRightsTypes.Create);
				Sungero.Contracts.IncomingInvoices.AccessRights.Grant(allUsers, DefaultAccessRightsTypes.Forbidden);
				Sungero.Contracts.IncomingInvoices.AccessRights.Save();
				InitializationLogger.Debug("fx_Init: Удалены права доступа на Создание для документа Входящий счет");
			}
		}
		
		/// <summary>
		/// Функция инициализации для выдачи прав на вычисляемые папки.
		/// </summary>
		public static void GrantRightsOnFolder()
		{
			var allUsers = Roles.AllUsers;
			finex.Subject.Module.RecordManagementUI.SpecialFolders.IncomingDocumentsfinex.AccessRights.Grant(allUsers, DefaultAccessRightsTypes.Read);
			finex.Subject.Module.RecordManagementUI.SpecialFolders.IncomingDocumentsfinex.AccessRights.Save();
			InitializationLogger.Debug("Выданы права на вычисляемую папку 'Входящие документы'");
			finex.Subject.Module.RecordManagementUI.SpecialFolders.OutgoingDocumentsfinex.AccessRights.Grant(allUsers, DefaultAccessRightsTypes.Read);
			finex.Subject.Module.RecordManagementUI.SpecialFolders.OutgoingDocumentsfinex.AccessRights.Save();
			InitializationLogger.Debug("Выданы права на вычисляемую папку 'Исходящие документы'");
			finex.Subject.Module.RecordManagementUI.SpecialFolders.InternalDocumentsfinex.AccessRights.Grant(allUsers, DefaultAccessRightsTypes.Read);
			finex.Subject.Module.RecordManagementUI.SpecialFolders.InternalDocumentsfinex.AccessRights.Save();
			InitializationLogger.Debug("Выданы права на вычисляемую папку 'Внутренние документы'");
			
		}
		
		/// <summary>
		/// Создание роли "Руководитель юр. отдела"
		/// </summary>
		/// <param name="roleName">Название роли.</param>
		/// <param name="roleDescription">Описание роли.</param>
		/// <param name="roleGuid">Guid роли. Игнорирует имя.</param>
		/// <returns>Новая роль.</returns>
		[Public]
		public static IRole CreateRole(string roleName, string roleDescription, Guid roleGuid)
		{
			InitializationLogger.DebugFormat("Init: Create Role {0}", roleName);
			var role = Roles.GetAll(r => r.Sid == roleGuid).FirstOrDefault();
			
			if (role == null)
			{
				role = Roles.Create();
				role.Name = roleName;
				role.Description = roleDescription;
				role.Sid = roleGuid;
				role.IsSystem = true;
				role.Save();
			}
			else
			{
				if (role.Name != roleName)
				{
					InitializationLogger.DebugFormat("Role '{0}'(Sid = {1}) renamed as '{2}'", role.Name, role.Sid, roleName);
					role.Name = roleName;
					role.Save();
				}
				if (role.Description != roleDescription)
				{
					InitializationLogger.DebugFormat("Role '{0}'(Sid = {1}) update Description '{2}'", role.Name, role.Sid, roleDescription);
					role.Description = roleDescription;
					role.Save();
				}
			}
			return role;
		}
	}
}

		// Объявление константы
		public static class Initialize
		{
			// При создании нового вида гуид для него надо сгенерировать.
			public static readonly Guid FinanceArchiveGuid = Guid.Parse("428a9e4e-a375-400f-8dc2-ab3f8d1815f8");
		}
