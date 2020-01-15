using Entekra.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace EntekraWebJob
{
    public class Functions
    {
        private readonly IProjectService _projectService;
        private readonly ILogger _logger;
        public Functions(IProjectService projectService, ILogger<Functions> logger)
        {
            _logger = logger;
            _projectService = projectService;
            _logger.LogInformation("Start Project Service");
        }
           
        public void ProcessTimeTrigger([TimerTrigger("0 */30 * * * *", RunOnStartup = true)]TimerInfo timerInfo)
        {
            _logger.LogInformation("Start getting projects from API");

            _projectService.GetProjectsFromExternalRepository().Wait();

        }

    }
}
