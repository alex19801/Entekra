using AutoMapper;
using Entekra.Data.Constants;
using Entekra.Data.Models;
using Entekra.Data.Repositories;
using Entekra.Models.MappingProfiles;
using Entekra.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EntekraWebJob
{
    public class EntekraContextFactory : IDesignTimeDbContextFactory<EntekraContext>
    {
        public EntekraContext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(AppContext.BaseDirectory)
             .AddJsonFile("appsettings.json");

            var config = builder.Build();

            var connString = config.GetConnectionString("AppConnection");

            var optionsBuilder = new DbContextOptionsBuilder<EntekraContext>();
            optionsBuilder.UseSqlServer(connString);

            return new EntekraContext(optionsBuilder.Options);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new HostBuilder();

            builder.ConfigureWebJobs((context, b) =>
            {
                b.AddAzureStorageCoreServices();
                b.AddTimers();

                var connString = context.Configuration.GetConnectionString("AppConnection");

                b.Services.AddDbContext<EntekraContext>(o =>
                {
                    o.UseSqlServer(connString, bl => bl.MigrationsAssembly("Entekra.Data"));

                    o.EnableSensitiveDataLogging();
                });

                ConfigureEnvironment(context);

            });

            builder.ConfigureLogging(ConfigureLogging);
            builder.UseConsoleLifetime();
            builder.ConfigureServices((c, s) => { ConfigureServices(s); });

            var host = builder.Build();
            using (host)
            {
                host.Run();
            }
        }

        private static void ConfigureLogging(HostBuilderContext context, ILoggingBuilder builder)
        {
            builder.AddConsole();
            builder.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            using (var serviceScope = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>()
            .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<EntekraContext>())
                {
                    context.Database.EnsureDeleted();
                    //context.Database.EnsureCreated();
                    context.Database.Migrate();
                    
                    context.Database.ExecuteSqlRaw(DbViews.DropAllViews);
                    context.Database.ExecuteSqlRaw(DbViews.ReportFormStauses_NonConformanceReport);
                    context.Database.ExecuteSqlRaw(DbViews.ReportFormStauses_ChangeRequest);
                    context.Database.ExecuteSqlRaw(DbViews.ReportTimeExpiredForms_NonConformanceReport);
                    context.Database.ExecuteSqlRaw(DbViews.ReportTimeExpiredForms_ChangeRequest);
                    context.Database.ExecuteSqlRaw(DbViews.ReportAreaResponsibleForIssue_NonConformanceReport);
                    context.Database.ExecuteSqlRaw(DbViews.ReportAreaResponsibleForIssue_ChangeRequest);
                    context.Database.ExecuteSqlRaw(DbViews.ReportOpenForms_NonConformanceReport);
                    context.Database.ExecuteSqlRaw(DbViews.ReportOpenForms_ChangeRequest);
                }
            }

            // Automapper
            services.AddAutoMapper(typeof(MappingProfiles).GetTypeInfo().Assembly);
            services.AddTransient<IDbRepository, DbRepository>();
            services.AddTransient<IApiRepository, ApiRepository>();
            services.AddTransient<IApiService, ApiService>();
            services.AddTransient<IProjectService, ProjectService>();
        }

        private static void ConfigureEnvironment(HostBuilderContext context)
        {
            AppConfig.Urls.ProjectsUrl = context.Configuration.GetValue<string>("Urls:ProjectsUrl", null);
            AppConfig.Urls.IssuessUrl = context.Configuration.GetValue<string>("Urls:IssuessUrl", null);
            AppConfig.Urls.CheckListUrl = context.Configuration.GetValue<string>("Urls:CheckListUrl", null);
            AppConfig.ProjectIdsToSkip = context.Configuration.GetSection("ProjectSetting:ProjectIdsToSkip").Get<List<int>>();
            AppConfig.CheckListFormsToReport = context.Configuration.GetSection("ProjectSetting:CheckListFormsToReport").Get<List<string>>();
        }
    }
}
