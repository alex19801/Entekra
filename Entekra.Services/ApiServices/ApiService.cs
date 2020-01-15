using Entekra.Data.Constants;
using Entekra.Data.Repositories;
using Entekra.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entekra.Services
{
    public class ApiService : IApiService
    {
        private readonly IApiRepository _apiRepository;
        private readonly ILogger<ApiService> _logger;

        public ApiService(IApiRepository apiRepository, ILogger<ApiService> logger)
        {
            _apiRepository = apiRepository;
            _logger = logger;
        }
        public async Task<List<ProjectDto>> GetProjects()
        {
            var projects = await _apiRepository.GetProjects();

            return projects;
        }

        public async Task<List<ProjectDto>> GetProjectsWithIssuesAndChecklist()
        {
            _logger.Log(LogLevel.Information, $"Getting projects...");
#if DEBUG
            var projects = (await _apiRepository.GetProjects()).Take(6).ToList(); 
#else
            var projects = await _apiRepository.GetProjects();
#endif

            // remove project by filter
            projects = projects.Where(p => !AppConfig.ProjectIdsToSkip.Contains(p.Id)).ToList();
            _logger.Log(LogLevel.Information, $"Got {projects.Count} projects.");
            
            for (int i = 0; i < projects.Count; i++)
            {
            
                _logger.Log(LogLevel.Information, $"Getting Issue and Checklist for project: \"{projects[i].Name}\" ...");
                if (AppConfig.ProjectIdsToSkip.Contains(projects[i].Id)) { continue; }
                var issues = await _apiRepository.GetIssues(projects[i].Id);
                var checkList = await _apiRepository.GetCheckLists(projects[i].Id);
                projects[i].Issues = issues;
                projects[i].Checklists = checkList;
                _logger.Log(LogLevel.Information, $"  Added Issues:{issues.Count}, Checklists:{checkList.Count} to project: \"{projects[i].Name}\".");
            }

            return projects;
        }
    }
}
