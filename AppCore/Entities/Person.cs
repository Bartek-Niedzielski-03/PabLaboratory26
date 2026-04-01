using AppCore.Enums;

namespace AppCore.Entities;

public class Person : Contact
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }

    public DateTime? BirthDate { get; set; }
    public Gender Gender { get; set; } = Gender.NotSpecified;

    public string? Position { get; set; }

    public Company? Employer { get; set; }
    
    public Organization? Organization { get; set; }

    public override string GetDisplayName() => $"{FirstName} {LastName}".Trim();
}