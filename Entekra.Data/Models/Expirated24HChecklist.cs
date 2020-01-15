using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entekra.Data.Models
{
    public partial class Expirated24HChecklist
    {
        public int Expirated24HChecklistId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int ProjectExternalId { get; set; }
        public int ChecklistExternalId { get; set; }
    }
}
