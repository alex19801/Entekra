using System;
using System.Collections.Generic;

namespace Entekra.Models
{
    public class ProjectDto
    {
        public int Id
        {
            get { return Convert.ToInt32(ProjectId.ID); }
        }
        public ProjectId ProjectId { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }

        public ICollection<CheckListDto> Checklists { get; set; }
        public ICollection<IssueDto> Issues { get; set; }
    }

    public class UnicId
    {
        public int Id
        {
            get { return Convert.ToInt32(ID); }
        }
        public int ID { get; set; }
    }

    public class ProjectId : UnicId
    {
    }
}
