using AppCore.Dto;
using AppCore.Entities;

namespace AppCore.Services;

public interface IContactService
{
    Task<Contact> AddContactAsync(CreateContactDto dto, string userId);
    Task<Contact> UpdateContactAsync(Guid id, CreateContactDto dto, string userId);
    Task DeleteContactAsync(Guid id, string userId);
    Task<PagedResult<ContactSummaryDto>> GetAllContactsPagedAsync(int page, int size);
    Task<ContactSummaryDto?> GetContactByIdAsync(Guid id);
}