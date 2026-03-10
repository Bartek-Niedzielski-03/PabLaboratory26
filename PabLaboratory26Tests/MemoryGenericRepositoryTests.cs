using AppCore.Entities;
using AppCore.Repositories;
using Infrastructure.Memory;

namespace PabLaboratory26Tests;

public class MemoryGenericRepositoryTests
{
    private readonly IGenericRepositoryAsync<Person> _repo;

    public MemoryGenericRepositoryTests()
    {
        _repo = new MemoryGenericRepository<Person>();
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntity()
    {
        var person = new Person
        {
            FirstName = "Adam",
            LastName = "Nowak",
            Email = "adam@test.pl",
            Phone = "123456789"
        };

        await _repo.AddAsync(person);
        var result = await _repo.FindByIdAsync(person.Id);

        Assert.NotNull(result);
        Assert.Equal(person.Id, result!.Id);
        Assert.Equal("Adam", result.FirstName);
    }

    [Fact]
    public async Task FindByIdAsync_ShouldReturnNull_WhenEntityDoesNotExist()
    {
        var result = await _repo.FindByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task FindAllAsync_ShouldReturnAllEntities()
    {
        var p1 = new Person { FirstName = "Jan", LastName = "Kowalski", Email = "jan@test.pl", Phone = "111" };
        var p2 = new Person { FirstName = "Anna", LastName = "Nowak", Email = "anna@test.pl", Phone = "222" };

        await _repo.AddAsync(p1);
        await _repo.AddAsync(p2);

        var all = await _repo.FindAllAsync();

        Assert.Equal(2, all.Count());
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        var person = new Person
        {
            FirstName = "Adam",
            LastName = "Nowak",
            Email = "adam@test.pl",
            Phone = "123"
        };

        await _repo.AddAsync(person);

        person.FirstName = "Ewa";
        await _repo.UpdateAsync(person);

        var updated = await _repo.FindByIdAsync(person.Id);

        Assert.NotNull(updated);
        Assert.Equal("Ewa", updated!.FirstName);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenEntityDoesNotExist()
    {
        var person = new Person
        {
            FirstName = "Ghost",
            LastName = "User",
            Email = "ghost@test.pl",
            Phone = "000"
        };

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _repo.UpdateAsync(person));
    }

    [Fact]
    public async Task RemoveByIdAsync_ShouldRemoveEntity()
    {
        var person = new Person
        {
            FirstName = "Adam",
            LastName = "Nowak",
            Email = "adam@test.pl",
            Phone = "123"
        };

        await _repo.AddAsync(person);
        await _repo.RemoveByIdAsync(person.Id);

        var result = await _repo.FindByIdAsync(person.Id);

        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveByIdAsync_ShouldThrowException_WhenEntityDoesNotExist()
    {
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _repo.RemoveByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task FindPagedAsync_ShouldReturnProperPage()
    {
        for (int i = 1; i <= 5; i++)
        {
            await _repo.AddAsync(new Person
            {
                FirstName = $"Name{i}",
                LastName = $"Last{i}",
                Email = $"person{i}@test.pl",
                Phone = $"{i}"
            });
        }

        var page = await _repo.FindPagedAsync(2, 2);

        Assert.Equal(5, page.TotalCount);
        Assert.Equal(2, page.Page);
        Assert.Equal(2, page.PageSize);
        Assert.Equal(2, page.Items.Count);
        Assert.True(page.HasPrevious);
        Assert.True(page.HasNext);
    }
}