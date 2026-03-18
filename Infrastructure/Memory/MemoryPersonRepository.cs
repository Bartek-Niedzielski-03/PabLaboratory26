using AppCore.Entities;
using AppCore.Enums;
using AppCore.Repositories;

namespace Infrastructure.Memory;

public class MemoryPersonRepository : MemoryGenericRepository<Person>, IPersonRepository
{
    public MemoryPersonRepository()
    {
        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = "ABC Sp. z o.o.",
            NIP = "1234567890"
        };

        var organization = new Organization
        {
            Id = Guid.NewGuid(),
            Name = "Fundacja Rozwoju",
            Type = OrganizationType.Foundation
        };

        var person1 = new Person
        {
            Id = Guid.NewGuid(),
            FirstName = "Adam",
            LastName = "Nowak",
            Gender = Gender.Male,
            Email = "adam.nowak@example.com",
            Phone = "111222333",
            Employer = company,
            Organization = organization
        };

        var person2 = new Person
        {
            Id = Guid.NewGuid(),
            FirstName = "Ewa",
            LastName = "Kowalska",
            Gender = Gender.Female,
            Email = "ewa.kowalska@example.com",
            Phone = "444555666",
            Employer = company,
            Organization = organization
        };

        _data[person1.Id] = person1;
        _data[person2.Id] = person2;
    }

    public Task<IEnumerable<Person>> FindByEmployerAsync(Guid companyId)
    {
        var result = _data.Values
            .Where(p => p.Employer is not null && p.Employer.Id == companyId)
            .AsEnumerable();

        return Task.FromResult(result);
    }

    public Task<IEnumerable<Person>> FindByOrganizationAsync(Guid organizationId)
    {
        var result = _data.Values
            .Where(p => p.Organization is not null && p.Organization.Id == organizationId)
            .AsEnumerable();

        return Task.FromResult(result);
    }
}