using Entekra.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Entekra.Data.Repositories
{
    public interface IApiRepository
    {
        Task<List<ProjectDto>> GetProjects();

        Task<List<IssueDto>> GetIssues(int projectId, string bookmark = null);

        Task<List<CheckListDto>> GetCheckLists(int projectId, string bookmark = null);
    }
}
