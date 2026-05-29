using AppCore.Enums;

namespace AppCore.Dto;

public record ContactSummaryDto
{
    public Guid Id { get; init; }
    public string ContactType { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public ContactStatus Status { get; init; }
    public string? CreatedByUserId { get; init; }
    public DateTime CreatedAt { get; init; }
}