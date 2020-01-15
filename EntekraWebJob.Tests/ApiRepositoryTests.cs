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
    public class ApiRepositoryTests
    {
        private readonly int projectId = 72324;
        private readonly int fakeProjectId = 123456;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetProjects_ShouldReturnValue()
        {
            var mockLogger = new Mock<ILogger<ApiRepository>>();
            var apiRepository = new ApiRepository(mockLogger.Object);
            var projects = await apiRepository.GetProjects();
        }

        [Test]
        public async Task GetIssues_ShouldReturnValue()
        {
            var mockLogger = new Mock<ILogger<ApiRepository>>();
            var apiRepository = new ApiRepository(mockLogger.Object);
            var issues = await apiRepository.GetIssues(projectId);
        }

        [Test]
        public async Task GetCheckList_ShouldReturnValue()
        {
            var mockLogger = new Mock<ILogger<ApiRepository>>();
            var apiRepository = new ApiRepository(mockLogger.Object);
            var checkLists = await apiRepository.GetCheckLists(projectId);
        }

    }
}