using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using finex.Subject.SupAgreement;

namespace finex.Subject.Client
{
	partial class SupAgreementActions
	{
		public virtual void ChangeDocumentTypefinex(Sungero.Domain.Client.ExecuteActionArgs e)
		{
			// Для смены типа необходимо отменить регистрацию.
			if (_obj.RegistrationState == Sungero.Docflow.OfficialDocument.RegistrationState.Registered &&
			    _obj.DocumentKind.NumberingType != Sungero.Docflow.DocumentKind.NumberingType.Numerable ||
			    _obj.RegistrationState == Sungero.Docflow.OfficialDocument.RegistrationState.Reserved)
			{
				// Используем диалоги, чтобы хинт не пробрасывался в задачу, в которую он вложен.
				Dialogs.ShowMessage(Sungero.Docflow.SimpleDocuments.Resources.NeedCancelRegistration,
				                    MessageType.Error);
				return;
			}
			else if (Functions.SupAgreement.Remote.AnyApprovalTasksWithCurrentDocument(_obj))
			{
				// Для смены типа необходимо остановить все активные задачи согласования по регламенту.
				Dialogs.ShowMessage(Sungero.Docflow.SimpleDocuments.Resources.NeedAbortApproval,
				                    MessageType.Error);
				return;
			}
			else
			{
				var types = new List<Sungero.Domain.Shared.IEntityInfo>();
				types.Add(Sungero.Docflow.Addendums.Info);
				types.Add(Sungero.Docflow.Memos.Info);
				types.Add(Sungero.Meetings.Minuteses.Info);
				types.Add(Sungero.Docflow.SimpleDocuments.Info);
				types.Add(Sungero.FinancialArchive.ContractStatements.Info);
				types.Add(Sungero.Contracts.IncomingInvoices.Info);
				types.Add(finex.Subject.Contracts.Info);
				types.Add(Sungero.Projects.ProjectDocuments.Info);
				types.Add(Sungero.RecordManagement.IncomingLetters.Info);
				types.Add(Sungero.RecordManagement.OutgoingLetters.Info);
				types.Add(Sungero.RecordManagement.Orders.Info);
				types.Add(Sungero.Docflow.CounterpartyDocuments.Info);
				
				Functions.Module.ChangeDocumentType(_obj, types, e);
			}
		}
	}
}


	partial class SupAgreementFunctions
	{
		/// <summary>
		/// Определить, есть ли активные задачи согласования по регламенту документа.
		/// </summary>
		/// <returns>True, если есть.</returns>
		[Remote]
		public bool AnyApprovalTasksWithCurrentDocument()
		{
			var anyTasks = false;

			AccessRights.AllowRead(
				() =>
				{
					anyTasks = finex.Entity.Tasks.GetAll()
						.Where(t => t.Status == finex.Entity.Task.Status.InProcess ||
						       t.Status == finex.Entity.Task.Status.Suspended)
						.Where(t => t.AttachmentDetails.Any(att => att.AttachmentId == _obj.Id))
						.Any();					
				});
			
			return anyTasks;
		}
	}

	/// <summary>
		/// Сменить тип документа.
		/// </summary>
		/// <param name="document">Документ-источник.</param>
		/// <param name="types">Типы документов, на которые можно сменить.</param>
		/// <param name="e">Аргументы действия для вывода хинтов.</param>
		public virtual void ChangeDocumentType(Sungero.Docflow.IOfficialDocument document, List<Sungero.Domain.Shared.IEntityInfo> types, Sungero.Domain.Client.ExecuteActionArgs e)
		{
			// Запретить смену типа, если документ или его тело заблокировано.
			var isCalledByDocument = CallContext.CalledDirectlyFrom(Sungero.Docflow.OfficialDocuments.Info);
			if (isCalledByDocument && Sungero.Docflow.PublicFunctions.Module.IsLockedByOther(document) ||
			    !isCalledByDocument && Sungero.Docflow.PublicFunctions.Module.IsLocked(document)  ||
			    VersionIsLocked(document.Versions.ToList()))
			{
				Dialogs.ShowMessage(Sungero.Docflow.ExchangeDocuments.Resources.ChangeDocumentTypeLockError,
				                    MessageType.Error);
				return;
			}
			
			// Открыть диалог по смене типа.
			var title = Sungero.Docflow.ExchangeDocuments.Resources.TypeChange;
			var dialog = Dialogs.CreateSelectTypeDialog(title, types.ToArray());
			if (dialog.Show() == DialogButtons.Ok)
			{			
				var convertedObj = document.ConvertTo(dialog.SelectedType);
				convertedObj.Show();
				e.CloseFormAfterAction = true;
			}
		}
		
		/// <summary>
		/// Проверка заблокированности любой версии.
		/// </summary>
		/// <param name="versions">Список версий документа.</param>
		/// <returns>True, если заблокирована хотябы одна версия.</returns>
		public static bool VersionIsLocked(List<Sungero.Content.IElectronicDocumentVersions> versions)
		{
			foreach (var version in versions)
			{
				var lockInfo = version.Body != null ? Locks.GetLockInfo(version.Body) : null;
				var isLockedByOther = lockInfo != null && lockInfo.IsLocked;
				
				if (isLockedByOther)
					return true;
			}
			
			return false;
		}