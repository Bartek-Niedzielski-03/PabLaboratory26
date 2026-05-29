using System.Text.Json;
using System.Text.Json.Serialization;
using AppCore.Enums;

namespace AppCore.Dto;

[JsonConverter(typeof(CreateContactDtoConverter))]
public abstract record CreateContactDto
{
    public string ContactType { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public AddressDto? Address { get; init; }
}

public record CreatePersonContactDto : CreateContactDto
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string? Position { get; init; }
    public DateTime? BirthDate { get; init; }
    public Gender Gender { get; init; }
    public Guid? EmployerId { get; init; }
}

public record CreateCompanyContactDto : CreateContactDto
{
    public string Name { get; init; } = string.Empty;
    public string? Nip { get; init; }
    public string? Regon { get; init; }
    public string? Industry { get; init; }
    public string? Website { get; init; }
}

public record CreateOrganizationContactDto : CreateContactDto
{
    public string Name { get; init; } = string.Empty;
    public OrganizationType Type { get; init; }
    public string? Mission { get; init; }
    public string? Website { get; init; }
}

public class CreateContactDtoConverter : JsonConverter<CreateContactDto>
{
    public override CreateContactDto? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        if (!root.TryGetProperty("contactType", out var typeProp))
            throw new JsonException("Brak pola 'contactType'.");

        var contactType = typeProp.GetString();
        var json = root.GetRawText();

        return contactType switch
        {
            "Person"       => JsonSerializer.Deserialize<CreatePersonContactDto>(json, options),
            "Company"      => JsonSerializer.Deserialize<CreateCompanyContactDto>(json, options),
            "Organization" => JsonSerializer.Deserialize<CreateOrganizationContactDto>(json, options),
            _ => throw new JsonException($"Nieznany typ kontaktu: '{contactType}'.")
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        CreateContactDto value,
        JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)value, options);
    }
}