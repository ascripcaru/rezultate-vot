using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Extensions.NETCore.Setup;
using ElectionResults.Core.Infrastructure;
using ElectionResults.Core.Repositories;
using ElectionResults.Core.Services;
using ElectionResults.Core.Services.BlobContainer;
using ElectionResults.Core.Services.CsvDownload;
using ElectionResults.Core.Services.CsvProcessing;
using ElectionResults.Core.Storage;
using ElectionResults.WebApi.Scheduler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using ElectionResults.WebApi.Hubs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace ElectionResults.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppConfig>(options => Configuration.GetSection("settings").Bind(options));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddTransient<IResultsRepository, ResultsRepository>();
            services.AddTransient<IResultsAggregator, ResultsAggregator>();
            services.AddTransient<ICsvDownloaderJob, CsvDownloaderJob>();
            services.AddTransient<IBucketUploader, BucketUploader>();
            services.AddTransient<IElectionConfigurationSource, ElectionConfigurationSource>();
            services.AddTransient<IFileProcessor, FileProcessor>();
            services.AddTransient<IStatisticsAggregator, StatisticsAggregator>();
            services.AddTransient<IBucketRepository, BucketRepository>();
            services.AddTransient<IFileRepository, FileRepository>();
            services.AddAWSService<IAmazonDynamoDB>();
            services.AddAWSService<Amazon.S3.IAmazonS3>(new AWSOptions
            {
                Profile = "default",
                Region = RegionEndpoint.EUCentral1
            });
            services.AddDefaultAWSOptions(new AWSOptions
            {
                Region = RegionEndpoint.EUCentral1
            });
            services.AddSingleton<IHostedService, ScheduleTask>();
            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            loggerFactory.AddAWSProvider(Configuration.GetAWSLoggingConfigSection());
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseSignalR(routes =>
            {
                routes.MapHub<ElectionResultsHub>("/live-results");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
