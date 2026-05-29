using AppCore.Dto;
using AppCore.Entities;
using AppCore.Exceptions;
using AppCore.Repositories;
using AppCore.ValueObjects;

namespace AppCore.Services;

public class ContactService : IContactService
{
    private readonly IContactUnitOfWork _unitOfWork;

    public ContactService(IContactUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Contact> AddContactAsync(CreateContactDto dto, string userId)
    {
        Contact contact = dto switch
        {
            CreatePersonContactDto p => new Person
            {
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email,
                Phone = PhoneNumber.Create(p.Phone),
                Position = p.Position,
                BirthDate = p.BirthDate,
                Gender = p.Gender,
                Address = AddressDto.ToEntity(p.Address),
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = userId
            },
            CreateCompanyContactDto c => new Company
            {
                Name = c.Name,
                Email = c.Email,
                Phone = PhoneNumber.Create(c.Phone),
                NIP = c.Nip,
                Industry = c.Industry,
                Website = c.Website,
                Address = AddressDto.ToEntity(c.Address),
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = userId
            },
            CreateOrganizationContactDto o => new Organization
            {
                Name = o.Name,
                Email = o.Email,
                Phone = PhoneNumber.Create(o.Phone),
                Type = o.Type,
                Mission = o.Mission,
                Website = o.Website,
                Address = AddressDto.ToEntity(o.Address),
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = userId
            },
            _ => throw new ArgumentException("Nieznany typ kontaktu.")
        };

        Contact added;

        switch (contact)
        {
            case Person person:
                added = await _unitOfWork.Persons.AddAsync(person);
                break;
            case Company company:
                added = await _unitOfWork.Companies.AddAsync(company);
                break;
            case Organization org:
                added = await _unitOfWork.Organizations.AddAsync(org);
                break;
            default:
                throw new ArgumentException("Nieznany typ kontaktu.");
        }

        await _unitOfWork.SaveChangesAsync();
        return added;
    }

    public async Task DeleteContactAsync(Guid id, string userId)
    {
        var contact = await FindContactByIdAsync(id);

        if (contact is null)
            throw new ContactNotFoundException($"Kontakt o id={id} nie istnieje.");

        if (contact.CreatedByUserId != userId)
            throw new UnauthorizedAccessException("Brak uprawnień do usunięcia tego kontaktu.");

        switch (contact)
        {
            case Person:
                await _unitOfWork.Persons.RemoveByIdAsync(id);
                break;
            case Company:
                await _unitOfWork.Companies.RemoveByIdAsync(id);
                break;
            case Organization:
                await _unitOfWork.Organizations.RemoveByIdAsync(id);
                break;
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<Contact> UpdateContactAsync(Guid id, CreateContactDto dto, string userId)
    {
        var contact = await FindContactByIdAsync(id);

        if (contact is null)
            throw new ContactNotFoundException($"Kontakt o id={id} nie istnieje.");

        if (contact.CreatedByUserId != userId)
            throw new UnauthorizedAccessException("Brak uprawnień do edycji tego kontaktu.");

        contact.Email = dto.Email;
        contact.Phone = PhoneNumber.Create(dto.Phone);
        contact.UpdatedAt = DateTime.UtcNow;

        switch (contact, dto)
        {
            case (Person p, CreatePersonContactDto pd):
                p.FirstName = pd.FirstName;
                p.LastName = pd.LastName;
                p.Position = pd.Position;
                p.BirthDate = pd.BirthDate;
                p.Gender = pd.Gender;
                await _unitOfWork.Persons.UpdateAsync(p);
                break;
            case (Company c, CreateCompanyContactDto cd):
                c.Name = cd.Name;
                c.NIP = cd.Nip;
                c.Industry = cd.Industry;
                c.Website = cd.Website;
                await _unitOfWork.Companies.UpdateAsync(c);
                break;
            case (Organization o, CreateOrganizationContactDto od):
                o.Name = od.Name;
                o.Type = od.Type;
                o.Mission = od.Mission;
                o.Website = od.Website;
                await _unitOfWork.Organizations.UpdateAsync(o);
                break;
            default:
                throw new ArgumentException("Niezgodność typu kontaktu i DTO.");
        }

        await _unitOfWork.SaveChangesAsync();
        return contact;
    }

    public async Task<PagedResult<ContactSummaryDto>> GetAllContactsPagedAsync(int page, int size)
    {
        var persons = await _unitOfWork.Persons.FindAllAsync();
        var companies = await _unitOfWork.Companies.FindAllAsync();
        var organizations = await _unitOfWork.Organizations.FindAllAsync();

        var all = persons.Cast<Contact>()
            .Concat(companies)
            .Concat(organizations)
            .OrderBy(c => c.CreatedAt)
            .ToList();

        var totalCount = all.Count;
        var items = all
            .Skip((page - 1) * size)
            .Take(size)
            .Select(ToSummaryDto)
            .ToList();

        return new PagedResult<ContactSummaryDto>(items, totalCount, page, size);
    }

    public async Task<ContactSummaryDto?> GetContactByIdAsync(Guid id)
    {
        var contact = await FindContactByIdAsync(id);
        return contact is null ? null : ToSummaryDto(contact);
    }

    private async Task<Contact?> FindContactByIdAsync(Guid id)
    {
        var person = await _unitOfWork.Persons.FindByIdAsync(id);
        if (person is not null) return person;

        var company = await _unitOfWork.Companies.FindByIdAsync(id);
        if (company is not null) return company;

        var org = await _unitOfWork.Organizations.FindByIdAsync(id);
        return org;
    }

    private static ContactSummaryDto ToSummaryDto(Contact contact) => new()
    {
        Id = contact.Id,
        ContactType = contact.GetType().Name,
        DisplayName = contact.GetDisplayName(),
        Email = contact.Email,
        Phone = contact.Phone.Value,
        Status = contact.Status,
        CreatedByUserId = contact.CreatedByUserId,
        CreatedAt = contact.CreatedAt
    };
}