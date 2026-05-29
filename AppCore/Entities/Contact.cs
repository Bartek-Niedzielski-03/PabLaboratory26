using AppCore.Enums;
using AppCore.ValueObjects;

namespace AppCore.Entities;

public abstract class Contact : EntityBase
{
    public string Email { get; set; } = string.Empty;
    public PhoneNumber Phone { get; set; } = PhoneNumber.Create("000000000");

    public Address? Address { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ContactStatus Status { get; set; } = ContactStatus.Active;
    
    public string? CreatedByUserId { get; set; }

    public List<Tag> Tags { get; set; } = new();
    public List<Note> Notes { get; set; } = new();

    public virtual string GetDisplayName() => Email;
}