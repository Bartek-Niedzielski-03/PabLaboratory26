using AppCore.Module;
using Infrastructure;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi();

        builder.Services.AddExceptionHandler<ProblemDetailsExceptionHandler>();
        builder.Services.AddProblemDetails();
        
        builder.Services.AddContactsEfModule(builder.Configuration);
        builder.Services.AddContactsCoreModule(builder.Configuration);

        //builder.Services.AddSingleton<IPersonRepository, MemoryPersonRepository>();
        //builder.Services.AddSingleton<ICompanyRepository, MemoryCompanyRepository>();
        //builder.Services.AddSingleton<IOrganizationRepository, MemoryOrganizationRepository>();

        //builder.Services.AddSingleton<IContactUnitOfWork, MemoryContactUnitOfWork>();
        //builder.Services.AddSingleton<IPersonService, MemoryPersonService>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseExceptionHandler();
        app.MapControllers();

        app.Run();
    }
}