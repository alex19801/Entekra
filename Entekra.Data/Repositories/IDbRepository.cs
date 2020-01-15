using Entekra.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Entekra.Data.Repositories
{
    public interface IDbRepository
    {
        Task<List<Project>> GetProjects();

        Task<List<Issue>> GetIssues(int projectId);

        Task<List<Checklist>> GetCheckLists(int projectId);
        Task<List<Expirated24HChecklist>> GetCalculatedExpirated24HChecklist();
        Task<List<Expirated24HChecklist>> GetExpirated24HChecklist();
        Task<Project> AddProject(Project newProject);
        Task<List<Project>> AddProjects(List<Project> newProject, bool isRemoveBeforeInserting = true);
        Task<Expirated24HChecklist> AddExpirated24HChecklist(Expirated24HChecklist newExpirated24HChecklist);

        Task ClearProjects();
    }
}
