using AppCore.Dto;
using AppCore.Entities;

namespace AppCore.Repositories;

public interface IContactRepository : IGenericRepositoryAsync<Contact>
{
    Task<PagedResult<Contact>> SearchAsync(ContactSearchDto searchDto);
    Task<IEnumerable<Contact>> FindByTagAsync(string tag);

    Task AddNoteAsync(Guid contactId, Note note);
    Task<IEnumerable<Note>> GetNotesAsync(Guid contactId);

    Task AddTagAsync(Guid contactId, Tag tag);
    Task RemoveTagAsync(Guid contactId, string tagName);
}