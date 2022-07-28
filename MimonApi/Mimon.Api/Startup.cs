using System.Reflection;
using Microsoft.OpenApi.Models;
using Mimon.BusinessLogic.Repositories.Database;
using Mimon.BusinessLogic.Repositories.Users;
using Mimon.BusinessLogic.Repositories.UsersRelations;
using Mimon.BusinessLogic.Services.Users;

namespace Mimon.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var postgresSection = Configuration.GetSection("PostgreSql");
        services.Configure<DatabaseOptions>(postgresSection);
        services.AddDbContext<DatabaseContext>(ServiceLifetime.Singleton, ServiceLifetime.Singleton);

        var useFakeRepositories = Configuration.GetValue<bool>("FakeRepositories");
        if (useFakeRepositories)
        {
            services.AddSingleton<IRelationsRepository, FakeRelationsRepository>();
            services.AddSingleton<IUsersRepository, FakeUsersRepository>();
        }
        else
        {
            services.AddTransient<IRelationsRepository, RelationsRepository>();
            services.AddTransient<IUsersRepository, UsersRepository>();
        }

        services.AddTransient<IUsersService, UsersService>();

        services.AddControllers();
        services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "MimonApi", Version = "v1" }); });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MimonApi v1");
                c.RoutePrefix = "swagger";
            });
        }

        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseWebSockets();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }

    public IConfiguration Configuration { get; }
}