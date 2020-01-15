using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entekra.Data.Models
{
    public partial class Project
    {
        public Project()
        {
            Checklist = new HashSet<Checklist>();
            Issues = new HashSet<Issue>();
        }

        public int ProjectId { get; set; }
        public int ProjectExternalId { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }

        public virtual ICollection<Checklist> Checklist { get; set; }
        public virtual ICollection<Issue> Issues { get; set; }
    }
}
