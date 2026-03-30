using AppCore.Dto;
using AppCore.Entities;

namespace AppCore.Services;

public interface IPersonService
{
    Task<PagedResult<PersonDto>> FindAllPeoplePaged(int page, int size);
    Task<PersonDto?> GetById(Guid id);
    Task<Person> AddPerson(CreatePersonDto personDto);
    Task<Person> UpdatePerson(Guid id, UpdatePersonDto personDto);

    Task DeleteAsync(Guid id);
    Task AddTagAsync(Guid id, string tagName);
    Task AddNoteAsync(Guid id, string noteContent, string createdBy);
}