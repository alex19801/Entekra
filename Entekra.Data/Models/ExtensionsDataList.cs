using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entekra.Data.Models
{
    public partial class ExtensionsDataList
    {
        public int ExtensionsDataListId { get; set; }
        public int ExtensionsDataListExternalId { get; set; }
        public string Comment { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int CheckListId { get; set; }

        public virtual Checklist CheckList { get; set; }

        [NotMapped]
        public virtual ICollection<int> AttachedIssueIds { get; set; }
        public virtual ICollection<Issue> AttachedIssues { get; set; }
    }
}
