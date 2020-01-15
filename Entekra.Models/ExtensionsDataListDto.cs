using System;
using System.Collections.Generic;

namespace Entekra.Models
{
    public class ExtensionsDataListDto
    {
        //public int ExtensionsDataListId { get; set; }
        public List<UnicId> AttachedIssues { get; set; }
        public string Comment { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
