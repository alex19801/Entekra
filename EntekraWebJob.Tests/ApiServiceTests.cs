using Entekra.Data.Constants;
using Entekra.Data.Repositories;
using Entekra.Services;
using EntekraWebJob;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Tests
{
    public class ApiServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetProjects_ShouldReturnValue()
        {
            AppConfig.Urls.ProjectsUrl = "https://field.dalux.com/service/APIv2/FieldRestService.svc/projects?key=b4d72655-6542-44de-9469-c80153efbe2c\\";
            AppConfig.Urls.IssuessUrl = "";
            AppConfig.Urls.CheckListUrl = "";

            var loggerApiRepository = new Mock<ILogger<ApiRepository>>().Object;
            var loggerApiService = new Mock<ILogger<ApiService>>().Object;
            var apiRepository = new ApiRepository(loggerApiRepository);
            var apiService = new ApiService(apiRepository, loggerApiService);
            var projects = await apiService.GetProjects();

        }
    }
}