using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entekra.Data.Models
{
    public abstract partial class ReportFormStauses
    {
        public string ProjectName { get; set; }
        public int Open { get; set; }
        public int Closed { get; set; }
    }

    public abstract partial class ReportTimeExpiredForms
    {
        public string ProjectName { get; set; }
        public int Ok { get; set; }
        public int Bad { get; set; }
    }

    public partial class ReportFormStausesChangeRequest : ReportFormStauses  { }
    public partial class ReportFormStausesNonConformanceReport : ReportFormStauses { }

    public partial class ReportTimeExpiredFormsChangeRequest : ReportTimeExpiredForms { }
    public partial class ReportTimeExpiredFormsNonConformanceReport : ReportTimeExpiredForms { }

}
