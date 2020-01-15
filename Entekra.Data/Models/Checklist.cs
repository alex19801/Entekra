using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entekra.Data.Models
{
    public partial class Checklist
    {
        public Checklist()
        {
            ExtensionsDataList = new HashSet<ExtensionsDataList>();
        }

        public int ChecklistId { get; set; }
        public int ChecklistExternalId { get; set; }
        public string ChecklistName { get; set; }
        public string ChecklistNumber { get; set; }
        public bool? Closed { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public bool? IsDeleted { get; set; }
        public int? ProjectId { get; set; }

        public virtual Project Project { get; set; }
        public virtual ICollection<ExtensionsDataList> ExtensionsDataList { get; set; }
    }
}
