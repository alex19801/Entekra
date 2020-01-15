using Entekra.Data.Models;
using Entekra.Data.Repositories;
using Entekra.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;

namespace Tests
{
    public class FunctionTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void MainTest()
        {
            //var context = new EntekraContext();
            //var dbRepo = new DbRepository(context);
            //var apiLogger = new Mock<ILogger<ApiRepository>>().Object;
            //var projectServiceLogger = new Mock<ILogger<ProjectService>>().Object;
            //var apiServiceLogger = new Mock<ILogger<ApiService>>().Object;
            //var apiRepo = new ApiRepository(apiLogger);
            //// var apiRepo = new OfflineApiRepository();
            //var apiService = new ApiService(apiRepo, apiServiceLogger);
            //var issueService = new ProjectService(dbRepo, apiService, apiRepo, projectServiceLogger);


            //issueService.GetProjectsFromExternalRepository().Wait();
        }
    }
}

public class MocLogger : ILogger
{
    public IDisposable BeginScope<TState>(TState state)
    {
        throw new NotImplementedException();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        throw new NotImplementedException();
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        // DO nothing
    }
}