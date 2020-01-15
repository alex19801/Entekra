using Entekra.Data.Models;
using Entekra.Data.Repositories;
using Entekra.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entekra.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IApiService _apiService;
        private readonly IDbRepository _dbRepository;
        private readonly IApiRepository _apiRepository;
        private readonly ILogger _logger;

        public ProjectService(IDbRepository dbRepository, IApiService apiService, IApiRepository apiRepository, ILogger<ProjectService> logger)
        {
            _apiService = apiService;
            _dbRepository = dbRepository;
            _apiRepository = apiRepository;
            _logger = logger;
        }
        public async Task<List<ProjectDto>> GetProjects()
        {
            var projects = await _apiRepository.GetProjects();

            return projects;
        }

        public async Task<List<IssueDto>> GetIssuesByProjectId(int projectId)
        {
            var issues = await _apiRepository.GetIssues(72324);
            return issues;
        }

        public Task<List<IssueDto>> GetCheckListByProjectId(int projectId)
        {
            return null;
        }

        public async Task<List<ProjectDto>> GetProjectsWithIssuesAndChecklist()
        {
            var projects = await _apiService.GetProjectsWithIssuesAndChecklist();

            return projects;
        }

        public async Task SetExpirated24HChecklist()
        {
            var calculatedExpirated24HChecklists = await _dbRepository.GetCalculatedExpirated24HChecklist();
            var expirated24HChecklists = await _dbRepository.GetCalculatedExpirated24HChecklist();

            foreach (var calculatedExpirated24HChecklist in calculatedExpirated24HChecklists)
            {
                if (!expirated24HChecklists.Contains(calculatedExpirated24HChecklist))
                {
                    _logger.LogWarning($"Adding new expirated checklist. Project:{calculatedExpirated24HChecklist.ProjectExternalId} CheckListId:{calculatedExpirated24HChecklist.ChecklistExternalId}");
                    await _dbRepository.AddExpirated24HChecklist(calculatedExpirated24HChecklist);
                }
            }

        }

        public async Task GetProjectsFromExternalRepository()
        {
            // restrict only one project fortesting
            var apiProjects = (await GetProjectsWithIssuesAndChecklist());
            if (apiProjects == null || apiProjects.Count == 0)
            {
                _logger.LogWarning("Try Update empty list of projects");
                return;
            }

            _logger.LogInformation("Remove old saved data (Tables: Issue, ExtensionsDataList, Checklist, Project)");
            await _dbRepository.ClearProjects();

            var projects = new List<Project>();
            for (var i = 0; i < apiProjects.Count; i++)
            {
                //create new project
                var currentProj = apiProjects[i];
                var issues = MapIssues(currentProj).OrderBy(issue => issue.IssueExternalId).ToList();
                List<Checklist> checkLists = MappCheckList(currentProj, ref issues);
                var newProject = new Project
                {
                    ProjectExternalId = currentProj.Id,
                    Name = currentProj.Name,
                    Number = currentProj.Number,
                    Issues = issues,
                    Checklist = checkLists
                };

                projects.Add(newProject);

            }
            _logger.LogInformation("Save projects to DataBase...");
            try
            {
                await _dbRepository.AddProjects(projects);
                _logger.LogInformation("Projects saved to DB.");

                _logger.LogInformation("Calcuate 24H Expirated Checklists and saved to DB.");
                await SetExpirated24HChecklist();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private static List<Checklist> MappCheckList(ProjectDto currentProj, ref List<Issue> issues)
        {
            var checkLists = new List<Checklist>();

            foreach (var ckl in currentProj.Checklists)
            {

                List<ExtensionsDataList> extensionsDataList = MappExtensionDataList(ckl);

                // attach issues and theirs extensionsDataList
                foreach (var edl in extensionsDataList)
                {
                    var attachedIssueIds = edl.AttachedIssueIds;
                    if (attachedIssueIds != null && attachedIssueIds.Count > 0)
                    {
                        var attachedIssues = issues.Where(i => attachedIssueIds.Contains(i.IssueExternalId)).ToList();

                        foreach (var issue in attachedIssues)
                        {
                            issue.ExtensionsDataList = edl;
                        };
                    }
                }

                checkLists.Add(new Checklist
                {
                    ChecklistExternalId = ckl.Id,
                    ChecklistName = ckl.ChecklistName,
                    ChecklistNumber = ckl.ChecklistNumber,
                    Closed = ckl.Closed,
                    CreatedDateTime = ckl.CreatedDateTime,
                    ExtensionsDataList = extensionsDataList,
                    IsDeleted = ckl.IsDeleted,
                    ProjectId = currentProj.Id
                });
            };

            return checkLists;
        }

        private static List<ExtensionsDataList> MappExtensionDataList(CheckListDto checkListDto)
        {
            var extensionsDataList = new List<ExtensionsDataList>();
            if (checkListDto.ExtensionsDataList.Count > 0)
            {
                checkListDto.ExtensionsDataList.ForEach(edl =>
                {
                    var attachedIssueIds = edl.AttachedIssues.Select(issue => issue.Id).ToList();

                    var extensionsDataListItem = new ExtensionsDataList
                    {
                        AttachedIssueIds = attachedIssueIds,
                        CheckListId = checkListDto.Id,
                        Comment = edl.Comment,
                        Name = edl.Name,
                        Value = edl.Value
                    };
                    extensionsDataList.Add(extensionsDataListItem);
                });
            }

            return extensionsDataList;
        }

        private static List<Issue> MapIssues(ProjectDto currentProj)
        {
            List<Issue> issues = new List<Issue>();
            currentProj.Issues.ToList().ForEach(item =>
            {
                issues.Add(new Issue
                {
                    IssueExternalId = item.Id,
                    CreatedDateTime = item.CreatedDateTime,
                    //ExtensionsDataListId - will be filled letter
                    InspectionType = item.InspectionType,
                    IssueNumber = item.IssueNumber,
                    IsDeleted = item.IsDeleted,
                    ProjectId = currentProj.Id,
                });
            });

            return issues;
        }
    }
}
