using AutoMapper;
using Entekra.Data.Models;

namespace Entekra.Models.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectDto, Project>();
            CreateMap<Issue, IssueDto>();
            CreateMap<IssueDto, Issue>();
            CreateMap<Checklist, CheckListDto>();
            CreateMap<CheckListDto, Checklist>();
            CreateMap<ExtensionsDataList, ExtensionsDataListDto>();
            CreateMap<ExtensionsDataListDto, ExtensionsDataList>();
        }
    }
}
