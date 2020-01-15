using Entekra.Data.Constants;
using Entekra.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entekra.Data.Repositories
{
    public class DbRepository : IDbRepository
    {
        private readonly EntekraContext _context;

        public DbRepository(EntekraContext context)
        {
            _context = context;
        }

        public async Task<List<Checklist>> GetCheckLists(int projectId)
        {
            return await _context.Checklist.Where(c => c.ProjectId == projectId).ToListAsync();
        }

        public async Task<List<Issue>> GetIssues(int projectId)
        {
            return await _context.Issue.Where(c => c.ProjectId == projectId).ToListAsync();
        }

        public async Task<List<Project>> GetProjects()
        {
            return await _context.Project.ToListAsync();
        }

        public async Task<List<Expirated24HChecklist>> GetCalculatedExpirated24HChecklist()
        {
            return await _context.Checklist.Where(cl => cl.IsDeleted != true
                    && cl.ChecklistName == "QA - NCR/CR Form"
                    && !cl.ExtensionsDataList.Any(edl => edl.Name == "Investigation Required?" && !string.IsNullOrEmpty(edl.Value)))
                .Select(cl =>
                new Expirated24HChecklist
                {
                    ChecklistExternalId = cl.ChecklistExternalId
                    ,
                    ProjectExternalId = cl.Project.ProjectExternalId,
                    CreatedDateTime = DateTime.Now
                }).ToListAsync();
        }

        public async Task<List<Expirated24HChecklist>> GetExpirated24HChecklist()
        {
            return await _context.Expirated24HChecklist.ToListAsync();
        }


        public async Task ClearProjects()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                await _context.Database.ExecuteSqlRawAsync(DbViews.RemoveProjects);
                await transaction.CommitAsync();
            }
        }

        public async Task<Project> AddProject(Project newProject)
        {
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    await _context.Project.AddAsync(newProject);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return newProject;
        }

        public async Task<Expirated24HChecklist> AddExpirated24HChecklist(Expirated24HChecklist newExpirated24HChecklist)
        {
            try
            {
                await _context.Expirated24HChecklist.AddAsync(newExpirated24HChecklist);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return newExpirated24HChecklist;
        }

        public async Task<List<Project>> AddProjects(List<Project> newProjects, bool isRemoveBeforeInserting = true)
        {
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    if (isRemoveBeforeInserting)
                    {
                        await _context.Database.ExecuteSqlRawAsync(DbViews.RemoveProjects);
                    }
                    await _context.Project.AddRangeAsync(newProjects);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return newProjects;
        }
    }
}
