using AppCore.Entities;
using AppCore.Enums;

namespace AppCore.Dto;

public record PersonDto : ContactBaseDto
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string? Position { get; init; }
    public DateTime? BirthDate { get; init; }
    public Gender Gender { get; init; }
    public Guid? EmployerId { get; init; }

    public static PersonDto FromEntity(Person person)
    {
        return new PersonDto
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            Position = person.Position,
            BirthDate = person.BirthDate,
            Gender = person.Gender,
            EmployerId = person.Employer?.Id,

            Email = person.Email,
            Phone = person.Phone,
            Address = AddressDto.FromEntity(person.Address),
            Status = person.Status,
            Tags = person.Tags.Select(t => t.Name).ToList(),
            CreatedAt = person.CreatedAt
        };
    }
}

public record CreatePersonDto(
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string? Position,
    DateTime? BirthDate,
    Gender Gender,
    Guid? EmployerId,
    AddressDto? Address
)
{
    public Person ToEntity(Company? employer = null)
    {
        return new Person
        {
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Phone = Phone,
            Position = Position,
            BirthDate = BirthDate,
            Gender = Gender,
            Employer = employer,
            Address = AddressDto.ToEntity(Address),
            Status = ContactStatus.Active,
            CreatedAt = DateTime.UtcNow
        };
    }
}

public record UpdatePersonDto(
    string? FirstName,
    string? LastName,
    string? Email,
    string? Phone,
    string? Position,
    DateTime? BirthDate,
    Gender? Gender,
    Guid? EmployerId,
    AddressDto? Address,
    ContactStatus? Status
)
{
    public void ApplyToEntity(Person person, Company? employer = null)
    {
        if (FirstName is not null)
            person.FirstName = FirstName;

        if (LastName is not null)
            person.LastName = LastName;

        if (Email is not null)
            person.Email = Email;

        if (Phone is not null)
            person.Phone = Phone;

        if (Position is not null)
            person.Position = Position;

        if (BirthDate.HasValue)
            person.BirthDate = BirthDate.Value;

        if (Gender.HasValue)
            person.Gender = Gender.Value;

        if (Address is not null)
            person.Address = AddressDto.ToEntity(Address);

        if (Status.HasValue)
            person.Status = Status.Value;

        person.Employer = employer;
        person.UpdatedAt = DateTime.UtcNow;
    }
}