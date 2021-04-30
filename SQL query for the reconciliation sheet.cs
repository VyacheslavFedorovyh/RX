DECLARE @rework datetime;

Begin
	SET @rework = (Select MAX(Completed) From Sungero_WF_Assignment Where Task = @TaskID AND BlockId = 24 AND Completed IS NOT NULL)	
	 
	Select 
		Job.BlockId,
		CASE 
			WHEN Job.CompletedBy IS NULL
			THEN TitlePerf.Name + ' ' + Perf.Name
		ELSE 
			CASE 
				WHEN Job.CompletedBy = Job.Performer
				THEN TitlePerf.Name + ' ' + Perf.Name
				ELSE TitleCompl.Name + ' ' + Compl.Name + ' за ' + TitlePerf.Name + ' ' + Perf.Name
			END
		END As Isp,
		FORMAT(Job.Created, 'dd.MM.yyyy') As DateCreated,
		FORMAT(Job.Completed, 'dd.MM.yyyy') As DateCompleted,
		CASE 
			WHEN Job.Result = 'Complete'
				THEN CASE WHEN JobText.Body = 'Согласовать.' THEN 'Согласованно.' ELSE 'Согласованно / ' + JobText.Body END 
			WHEN Job.Result = 'Review'
				THEN 'На даработку / ' + JobText.Body 
			ELSE 'Отказанно / ' + JobText.Body 	
		END As Txt
	From 
		Sungero_WF_Assignment as Job
				
	INNER JOIN Sungero_WF_Text as JobText ON JobText.Assignment = Job.Id
	INNER JOIN Sungero_Core_Recipient as Perf ON Perf.Id = Job.Performer
	INNER JOIN Sungero_Company_JobTitle as TitlePerf ON TitlePerf.Id = Perf.JobTitle_Company_Sungero
	LEFT JOIN Sungero_Core_Recipient as Compl ON Compl.Id = Job.CompletedBy
	INNER JOIN Sungero_Company_JobTitle as TitleCompl ON TitleCompl.Id = Compl.JobTitle_Company_Sungero

	Where
		Job.Task = @TaskID AND	
		--	Выборка только 4 заданий по ID блока
		Job.BlockId IN (20, 25, 26, 27) AND
		Job.Completed IS NOT NULL AND
		(
			(@rework IS NOT NULL AND Job.Completed >= @rework) 
			OR 
			@rework IS NULL
		)
END