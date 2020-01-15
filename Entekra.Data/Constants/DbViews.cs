namespace Entekra.Data.Constants
{
    public static class DbViews
    {
        private static class ViewNames
        {
            public static string NonConformanceReport = "Non Conformance Report";
            public static string ChangeRequest = "Change Request";
        }
        private static class Purpose
        {
            public static string NonConformanceReport = "Non Conformance Report";
            public static string ChangeRequest = "Change Request";
        }

        private static string NormalizeViewName(string purpose)
        {
            return purpose.Replace(" ", "").Replace("-", "");
        }

        private static string ReportFormStausesByPurpose(string purpose)
        {
            var viewName = $"ReportFormStauses_{NormalizeViewName(purpose)}";
            return $@"
CREATE VIEW {viewName} AS 
SELECT ProjectName, 
    SUM(CASE WHEN mainstatus = 'Open' THEN 1 ELSE 0 end) AS 'Open', 
    SUM(CASE WHEN MainStatus = 'Closed' THEN 1 ELSE 0 end) AS 'Closed' 
    FROM 
  ({FormStausesByPurpose(purpose)}) AS a
  GROUP BY ProjectName";
        }
        private static string ReportTimeExpiredFormsByPurpose(string purpose)
        {
            var viewName = $"ReportTimeExpiredForms_{NormalizeViewName(purpose)}";
            return $@"
CREATE VIEW {viewName} AS 
SELECT ProjectName, 
    SUM(CASE WHEN mainstatus = 'Bad' THEN 1 ELSE 0 end) AS 'Bad', 
    SUM(CASE WHEN MainStatus = 'Ok' THEN 1 ELSE 0 end) AS 'Ok' 
    FROM 
  ({TimeExpiredFormsByPurpose(purpose)}) AS a
  GROUP BY ProjectName";
        }
        private static string ReportOpenFormsByPurpose(string purpose)
        {
            var viewName = $"ReportOpenForms_{NormalizeViewName(purpose)}";
            return $@"
CREATE VIEW {viewName} AS 
SELECT ProjectName, 
    SUM(CASE WHEN TechnicalOrEngineering = 'Open' THEN 1 ELSE 0 end) AS 'TechnicalOrEngineering'
    ,SUM(CASE WHEN [Site] = 'Open' THEN 1 ELSE 0 end) AS 'Site'
    ,SUM(CASE WHEN Production = 'Open' THEN 1 ELSE 0 end) 'Production'
    ,SUM(CASE WHEN mainstatus = 'Open' THEN 1 ELSE 0 end) AS 'Open-Total'
    --SUM(CASE WHEN MainStatus = 'Closed' THEN 1 ELSE 0 end) AS 'Closed' 
    FROM 
  ({OpenFormsByPurpose(purpose)}) AS a
  GROUP BY ProjectName";
        }

        private static string ReportAreaResponsibleForIssueByPurpose(string purpose)
        {
            var viewName = $"ReportAreaResponsibleForIssue_{NormalizeViewName(purpose)}";
            return $@"
CREATE VIEW {viewName} AS 
SELECT ProjectName 
	,SUM(CASE WHEN ExtensionsDataList_Value = 'Change Request' THEN a.Count ELSE 0 end) AS 'Change Request'
	,SUM(CASE WHEN ExtensionsDataList_Value = 'Client Issue' THEN a.Count ELSE 0 end) AS 'Client Issue'
	,SUM(CASE WHEN ExtensionsDataList_Value = 'Client Issue.' THEN a.Count ELSE 0 end) AS 'Client Issue.'
	,SUM(CASE WHEN ExtensionsDataList_Value = 'Design' THEN a.Count ELSE 0 end) AS 'Design'
	,SUM(CASE WHEN ExtensionsDataList_Value = 'Erector' THEN a.Count ELSE 0 end) AS 'Erector'
	,SUM(CASE WHEN ExtensionsDataList_Value = 'Floor Engineering' THEN a.Count ELSE 0 end) AS 'Floor Engineering'
	,SUM(CASE WHEN ExtensionsDataList_Value = 'Floor Production' THEN a.Count ELSE 0 end) AS 'Floor Production'
	,SUM(CASE WHEN ExtensionsDataList_Value = 'Loading' THEN a.Count ELSE 0 end) AS 'Loading'
	,SUM(CASE WHEN ExtensionsDataList_Value = 'Loading, Stair Production' THEN a.Count ELSE 0 end) AS 'Loading, Stair Production'
	,SUM(CASE WHEN ExtensionsDataList_Value = 'Roof Engineering' THEN a.Count ELSE 0 end) AS 'Roof Engineering'
	,SUM(CASE WHEN ExtensionsDataList_Value = 'Roof Production' THEN a.Count ELSE 0 end) AS 'Roof Production'
	,SUM(CASE WHEN ExtensionsDataList_Value = 'Stair Engineering' THEN a.Count ELSE 0 end) AS 'Stair Engineering'
	,SUM(CASE WHEN ExtensionsDataList_Value = 'Stair Engineering, Stair Production' THEN a.Count ELSE 0 end) AS 'Stair Engineering, Stair Production'
	,SUM(CASE WHEN ExtensionsDataList_Value = 'Stair Production' THEN a.Count ELSE 0 end) AS 'Stair Production'
	,SUM(CASE WHEN ExtensionsDataList_Value = 'Survey Team' THEN a.Count ELSE 0 end) AS 'Survey Team'
	,SUM(CASE WHEN ExtensionsDataList_Value = 'Survey Team, Client Issue.' THEN a.Count ELSE 0 end) AS 'Survey Team, Client Issue.'
	,SUM(CASE WHEN ExtensionsDataList_Value = 'Survey Team, Design, Wall Engineering' THEN a.Count ELSE 0 end) AS 'Survey Team, Design, Wall Engineering'
	,SUM(CASE WHEN ExtensionsDataList_Value = 'Wall Engineering' THEN a.Count ELSE 0 end) AS 'Wall Engineering'
	,SUM(CASE WHEN ExtensionsDataList_Value = 'Wall Engineering, Design' THEN a.Count ELSE 0 end) AS 'Wall Engineering, Design'
	,SUM(CASE WHEN ExtensionsDataList_Value = 'Wall Engineering, Wall Production' THEN a.Count ELSE 0 end) AS 'Wall Engineering, Wall Production'
	,SUM(CASE WHEN ExtensionsDataList_Value = 'Wall Production' THEN a.Count ELSE 0 end) AS 'Wall Production'
	,SUM(CASE WHEN ExtensionsDataList_Value = 'Wall Production, Loading, Erector' THEN a.Count ELSE 0 end) AS 'Wall Production, Loading, Erector'
    FROM 
  ({AreaResponsibleForIssueByPurpose(purpose)}) AS a
  GROUP BY ProjectName";
        }


        private static string FormStausesByPurpose(string purpose)
        {
            return $@"SELECT 
      p.Name AS ProjectName
	  ,CASE
        WHEN c.IsDeleted IS NULL THEN NULL
	    WHEN (edl1.Value IS NULL OR edl2.Value IS NULL OR edl3.Value IS NULL OR
            edl1.Value ='' OR edl2.Value = '' OR edl3.Value = '') THEN 'Open'
	    ELSE 'Closed'
	  END as MainStatus
  FROM Project p
  LEFT JOIN Checklist c ON p.ProjectId = c.ProjectId AND c.ChecklistName = 'QA - NCR/CR Form'
  LEFT JOIN ExtensionsDataList edl1 ON edl1.CheckListId = c.ChecklistId AND (edl1.Name = 'Form Review Status - Technical:' OR edl1.Name = 'Form Review Status - Engineering:')
  LEFT JOIN ExtensionsDataList edl2 ON edl2.CheckListId = c.ChecklistId AND edl2.Name = 'Form Review Status - Site:'
  LEFT JOIN ExtensionsDataList edl3 ON edl3.CheckListId = c.ChecklistId AND edl3.Name = 'Form Review Status - Production:'
  LEFT JOIN ExtensionsDataList edl4 ON edl4.CheckListId = c.ChecklistId AND edl4.Name = 'Purpose:'

  WHERE 
    (c.IsDeleted = 0 OR c.IsDeleted IS NULL)
    AND edl4.Value = '{purpose}'";
        }

        private static string AreaResponsibleForIssueByPurpose(string purpose)
        {
            return $@"SELECT
      p.Name AS ProjectName

	  ,edl1.Value as ExtensionsDataList_Value
	  , COUNT (edl1.Name ) AS 'Count'
  FROM Project p
  LEFT JOIN Checklist c ON p.ProjectId = c.ProjectId AND (c.IsDeleted = 0 OR c.IsDeleted IS NULL) AND c.ChecklistName = 'QA - NCR/CR Form'
  LEFT JOIN ExtensionsDataList edl1 ON edl1.CheckListId = c.ChecklistId AND edl1.Name = 'INV - Area Responsible for Issue:'
  LEFT JOIN ExtensionsDataList edl4 ON edl4.CheckListId = c.ChecklistId AND edl4.Name = 'Purpose:'
  WHERE 
    edl4.Value = '{purpose}'
    Group by p.Name, edl1.Value
    ";
        }

        private static string OpenFormsByPurpose(string purpose)
        {
            return $@"SELECT 
      p.Name AS ProjectName
	  ,CASE WHEN (edl1.Value IS NULL OR edl1.Value ='') THEN 'Open' ELSE 'Closed' END as 'TechnicalOrEngineering'
	  ,CASE WHEN (edl2.Value IS NULL OR edl2.Value ='') THEN 'Open' ELSE 'Closed' END as 'Site'
	  ,CASE WHEN (edl3.Value IS NULL OR edl3.Value ='') THEN 'Open' ELSE 'Closed' END as 'Production'
	  ,CASE WHEN (edl1.Value IS NULL OR edl2.Value IS NULL OR edl3.Value IS NULL OR
            edl1.Value ='' OR edl2.Value = '' OR edl3.Value = '') THEN 'Open' ELSE 'Closed'
	  END as MainStatus
  FROM Project p
  LEFT JOIN Checklist c ON p.ProjectId = c.ProjectId AND c.ChecklistName = 'QA - NCR/CR Form' AND (c.IsDeleted = 0)
  LEFT JOIN ExtensionsDataList edl1 ON edl1.CheckListId = c.ChecklistId AND (edl1.Name = 'Form Review Status - Technical:' OR edl1.Name = 'Form Review Status - Engineering:')
  LEFT JOIN ExtensionsDataList edl2 ON edl2.CheckListId = c.ChecklistId AND edl2.Name = 'Form Review Status - Site:'
  LEFT JOIN ExtensionsDataList edl3 ON edl3.CheckListId = c.ChecklistId AND edl3.Name = 'Form Review Status - Production:'
  LEFT JOIN ExtensionsDataList edl4 ON edl4.CheckListId = c.ChecklistId AND edl4.Name = 'Purpose:'

  WHERE 
     edl4.Value = '{purpose}'";
        }

        private static string TimeExpiredFormsByPurpose(string purpose)
        {
            return $@"SELECT 
      p.Name AS ProjectName
	  ,c.ChecklistNumber
	  ,edl1.Name AS ExtensionsDataList_Name
	  ,edl1.Value as ExtensionsDataList_Value
	  ,c.CreatedDateTime
	  ,GETDATE() AS Now
	  ,DATEDIFF(hour,c.CreatedDateTime,GETDATE()) as DiffHours

	  ,CASE
		WHEN e24.Expirated24HChecklistId IS NOT NULL THEN 'Bad'
	    WHEN (edl1.Value IS NULL OR edl1.Value ='' ) AND DATEDIFF(hour,c.CreatedDateTime,GETDATE()) > 24 THEN 'Bad'
	    ELSE 'Ok'
	   END as MainStatus
  FROM Project p
  LEFT JOIN Checklist c ON p.ProjectId = c.ProjectId AND c.ChecklistName = 'QA - NCR/CR Form' AND (c.IsDeleted = 0 OR c.IsDeleted IS NULL)
  LEFT JOIN ExtensionsDataList edl1 ON edl1.CheckListId = c.ChecklistId AND edl1.Name = 'Investigation Required?'
  LEFT JOIN Expirated24HChecklist e24 ON e24.ChecklistExternalId = c.ChecklistExternalId AND e24.ProjectExternalId = p.ProjectExternalId
  LEFT JOIN ExtensionsDataList edl4 ON edl4.CheckListId = c.ChecklistId AND edl4.Name = 'Purpose:'

  WHERE 
    edl4.Value = '{purpose}'";
        }

        public static string ReportFormStauses_NonConformanceReport = ReportFormStausesByPurpose(Purpose.NonConformanceReport);

        public static string ReportFormStauses_ChangeRequest = ReportFormStausesByPurpose(Purpose.ChangeRequest);

        public static string ReportTimeExpiredForms_NonConformanceReport = ReportTimeExpiredFormsByPurpose(Purpose.NonConformanceReport);

        public static string ReportTimeExpiredForms_ChangeRequest = ReportTimeExpiredFormsByPurpose(Purpose.ChangeRequest);
        public static string ReportAreaResponsibleForIssue_NonConformanceReport = ReportAreaResponsibleForIssueByPurpose(Purpose.NonConformanceReport);
        public static string ReportAreaResponsibleForIssue_ChangeRequest = ReportAreaResponsibleForIssueByPurpose(Purpose.ChangeRequest);
        public static string ReportOpenForms_NonConformanceReport = ReportOpenFormsByPurpose(Purpose.NonConformanceReport);
        public static string ReportOpenForms_ChangeRequest = ReportOpenFormsByPurpose(Purpose.ChangeRequest);

        public static string DropAllViews = $@"
DECLARE @sql VARCHAR(MAX) = ''
        , @crlf VARCHAR(2) = CHAR(13) + CHAR(10) ;
SELECT @sql = @sql + 'DROP VIEW ' + QUOTENAME(SCHEMA_NAME(schema_id)) + '.' + QUOTENAME(v.name) +';' + @crlf
FROM   sys.views v
WHERE QUOTENAME(v.name) != '[database_firewall_rules]'
-- PRINT @sql;
EXEC(@sql);";

        public static string RemoveProjects = $@"DELETE FROM  Issue;
DELETE FROM  ExtensionsDataList;
DELETE FROM  Checklist;
DELETE FROM  Project;";
    }
}
