using DataStoreFramework.AwsS3;
using DataStoreFramework.AwsS3.Orchestration;
using DataStoreFramework.AzureBlob;
using DataStoreFramework.AzureBlob.Orchestration;
using DataStoreFramework.Orchestration;
using DataStoreFramework.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace StaticOrchestrationApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataStoreOrchestration(opts =>
            {
                opts.UseStaticConfiguration(
                    builder => builder
                        .AddAwsS3DataStore(
                            opt => opt
                                .WithName("aws-test-1")
                                .UseRegion("eu-west-2")
                                .SetMaxFilePartSize(5000)
                                .ForcePathStyle()
                                .UseServiceEndpoint("http://localhost:4566")
                        )
                        .AddAzureBlobDataStore(
                            opt => opt
                                .WithName("azure-test-1")
                                .UseServiceEndpoint("...")
                                .UseContainer("azure-test-container")
                                .SetMaxFilePartSize(5000)
                        )
                    );
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StaticOrchestrationApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StaticOrchestrationApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
