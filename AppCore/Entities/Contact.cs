using AppCore.Enums;

namespace AppCore.Entities;

public abstract class Contact : EntityBase
{
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    public Address? Address { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ContactStatus Status { get; set; } = ContactStatus.Active;

    public List<Tag> Tags { get; set; } = new();
    public List<Note> Notes { get; set; } = new();

    public virtual string GetDisplayName() => Email;
}