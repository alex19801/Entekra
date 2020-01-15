using System;
using System.Collections.Generic;

namespace Entekra.Models
{
    public class ChecklistsDto
    {
        public List<CheckListDto> Checklists { get; set; }

        public string NextBookmark { get; set; }
    }

    public class CheckListDto
    {
        public int Id
        {
            get { return Convert.ToInt32(ChecklistId.ID); }
        }

        public ChecklistID ChecklistId { get; set; }
        public string ChecklistName { get; set; }
        public string ChecklistNumber { get; set; }
        public bool? Closed { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public bool? IsDeleted { get; set; }
        public List<ExtensionsDataListDto> ExtensionsDataList { get; set; }
    }

    public class ChecklistID : UnicId
    {
    }
}
