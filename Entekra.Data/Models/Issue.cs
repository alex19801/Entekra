using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entekra.Data.Models
{
    public partial class Issue
    {
        public int IssueId { get; set; }
        public int IssueExternalId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string InspectionType { get; set; }
        public bool? IsDeleted { get; set; }
        public string IssueNumber { get; set; }
        public int ProjectId { get; set; }
        public int? ExtensionsDataListId { get; set; }

        public virtual Project Project { get; set; }
        public virtual ExtensionsDataList ExtensionsDataList { get; set; }
    }
}
