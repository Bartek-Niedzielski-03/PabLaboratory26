using AppCore.Dto;

namespace AppCore.Services;

public interface IPersonService
{
    Task<PagedResult<PersonDto>> FindAllPeoplePaged(int page, int size);
    Task<PersonDto?> FindByIdAsync(Guid id);
    Task<PersonDto> CreateAsync(CreatePersonDto dto);
    Task<PersonDto> UpdateAsync(Guid id, UpdatePersonDto dto);
    Task DeleteAsync(Guid id);
    Task AddTagAsync(Guid id, string tagName);
    Task AddNoteAsync(Guid id, string noteContent, string createdBy);
}