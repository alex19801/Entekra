using Entekra.Data.Constants;
using Entekra.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Entekra.Data.Repositories
{
    public class ApiRepository : IApiRepository
    {
        private ILogger _logger;

        public ApiRepository(ILogger<ApiRepository> logger)
        {
            _logger = logger;
        }
        public async Task<List<ProjectDto>> GetProjects()
        {
            var res = new List<ProjectDto>();
            try
            {
                var httpClient = new HttpClient();
                if (string.IsNullOrEmpty(AppConfig.Urls.ProjectsUrl))
                {
                    _logger.LogError("ProjectsUrl is not configured");
                    return res;
                }
                var response = await httpClient.GetAsync(AppConfig.Urls.ProjectsUrl);

                var content = response.Content;

                //get the json result from your api
                var result = await content.ReadAsStringAsync();
                res = JsonConvert.DeserializeObject<List<ProjectDto>>(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "API repository can't get projects");
            }

            res.Where(project => AppConfig.ProjectIdsToSkip.Contains(project.Id)).ToList();

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<List<IssueDto>> GetIssues(int projectId, string bookmark = null)
        {
            var resDto = new IssueListDto();

            if (string.IsNullOrEmpty(AppConfig.Urls.IssuessUrl))
            {
                _logger.LogError("AppConfig.Urls.IssuessUrl is not configured");
                return null;
            }

            var url = string.Format(AppConfig.Urls.IssuessUrl, projectId);

            // add bookmark if exist
            if (!string.IsNullOrWhiteSpace(bookmark))
            {
                url += $"&bookmark={bookmark}";
            }

            try
            {
                var result = await GetResponseByUrl(url);
                resDto = JsonConvert.DeserializeObject<IssueListDto>(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "API repository can't get Issues by projectId: {0}", projectId);
                return null;
            }

            // if item list is not null get next bookmark 
            if (resDto?.IssueList.Count > 0)
            {
                resDto.IssueList.AddRange(await GetIssues(projectId, resDto.NextBookmark));
            }
            return resDto?.IssueList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<List<CheckListDto>> GetCheckLists(int projectId, string bookmark = null)
        {
            ChecklistsDto resDto;
            if (string.IsNullOrEmpty(AppConfig.Urls.CheckListUrl))
            {
                _logger.LogError("CheckListUrl is not configured");
                return null;
            }
            var url = string.Format(AppConfig.Urls.CheckListUrl, projectId);

            // add bookmark if exist
            if (!string.IsNullOrWhiteSpace(bookmark))
            {
                url += $"&bookmark={bookmark}";
            }

            try
            {
                var result = await GetResponseByUrl(url);
                resDto = JsonConvert.DeserializeObject<ChecklistsDto>(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "API repository can't get CheckList by projectId: {0}", projectId);
                resDto = null;
            }


            // if item list is not null get next bookmark 
            if (resDto?.Checklists.Count > 0)
            {
                resDto.Checklists.AddRange(await GetCheckLists(projectId, resDto.NextBookmark));
            }

            return resDto?.Checklists;
        }

        private async Task<string> GetResponseByUrl(string url)
        {
            string res = null;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(url))
                    {
                        //get the json result from your api
                        res = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception e)
            {

            }
            return res;
        }

    }
}
