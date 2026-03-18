using AppCore.Entities;
using AppCore.Repositories;

namespace Infrastructure.Memory;

public class MemoryCompanyRepository : MemoryGenericRepository<Company>, ICompanyRepository
{
    public MemoryCompanyRepository()
    {
        var company1 = new Company
        {
            Id = Guid.NewGuid(),
            Name = "ABC Sp. z o.o.",
            NIP = "1234567890"
        };

        var company2 = new Company
        {
            Id = Guid.NewGuid(),
            Name = "XYZ S.A.",
            NIP = "9876543210"
        };

        _data[company1.Id] = company1;
        _data[company2.Id] = company2;
    }

    public Task<IEnumerable<Company>> FindByNameAsync(string name)
    {
        var result = _data.Values
            .Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .AsEnumerable();

        return Task.FromResult(result);
    }

    public Task<Company?> FindByNipAsync(string nip)
    {
        var result = _data.Values.FirstOrDefault(c => c.NIP == nip);
        return Task.FromResult(result);
    }

    public Task<IEnumerable<Person>> GetEmployeesAsync(Guid companyId)
    {
        var company = _data.Values.FirstOrDefault(c => c.Id == companyId);

        IEnumerable<Person> result = company?.Employees ?? Enumerable.Empty<Person>();
        return Task.FromResult(result);
    }
}