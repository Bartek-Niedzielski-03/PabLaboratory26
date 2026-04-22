using AppCore.Entities;
using AppCore.Enums;
using Infrastructure.EntityFramework.Context;
using Infrastructure.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Seed;

public class PersonDbSeeder : IDataSeeder
{
    public int Order => 2;

    private readonly ContactsDbContext _context;
    private readonly ILogger<PersonDbSeeder> _logger;
    private readonly UserManager<CrmUser> _userManager;
    private readonly IPasswordHasher<CrmUser> _passwordHasher;

    public PersonDbSeeder(
        ContactsDbContext context,
        ILogger<PersonDbSeeder> logger,
        UserManager<CrmUser> userManager,
        IPasswordHasher<CrmUser> passwordHasher)
    {
        _context = context;
        _logger = logger;
        _userManager = userManager;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedAsync()
    {
        await ResetTestUserPassword();

        if (await _context.People.AnyAsync())
        {
            _logger.LogInformation("Osoby już istnieją w bazie — pomijam seed Person.");
            return;
        }

        var createdAt = DateTime.UtcNow;

        var people = new List<Person>
        {
            new()
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jan.kowalski@example.com",
                Phone = "111222333",
                Position = "Handlowiec",
                BirthDate = new DateTime(1995, 5, 10),
                Gender = Gender.Male,
                Status = ContactStatus.Active,
                CreatedAt = createdAt
            },
            new()
            {
                FirstName = "Anna",
                LastName = "Nowak",
                Email = "anna.nowak@example.com",
                Phone = "444555666",
                Position = "Specjalista ds. wsparcia",
                BirthDate = new DateTime(1997, 8, 15),
                Gender = Gender.Female,
                Status = ContactStatus.Active,
                CreatedAt = createdAt
            },
            new()
            {
                FirstName = "Piotr",
                LastName = "Wiśniewski",
                Email = "piotr.wisniewski@example.com",
                Phone = "777888999",
                Position = "Tester",
                BirthDate = new DateTime(1993, 2, 20),
                Gender = Gender.Male,
                Status = ContactStatus.Active,
                CreatedAt = createdAt
            }
        };

        await _context.People.AddRangeAsync(people);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Dodano przykładowe kontakty typu Person.");
    }

    private async Task ResetTestUserPassword()
    {
        const string email = "sales@crm.local";
        const string newPassword = "Sales@123!";

        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            _logger.LogWarning("Nie znaleziono użytkownika {Email}", email);
            return;
        }

        user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            _logger.LogError("Błąd ustawiania hasła: {Errors}",
                string.Join("; ", result.Errors.Select(e => e.Description)));
            return;
        }

        _logger.LogInformation("Ustawiono hasło dla użytkownika {Email}", email);
    }
}