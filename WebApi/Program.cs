using AppCore.Module;
using Infrastructure;
using Infrastructure.Security;
using Infrastructure.Seed;

namespace WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi();

        builder.Services.AddExceptionHandler<ProblemDetailsExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.AddContactsEfModule(builder.Configuration);
        builder.Services.AddContactsCoreModule(builder.Configuration);
        builder.Services.AddSingleton<JwtSettings>();
        builder.Services.AddJwt(new JwtSettings(builder.Configuration));

        //builder.Services.AddSingleton<IPersonRepository, MemoryPersonRepository>();
        //builder.Services.AddSingleton<ICompanyRepository, MemoryCompanyRepository>();
        //builder.Services.AddSingleton<IOrganizationRepository, MemoryOrganizationRepository>();

        //builder.Services.AddSingleton<IContactUnitOfWork, MemoryContactUnitOfWork>();
        //builder.Services.AddSingleton<IPersonService, MemoryPersonService>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            using var scope = app.Services.CreateScope();

            var seeders = scope.ServiceProvider
                .GetServices<IDataSeeder>()
                .OrderBy(s => s.Order);

            foreach (var seeder in seeders)
                await seeder.SeedAsync();
        }

        app.UseExceptionHandler();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}