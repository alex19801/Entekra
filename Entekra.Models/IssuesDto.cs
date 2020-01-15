using System;
using System.Collections.Generic;

namespace Entekra.Models
{
    public class IssueListDto
    {
        public List<IssueDto> IssueList { get; set; }

        public string NextBookmark { get; set; }
    }

    public class IssueDto
    {
        public int Id
        {
            get { return Convert.ToInt32(IssueId.ID); }
        }
        public IssueID IssueId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string InspectionType { get; set; }
        public bool? IsDeleted { get; set; }
        public string IssueNumber { get; set; }
    }

    public class IssueID : UnicId
    {
    }
}
