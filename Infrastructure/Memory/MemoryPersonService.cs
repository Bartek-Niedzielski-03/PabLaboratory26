using AppCore.Dto;
using AppCore.Entities;
using AppCore.Repositories;
using AppCore.Services;

namespace Infrastructure.Memory;

public class MemoryPersonService : IPersonService
{
    private readonly IContactUnitOfWork _unitOfWork;

    public MemoryPersonService(IContactUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<PersonDto>> FindAllPeoplePaged(int page, int size)
    {
        var people = await _unitOfWork.Persons.FindPagedAsync(page, size);
        var items = people.Items
            .Select(PersonDto.FromEntity)
            .ToList();

        return new PagedResult<PersonDto>(
            items,
            people.TotalCount,
            people.Page,
            people.PageSize
        );
    }

    public async Task<PersonDto?> FindByIdAsync(Guid id)
    {
        var person = await _unitOfWork.Persons.FindByIdAsync(id);

        if (person is null)
            return null;

        return PersonDto.FromEntity(person);
    }

    public async Task<PersonDto> CreateAsync(CreatePersonDto dto)
    {
        Company? employer = null;

        if (dto.EmployerId.HasValue)
        {
            employer = await _unitOfWork.Companies.FindByIdAsync(dto.EmployerId.Value);
        }

        var person = dto.ToEntity(employer);
        var created = await _unitOfWork.Persons.AddAsync(person);

        await _unitOfWork.SaveChangesAsync();

        return PersonDto.FromEntity(created);
    }

    public async Task<PersonDto> UpdateAsync(Guid id, UpdatePersonDto dto)
    {
        var person = await _unitOfWork.Persons.FindByIdAsync(id);

        if (person is null)
            throw new KeyNotFoundException($"Person with id {id} was not found.");

        Company? employer = person.Employer;

        if (dto.EmployerId.HasValue)
        {
            employer = await _unitOfWork.Companies.FindByIdAsync(dto.EmployerId.Value);
        }

        dto.ApplyToEntity(person, employer);

        var updated = await _unitOfWork.Persons.UpdateAsync(person);

        await _unitOfWork.SaveChangesAsync();

        return PersonDto.FromEntity(updated);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _unitOfWork.Persons.RemoveByIdAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task AddTagAsync(Guid id, string tagName)
    {
        var person = await _unitOfWork.Persons.FindByIdAsync(id);

        if (person is null)
            throw new KeyNotFoundException($"Person with id {id} was not found.");

        person.Tags.Add(new Tag
        {
            Id = Guid.NewGuid(),
            Name = tagName
        });

        await _unitOfWork.Persons.UpdateAsync(person);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task AddNoteAsync(Guid id, string noteContent, string createdBy)
    {
        var person = await _unitOfWork.Persons.FindByIdAsync(id);

        if (person is null)
            throw new KeyNotFoundException($"Person with id {id} was not found.");

        person.Notes.Add(new Note
        {
            Id = Guid.NewGuid(),
            Content = noteContent,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        });

        await _unitOfWork.Persons.UpdateAsync(person);
        await _unitOfWork.SaveChangesAsync();
    }
}