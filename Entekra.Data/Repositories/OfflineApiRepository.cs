using Entekra.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Entekra.Data.Repositories
{
    public partial class OfflineApiRepository : IApiRepository
    {
      
        public async Task<List<ProjectDto>> GetProjects()
        {
            var res = new List<ProjectDto>();
            try
            {
                res = JsonConvert.DeserializeObject<List<ProjectDto>>(ProjectsRes);
            }
            catch (Exception e)
            {

            }
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

            try
            {
                resDto = JsonConvert.DeserializeObject<IssueListDto>(IssuessRes);
            }
            catch (Exception e)
            {
                resDto = null;
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
            try
            {
                resDto = JsonConvert.DeserializeObject<ChecklistsDto>(CheckListRes);
            }
            catch (Exception e)
            {
                resDto = null;
            }
            return resDto?.Checklists;
        }
    }
}
