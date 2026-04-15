using AppCore.Dto;
using AppCore.Entities;
using AppCore.Exceptions;
using AppCore.Repositories;

namespace AppCore.Services;

public class PersonService : IPersonService
{
    private readonly IContactUnitOfWork _unitOfWork;

    public PersonService(IContactUnitOfWork unitOfWork)
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

    public async Task<PersonDto?> GetById(Guid id)
    {
        var person = await _unitOfWork.Persons.FindByIdAsync(id);

        if (person is null)
            return null;

        return PersonDto.FromEntity(person);
    }

    public async Task<PersonDto> GetPerson(Guid personId)
    {
        var person = await _unitOfWork.Persons.FindByIdAsync(personId);

        if (person is null)
            throw new ContactNotFoundException($"Person with id={personId} not found!");

        return PersonDto.FromEntity(person);
    }

    public async Task<Person> AddPerson(CreatePersonDto dto)
    {
        Company? employer = null;

        if (dto.EmployerId.HasValue)
        {
            employer = await _unitOfWork.Companies.FindByIdAsync(dto.EmployerId.Value);
        }

        var person = dto.ToEntity(employer);
        var created = await _unitOfWork.Persons.AddAsync(person);

        await _unitOfWork.SaveChangesAsync();

        return created;
    }

    public async Task<Person> UpdatePerson(Guid id, UpdatePersonDto dto)
    {
        var person = await _unitOfWork.Persons.FindByIdAsync(id);

        if (person is null)
            throw new ContactNotFoundException($"Person with id={id} not found!");

        Company? employer = person.Employer;

        if (dto.EmployerId.HasValue)
        {
            employer = await _unitOfWork.Companies.FindByIdAsync(dto.EmployerId.Value);
        }

        dto.ApplyToEntity(person, employer);

        var updated = await _unitOfWork.Persons.UpdateAsync(person);

        await _unitOfWork.SaveChangesAsync();

        return updated;
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
            throw new ContactNotFoundException($"Person with id={id} not found!");

        person.Tags.Add(new Tag
        {
            Id = Guid.NewGuid(),
            Name = tagName
        });

        await _unitOfWork.Persons.UpdateAsync(person);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<Note> AddNoteToPerson(Guid personId, CreateNoteDto noteDto)
    {
        var person = await _unitOfWork.Persons.FindByIdAsync(personId);

        if (person is null)
            throw new ContactNotFoundException($"Person with id={personId} not found!");

        if (person.Notes is null)
            person.Notes = new List<Note>();

        var note = new Note
        {
            Id = Guid.NewGuid(),
            Content = noteDto.Content,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = string.Empty
        };

        person.Notes.Add(note);

        await _unitOfWork.Persons.UpdateAsync(person);
        await _unitOfWork.SaveChangesAsync();

        return note;
    }

    public async Task DeleteNoteFromPerson(Guid personId, Guid noteId)
    {
        var person = await _unitOfWork.Persons.FindByIdAsync(personId);

        if (person is null)
            throw new ContactNotFoundException($"Person with id={personId} not found!");

        var note = person.Notes.FirstOrDefault(n => n.Id == noteId);

        if (note is null)
            throw new ContactNotFoundException($"Note with id={noteId} not found for person with id={personId}!");

        person.Notes.Remove(note);

        await _unitOfWork.Persons.UpdateAsync(person);
        await _unitOfWork.SaveChangesAsync();
    }
}