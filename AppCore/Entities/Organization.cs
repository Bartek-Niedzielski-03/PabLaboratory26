using AppCore.Enums;

namespace AppCore.Entities;

public class Organization : Contact
{
    public string Name { get; set; } = string.Empty;
    public OrganizationType Type { get; set; } = OrganizationType.Other;

    public string? KRS { get; set; }
    public string? Website { get; set; }
    public string? Mission { get; set; }

    public List<Person> Members { get; set; } = new();
    public Person? PrimaryContact { get; set; }

    public override string GetDisplayName() => Name;
}