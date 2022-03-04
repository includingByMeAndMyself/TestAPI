using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.IO;
using Timesheet.API.Models;
using Timesheet.BussinessLogic.Services;
using Timesheet.DAL.CSV.Infrastructure;
using Timesheet.DAL.CSV.Repositories;
using Timesheet.DAL.MSSQL;
using Timesheet.Domain.Interfaces.IClient;
using Timesheet.Domain.Interfaces.IRepository;
using Timesheet.Domain.Interfaces.IService;
using Timesheet.Itegrations.GitHub;

namespace Timesheet.API
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
            services.AddAutoMapper(typeof(ApiMappingProfile), typeof(DataAccessMappingProfile));

            services.AddTransient<IValidator<CreateTimeLogRequest>, TimeLogFluentValidator>();
            services.AddTransient<IValidator<LoginRequest>, LoginRequestFluentValidator>();

            services.AddTransient<IAuthService, AuthService>();
            
            //services.AddTransient<ITimesheetRepository, TimesheetRepository>();
            
            services.AddTransient<ITimesheetRepository, DAL.MSSQL.Repositories.TimesheetRepository>();
            services.AddTransient<ITimesheetService, TimesheetService>();
            
            //services.AddTransient<IEmployeeRepository, DAL.CSV.Repositories.EmployeeRepository>();
            
            services.AddTransient<IEmployeeRepository, DAL.MSSQL.Repositories.EmployeeRepository>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            
            services.AddTransient<IReportService, ReportService>();
            
            services.AddTransient<IIssuesService, IssuesService>();

            services.AddTransient<IIssuesClient>(x => new IssuesClient("token"));

            services.AddSingleton(x => new CsvSettings(";", "..\\Timesheet.DAL.CSV\\Data"));

            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));

            services.AddControllers().AddFluentValidation();
            services.AddControllers().AddNewtonsoftJson();

            services.AddDbContext<TimesheetContext>(x =>
                x.UseSqlServer(Configuration.GetConnectionString("TimesheetContext")));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Timesheet API", Version = "v1" });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "Timesheet API", Version = "v2" });

                var filePath = Path.Combine(System.AppContext.BaseDirectory, "Timesheet.API.xml");
                c.IncludeXmlComments(filePath);
            });

            services.AddOpenApiDocument();

            //services.AddSwaggerDocument();

            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<ControllerBase>>();
            services.AddSingleton(typeof(ILogger), logger);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseSwagger();
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestAPI v1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "TestAPI v2");
            });

            app.UseOpenApi();

            app.UseReDoc();

            app.UseHttpsRedirection();
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<JwtAuthMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
