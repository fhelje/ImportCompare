using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplication1 {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            var environmentConfig = new EnvironmentsConfiguration();
            Configuration.GetSection("EnvironmentsConfig").Bind(environmentConfig);
            
            services.AddSpaStaticFiles(configuration => {
                configuration.RootPath = "wwwroot";
            });
            services.AddMemoryCache();
            services.AddSingleton(environmentConfig);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseMvc();
            app.UseSpa(spa => {
                spa.Options.SourcePath = "wwwroot";
            });
        }
    }

    public class EnvironmentsConfiguration {
        public EnvironmentConfig[] Environments { get; set; }
    }

    public class EnvironmentConfig {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
    }
}
