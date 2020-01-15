using System.Collections.Generic;

namespace Entekra.Data.Constants
{
    public static class AppConfig
    {
        public static class Urls
        {
            public static string ProjectsUrl = null;
            public static string IssuessUrl = null;
            public static string CheckListUrl = null;
        }

        public static List<int> ProjectIdsToSkip { get; set; }
        public static List<string> CheckListFormsToReport { get; set; }
    }
}
