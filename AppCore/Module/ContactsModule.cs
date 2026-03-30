using AppCore.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.Module;

public static class ContactsModule
{
    public static IServiceCollection AddContactsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        //rejestracja walidatorów
        services.AddValidatorsFromAssemblyContaining<CreatePersonDtoValidator>();

        //wlaczenie automatycznej walidacji
        services.AddFluentValidationAutoValidation();

        return services;
    }
}