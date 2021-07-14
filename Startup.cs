using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenFIS.Repositories;
using OpenFIS.Services;

namespace OpenFIS
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;
        public IConfiguration Configuration { get; }
        private string _fisDbConnectionString;

        public void ConfigureServices(IServiceCollection services)
        {
            _fisDbConnectionString = Configuration["FisDb:ConnectionString"];
            services.AddDbContext<FisDbContext>(x => x.UseNpgsql(_fisDbConnectionString).UseSnakeCaseNamingConvention());
            services.AddScoped<IAthleteRepository, AthleteRepository>();
            services.AddScoped<IAthleteService, AthleteService>();
            services.AddScoped<ICompetitionRepository, CompetitionRepository>();
            services.AddScoped<ICompetitionService, CompetitionService>();
            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(x => x.MapControllers());
        }
    }
}
