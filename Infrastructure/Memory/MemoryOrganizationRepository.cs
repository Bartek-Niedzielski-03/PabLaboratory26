using AppCore.Entities;
using AppCore.Enums;
using AppCore.Repositories;

namespace Infrastructure.Memory;

public class MemoryOrganizationRepository : MemoryGenericRepository<Organization>, IOrganizationRepository
{
    public MemoryOrganizationRepository()
    {
        var organization1 = new Organization
        {
            Id = Guid.NewGuid(),
            Name = "Fundacja Rozwoju",
            Type = OrganizationType.Foundation
        };

        var organization2 = new Organization
        {
            Id = Guid.NewGuid(),
            Name = "Stowarzyszenie Edukacja",
            Type = OrganizationType.Association
        };

        _data[organization1.Id] = organization1;
        _data[organization2.Id] = organization2;
    }

    public Task<IEnumerable<Organization>> FindByTypeAsync(OrganizationType type)
    {
        var result = _data.Values
            .Where(o => o.Type == type)
            .AsEnumerable();

        return Task.FromResult(result);
    }

    public Task<IEnumerable<Person>> GetMembersAsync(Guid organizationId)
    {
        var organization = _data.Values.FirstOrDefault(o => o.Id == organizationId);

        IEnumerable<Person> result = organization?.Members ?? Enumerable.Empty<Person>();
        return Task.FromResult(result);
    }
}